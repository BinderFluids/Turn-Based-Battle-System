using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityUtils;

namespace SelectableSystem
{
    public class SelectionManager : Singleton<SelectionManager>
    {
        [SerializeField] private int selectionCount;

        private SelectionMenuBuilder menuBuilder;
        [field: SerializeField] public SelectionMenu ActiveMenu { get; private set; }
        private Stack<SelectionMenu> previousMenuStack = new Stack<SelectionMenu>();
    
        
        public Observer<ISelectable> CurrentItem = new Observer<ISelectable>(null); 
        
        [Header("Default Settings")]
        [SerializeField] private InputActionReference defaultConfirm;
        [SerializeField] private InputActionReference defaultNavigate;
        [SerializeField] private InputActionReference defaultBacktrack;
        [FormerlySerializedAs("defaultHighligher")] 
        [SerializeField] private SelectionHighlighter defaultHighlighter;

        [SerializeField] private bool active; 
        

        private void Start()
        {
            menuBuilder = new SelectionMenuBuilder(
                defaultConfirm, 
                defaultNavigate, 
                defaultBacktrack,
                defaultHighlighter); 
        }

        public SelectionMenuBuilder CreateMenu() => menuBuilder;
        
        /*
         TODO: Create some sort of "SelectionSession" class that can be used to track the selection state, and can be built
            using the builder pattern. e.g. StartSelection().WithNavigateInputAction().WithConfirmAction()... you know the one.
        */
        public void StartSelection(SelectionMenu menu)
        {
            if (active && !ignorePush)
            {
                ActiveMenu.Deactivate();
                previousMenuStack.Push(ActiveMenu);
            }

            ignorePush = false; 
            active = true; 
            
            ActiveMenu = menu;
            ActiveMenu.Activate();
            
            CurrentItem.Value = ActiveMenu.CurrentItem; 
        }

        private bool ignorePush; 
        void Backtrack()
        {
            if (previousMenuStack.Count == 0) return;
            ActiveMenu.Backtrack();
            
            ignorePush = true; 
            StartSelection(previousMenuStack.Pop());
        }
        
        internal void EndSelection()
        {
            previousMenuStack.Clear();
            ActiveMenu = null; 
            active = false;
        }

        private void Update()
        {
            if (!active) return;

            NavigateSelectables();
            HandleBacktrackInput();
            ConfirmSelectable();
        }

        void ConfirmSelectable()
        {
            if (ActiveMenu.ConfirmAction == null) return;
            if (ActiveMenu.ConfirmAction.WasPressedThisFrame())
            {
                SelectionMenu menu = ActiveMenu;
                if (menu.IsLastMenu) EndSelection();
                menu.Select();
            }
        }

        void HandleBacktrackInput()
        {
            if (ActiveMenu.BacktrackAction == null) return;
            if (ActiveMenu.BacktrackAction.WasPressedThisFrame())
                Backtrack();
        }
        
        void NavigateSelectables()
        {
            if (ActiveMenu.NavigateAction == null) return;
            if (!ActiveMenu.NavigateAction.WasPressedThisFrame()) return;

            
            Vector2 value = ActiveMenu.NavigateAction.ReadValue<Vector2>();
        
            if (value.x < 0)
                ShiftSelection(ActiveMenu.InvertNavigation ? 1 : -1);
            if (value.x > 0)
                ShiftSelection(ActiveMenu.InvertNavigation ? -1 : 1);
        }

        void ShiftSelection(int amt)
        {
            ActiveMenu.ShiftSelection(amt);
            CurrentItem.Value = ActiveMenu.CurrentItem;
        }
    }
}
