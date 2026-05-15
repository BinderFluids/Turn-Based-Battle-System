using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EventBus;
using SelectableSystem.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityUtils;

namespace SelectableSystem
{
    public class SelectionManager : Singleton<SelectionManager>
    {
        [SerializeField] private int selectionCount;
    
        private List<ISelectable> activeItems;
        public Observer<ISelectable> CurrentItem = new Observer<ISelectable>(null); 
        [SerializeField] private bool active = false;

        [FormerlySerializedAs("defaultHighligher")] 
        [SerializeField] private SelectionHighlighter defaultHighlighter;
        [SerializeField] private SelectionHighlighter currentHighlighter;

        [SerializeField] private InputActionReference defaultConfirm;
        [SerializeField] private InputActionReference defaultNavigate;
        private InputAction confirmAction;
        private InputAction navigateAction;
        

        int GetTrueIndex(int index, List<ISelectable> items)
        {
            if (items.Count == 0) return 0; 
            int newIndex = (index % items.Count + items.Count) % items.Count;
            return newIndex;
        }

        /*
         TODO: Create some sort of "SelectionSession" class that can be used to track the selection state, and can be built
            using the builder pattern. e.g. StartSelection().WithNavigateInputAction().WithConfirmAction()... you know the one.
        */
        public void StartSelection(
            List<ISelectable> items, 
            int index = 0, 
            SelectionHighlighter highlighter = null
        )
        {
            print($"Try Start Selection with {items.Count} items");
            if (active)
            {
                Debug.LogWarning("Selection already started");
                return;
            }
            
            currentHighlighter = highlighter ?? defaultHighlighter; 
        
            active = true; 
            activeItems = items; 
            selectionCount = index;
        
            currentHighlighter.Activate();
            CurrentItem.Value = activeItems[selectionCount];
            
            SetConfirmAction(defaultConfirm);
            SetNavigateAction(defaultNavigate);
        }
        public void SetConfirmAction(InputAction confirmAction)
        {
            this.confirmAction = confirmAction ?? defaultConfirm.ToInputAction(); 
            this.confirmAction.Enable();
        }
        public void SetNavigateAction(InputAction navigateAction)
        {
            this.navigateAction = navigateAction ?? defaultNavigate.ToInputAction(); 
            this.navigateAction.Enable();
        }
        
        public void EndSelection()
        {
            if (!active)
            {
                Debug.LogWarning("No selection session to end");
                return;
            }

            currentHighlighter.Deactivate();
            currentHighlighter = null; 
        
            active = false; 
            activeItems.Clear();
        }

        private void Update()
        {
            if (!active) return;

            NavigateSelectables();
            ConfirmSelectable();
           
        }

        void ConfirmSelectable()
        {
            if (confirmAction == null) return;
            if (confirmAction.WasPressedThisFrame())
            {
                CurrentItem.Value.Select();
                EndSelection();
            
                EventBus<SelectableChosenEvent>.Raise( //TODO THIS IS KILLING ME MAN HELP ME
                    new SelectableChosenEvent
                    {
                        SelectedItem = CurrentItem.Value
                    }
                ); 
            }
        }
        
        void NavigateSelectables()
        {
            if (navigateAction == null) return;
            if (!navigateAction.WasPressedThisFrame()) return;

            Vector2 value = navigateAction.ReadValue<Vector2>();
        
            if (value.x < 0)
                ShiftSelection(-1);
            if (value.x > 0)
                ShiftSelection(1);
        }

        private UniTask currentHighlighterTask; 
        void ShiftSelection(int shift)
        {
            //if (currentHighlighterTask.Status == UniTaskStatus.Pending) return;
        
            selectionCount += shift; 
            var index = GetTrueIndex(selectionCount, activeItems);
            CurrentItem.Value = activeItems[index];

            //currentHighlighterTask = currentHighlighter.GetHighlightTask();
        }
    }
}
