using System;
using System.Collections.Generic;
using Battle.Interfaces;
using SelectableSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Battle.SelectionStrategy
{
    public class BattleActionWheel : MonoBehaviour, IBattleActionSelectionStrategy
    {
        [SerializeField] List<BattleActionWheelItem> items;

        public event Action<IBattleAction> onActionSelected;
        private event Action<IBattleAction> onActionSelectedCached; 
        
        private BattleActionWheelItem selectedItem;

        [SerializeField] private InputActionReference confirm;

        
        public void GetAction(IEnumerable<IBattleAction> context)
        {
            onActionSelectedCached = onActionSelected;
            
            var menu = SelectionManager.Instance
                .CreateMenu()
                .WithConfirmAction(confirm.ToInputAction())
                .Build(items.ConvertAll(i => i as ISelectable));
            
            menu.onMenuActivated += () => ActivateItems(); 
            menu.onMenuDeactivated += () => ActivateItems(false);
            menu.onItemSelected += OnMenuItemSelected; 
            
            SelectionManager.Instance.StartSelection(menu); 
        }
    
        void OnMenuItemSelected(ISelectable item)
        {
            if (item is BattleActionWheelItem wheel)
            {
                onActionSelectedCached?.Invoke(wheel.Action); 
                ActivateItems(false); 
            }
        }
    
        void ActivateItems(bool active = true)
        {
            foreach (var item in items)
                item.gameObject.SetActive(active); 
        }
    }
}