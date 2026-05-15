using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace SelectableSystem
{
    public class SelectionMenuBuilder
    {
        private InputAction defaultConfirmAction;
        private InputAction defaultNavigateAction;
        private InputAction defaultBacktrackAction;
        private SelectionHighlighter defaultHighlighter;
        
        private InputAction confirmAction;
        private InputAction navigateAction;
        private InputAction backtrackAction;
        private SelectionHighlighter highlighter;
        private bool invertNavigation = false;
        private bool isLastMenu = false;
        
        public SelectionMenuBuilder WithNavigationAction(InputAction action)
        {
            navigateAction = action;
            return this; 
        }
        
        public SelectionMenuBuilder WithInvertedNavigation(bool doInvert = true)
        {
            invertNavigation = doInvert;
            return this; 
        }

        public SelectionMenuBuilder WithConfirmAction(InputAction action)
        {
            confirmAction = action;
            return this; 
        }
        
        public SelectionMenuBuilder WithBacktrackAction(InputAction action)
        {
            backtrackAction = action;
            return this; 
        }
        
        public SelectionMenuBuilder WithHighlighter(SelectionHighlighter highlighter)
        {
            this.highlighter = highlighter;
            return this;
        }

        /// <summary>
        /// When an item is selected, the selection session will optionally end.
        /// </summary>
        public SelectionMenuBuilder WithIsLastMenu(bool setAsLastMenu = true)
        {
            isLastMenu = setAsLastMenu;
            return this; 
        }

        public SelectionMenu Build(List<ISelectable> items)
        {
            SelectionMenu newMenu = new SelectionMenu(
                items, 
                confirmAction ?? defaultConfirmAction, 
                navigateAction ?? defaultNavigateAction, 
                backtrackAction ?? defaultBacktrackAction,
                highlighter ?? defaultHighlighter,
                invertNavigation,
                isLastMenu
            );
            
            ResetValues();
            
            return newMenu;
        }

        void ResetValues()
        {
            confirmAction = null; 
            navigateAction = null; 
            highlighter = null;
            invertNavigation = false;
            isLastMenu = false; 
        }

        internal SelectionMenuBuilder(InputAction defaultConfirmAction, InputAction defaultNavigateAction,
            InputAction defaultBacktrackAction, SelectionHighlighter defaultHighlighter)
        {
            this.defaultConfirmAction = defaultConfirmAction;
            this.defaultNavigateAction = defaultNavigateAction;
            this.defaultBacktrackAction = defaultBacktrackAction; 
            this.defaultHighlighter = defaultHighlighter;
        }
    }
}