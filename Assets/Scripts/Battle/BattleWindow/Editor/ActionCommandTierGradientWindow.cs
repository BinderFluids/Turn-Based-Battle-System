using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Enums;
using UnityEditor;
using UnityEngine;

namespace Battle.Window.Editor
{
    public class ActionCommandTierGradientWindow : EditorWindow
    {
        private ActionCommandTierGradient gradient;
        private UnityEngine.Object target; 
        
        private Rect gradientPreviewRect;
        private Rect[] keyRects;
        
        private const int BORDER_SIZE = 10;
        private const int GRADIENT_PREVIEW_HEIGHT = 20;
        private const int KEY_WIDTH = 10;
        private const int KEY_HEIGHT = 20;
 
        private const int FRAMES_WIDTH = 200;
        private const int FRAME_LABEL_MARGIN = 10; 

        private int selectedKeyIndex;
        private bool mouseIsDownOverKey;
        private bool needsRepaint; 
        

        public void Initialize(ActionCommandTierGradient gradient, UnityEngine.Object target)
        {
            this.gradient = gradient;
            this.target = target;
        }
        
        private void OnEnable()
        {
            titleContent = new GUIContent("Edit Tier Gradient");
            position.Set(position.x, position.y, 400, 150);
            minSize = new Vector2(200, 150);
            maxSize = new Vector2(1920, 150);
        }

        private void OnDisable()
        {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssetIfDirty(target); 
        }


        private void OnGUI() 
        {
            Draw();
            HandleInput();

            if (needsRepaint)
            {
                needsRepaint = false;
                Repaint();
            }
        }

        
        private void Draw()
        {
            float totalYOffset = BORDER_SIZE;


            Rect frameCountRect = new Rect(
                position.width - FRAMES_WIDTH - BORDER_SIZE,
                totalYOffset,
                FRAMES_WIDTH,
                EditorGUIUtility.singleLineHeight
                );
            totalYOffset += frameCountRect.yMax;
            
            GUILayout.BeginArea(frameCountRect);
            GUILayout.BeginHorizontal();
            
            //GUILayout.Label("Frames");
            EditorGUI.BeginChangeCheck();
            int newFrames = EditorGUILayout.IntField("Frames", gradient.Frames, GUILayout.Width(FRAMES_WIDTH));
            if (EditorGUI.EndChangeCheck())
                gradient.UpdateFrames(newFrames); 
                
            GUILayout.EndArea();
            GUILayout.EndHorizontal();
            
            
            gradientPreviewRect = new Rect(
                BORDER_SIZE, 
                totalYOffset, 
                position.width - BORDER_SIZE * 2,
                GRADIENT_PREVIEW_HEIGHT
                );
            totalYOffset += GRADIENT_PREVIEW_HEIGHT + BORDER_SIZE;

            //draw gradient preview
            Texture2D texture = gradient.GetTexture();
            GUI.DrawTexture(gradientPreviewRect, texture);

            //draw key handles
            keyRects = new Rect[gradient.NumKeys];
            for (int i = 0; i < gradient.NumKeys; i++)
            {
                ActionCommandTierGradient.TierKey key = gradient.GetKey(i);

                float keyPosition = (float)key.Frame / (float)gradient.Frames;
                Rect newKeyRect = new Rect(gradientPreviewRect.x + (gradientPreviewRect.width * keyPosition) - (KEY_WIDTH/2f),
                    totalYOffset, KEY_WIDTH, KEY_HEIGHT);
                
                //selection highlight
                if (i == selectedKeyIndex)
                    EditorGUI.DrawRect(new Rect(newKeyRect.x-3, newKeyRect.y-3, newKeyRect.width+6, newKeyRect.height+6), Color.black);
                
                EditorGUI.DrawRect(newKeyRect, BattleUtils.TierToColor(key.Tier));
                keyRects[i] = newKeyRect; 
            }
            totalYOffset += KEY_HEIGHT + BORDER_SIZE;
            
            //draw settings
            Rect settingsRect = new Rect(BORDER_SIZE, totalYOffset,
                position.width - BORDER_SIZE*2, position.height);
            totalYOffset += settingsRect.yMax + BORDER_SIZE;

            if (selectedKeyIndex >= gradient.TierKeys.Count)
                return; 
            
            GUILayout.BeginArea(settingsRect);
            ActionCommandTierGradient.TierKey currentKey = gradient.TierKeys[selectedKeyIndex];
            
            EditorGUI.BeginChangeCheck();
            int newFrame = EditorGUILayout.IntField("Frame", currentKey.Frame);
            if (EditorGUI.EndChangeCheck())
                gradient.UpdateKeyTime(selectedKeyIndex, newFrame);
            
            EditorGUI.BeginChangeCheck();
            ActionCommandTier newTier =
                (ActionCommandTier)EditorGUILayout.EnumPopup("Tier", currentKey.Tier);
            if (EditorGUI.EndChangeCheck())
                gradient.UpdateKeyTier(selectedKeyIndex, newTier);
            
            GUILayout.EndArea();
        }

        private void HandleInput()
        {
            Event guiEvent = Event.current;

            //left click
            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
            {
                //select key
                for (int i = 0; i < keyRects.Length; i++)
                {
                    if (keyRects[i].Contains(guiEvent.mousePosition))
                    {
                        GUI.FocusControl(null); 
                        selectedKeyIndex = i;
                        mouseIsDownOverKey = true;
                        needsRepaint = true; 
                        break;
                    }
                }
            }

            //right click
            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
            {
                //create new key
                if (!mouseIsDownOverKey)
                {
                    int keyTime = MousePositionToFrame();
                    selectedKeyIndex = gradient.AddKey(ActionCommandTier.OKAY, keyTime);
                    mouseIsDownOverKey = true; 
                }
            }
            
            //mouse up
            if (guiEvent.type == EventType.MouseUp && (guiEvent.button == 0 || guiEvent.button == 1))
                mouseIsDownOverKey = false;
            
            if (mouseIsDownOverKey && guiEvent.type == EventType.MouseDrag && guiEvent.button == 0)
            {
                int keyTime = MousePositionToFrame();
                
                if (selectedKeyIndex != 0)
                    selectedKeyIndex = gradient.UpdateKeyTime(selectedKeyIndex, keyTime);
                needsRepaint = true;
            }
            
            if (guiEvent.keyCode == KeyCode.Backspace && guiEvent.type == EventType.KeyDown)
            {
                if (selectedKeyIndex < 1) return; 
                
                gradient.RemoveKey(selectedKeyIndex);
                if (selectedKeyIndex >= gradient.NumKeys)
                {
                    selectedKeyIndex--;
                }
                needsRepaint= true;
            }
        }
        private int MousePositionToFrame()
        {
            float mousePositionX = Event.current.mousePosition.x - gradientPreviewRect.x;
            mousePositionX = Mathf.Clamp(mousePositionX, 0, gradientPreviewRect.width); 
            float mouseProgressX = Mathf.InverseLerp(0, gradientPreviewRect.width, mousePositionX);
            int keyTime = Mathf.FloorToInt(mouseProgressX * gradient.Frames);
            return keyTime; 
        }
    }
}