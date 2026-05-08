using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Enums;
using UnityEngine;
using UnityUtils;

namespace Battle.Window
{
    /// <summary>
    /// Utility class to define timings for an action command.
    /// </summary>
    [Serializable]
    public class ActionCommandTierGradient
    {
        [SerializeReference] private int frames;
        public int Frames => frames; 

        public void UpdateFrames(int frames)
        {
            if (frames < NumKeys) return;

            for (int i = NumKeys-1; i > 0; i--)
                if (tierKeys[i].Frame > frames)
                    RemoveKey(i); 
            
            
            this.frames = frames;
        }
        
        private List<TierKey> tierKeys = new(); 
        public IReadOnlyList<TierKey> TierKeys => tierKeys;
        
        public int NumKeys => tierKeys.Count;

        public struct TierKey
        {
            private ActionCommandTier tier;
            private int frame;
            public int Frame => frame;
            public ActionCommandTier Tier => tier; 
            public TierKey(ActionCommandTier tier, int frame)
            {
                this.tier = tier; 
                this.frame = frame;
            }
        }
        
        public ActionCommandTierGradient()
        {
            frames = 80;
            AddKey(ActionCommandTier.OKAY, 0);
            AddKey(ActionCommandTier.GOOD, 20);
            AddKey(ActionCommandTier.GREAT, 40);
            AddKey(ActionCommandTier.EXCELLENT, 60);
        }
        
        public Texture2D GetTexture()
        {
            Texture2D texture = new Texture2D(frames, 1);

            Color[] colors = new Color[frames];
            
            for (int k = 0; k < tierKeys.Count; k++)
            {
                int start = tierKeys[k].Frame;
                int end = (k < tierKeys.Count - 1) ? tierKeys[k + 1].Frame : frames;

                Color color = BattleUtils.TierToColor(tierKeys[k].Tier);

                for (int i = start; i < end; i++)
                    colors[i] = color;
            }

            texture.SetPixels(colors);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();
            
            return texture;
        }

        public int AddKey(ActionCommandTier tier, int frame)
        {
            var newKey = new TierKey(tier, frame);
            for (int i = 0; i < tierKeys.Count; i++)
            {
                if (newKey.Frame < tierKeys[i].Frame)
                {
                    tierKeys.Insert(i, newKey);
                    return i; 
                }
            }
            tierKeys.Add(newKey);
            return tierKeys.Count - 1;
        }

        public TierKey GetKey(int index)
        {
            return tierKeys[index];
        }

        public int UpdateKeyTime(int index, int frame)
        {
            //if there is a key at the new frame, dont do anything
            if (index == 0) return index;
            if (frame > Frames) return index;
            if (frame < 0) return index; 
            if (tierKeys.Any(k => k.Frame == frame)) return index;
            
            ActionCommandTier oldKeyTier = GetKey(index).Tier;
            RemoveKey(index); 
            return AddKey(oldKeyTier, frame);
        }

        public int UpdateKeyTier(int index, ActionCommandTier tier)
        {
            int oldFrame = GetKey(index).Frame;
            RemoveKey(index); 
            return AddKey(tier, oldFrame);
        }

        public void RemoveKey(int index)
        {
            if (index == 0)
                return;
            
            tierKeys.RemoveAt(index);
        }

        public ActionCommandTier Evaluate(int frame)
        {
            TierKey keyLeft = tierKeys[0];
            TierKey keyRight = tierKeys.Last();

            for (int i = 0; i < tierKeys.Count; i++)
            {
                TierKey key = tierKeys[i];
                if (key.Frame <= frame)
                    keyLeft = key;
                if (key.Frame >= frame)
                {
                    keyRight = key;
                    break; 
                }
            }

            return keyRight.Tier; 
        }
    }
}