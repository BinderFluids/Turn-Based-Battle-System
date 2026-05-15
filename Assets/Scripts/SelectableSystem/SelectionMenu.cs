using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SelectableSystem
{
    [Serializable]
    public class SelectionMenu
    {
        private List<ISelectable> items;
        public IReadOnlyList<ISelectable> Items => items;

        private int index; 
        public int Index {
            get
            {
                if (items == null) return -1; 
                if (items.Count == 0) return -1; 
                int newIndex = (index % items.Count + items.Count) % items.Count;
                return newIndex;
            }
        }

        public ISelectable CurrentItem
        {
            get
            {
                if (Index == -1) return null; 
                return items[Index];
            }
        }
        
        [field: SerializeField] public InputAction ConfirmAction { get; private set; }
        [field: SerializeField] public InputAction NavigateAction { get; private set; }
        [field: SerializeField] public InputAction BacktrackAction { get; private set; }
        [field: SerializeField] public SelectionHighlighter Highlighter { get; private set; }
        [field: SerializeField] public bool InvertNavigation { get; private set; }
        [field: SerializeField] public bool IsLastMenu { get; private set; }
        
        public event Action onMenuActivated;
        public event Action onMenuDeactivated;
        public event Action<ISelectable> onItemSelected;
        public event Action onMenuBacktracked; 
        
        public void ShiftSelection(int shift)
        {
            index += shift;
        }

        internal void Activate()
        {
            onMenuActivated?.Invoke();
            
            ConfirmAction?.Enable();
            NavigateAction?.Enable();
            BacktrackAction?.Enable();
            Highlighter?.Activate();
        }

        public void Backtrack()
        {
            onMenuBacktracked?.Invoke();
        }
        
        public void Select()
        {
            if (CurrentItem == null) return;
            onItemSelected?.Invoke(CurrentItem);
            CurrentItem.Select();
        }

        internal void Deactivate()
        {
            onMenuDeactivated?.Invoke();
            
            Highlighter?.Deactivate();
        }

        internal SelectionMenu(List<ISelectable> items, InputAction confirmAction, InputAction navigateAction, 
            InputAction backtrackAction, SelectionHighlighter highlighter, bool invertNavigation, bool isLastMenu = false)
        {
            this.items = items;
            ConfirmAction = confirmAction;
            NavigateAction = navigateAction;
            BacktrackAction = backtrackAction;
            Highlighter = highlighter;
            InvertNavigation = invertNavigation;
            IsLastMenu = isLastMenu;
        }
    }
}