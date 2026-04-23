using System;
using System.Collections.Generic;
using Battle.Interfaces;
using UnityEngine;

namespace Battle.TargetSelection
{
    [CreateAssetMenu(menuName = "Battle/Entity Selection/Manual", fileName = "ManualSelectTarget", order = 0)]
    public class ManualSelectTarget : ScriptableBattleEntitySelectionStrategy
    {
        public override event Action<BattleEntity> onEntitySelected;
    
        private EventBinding<SelectableChosenEvent> chosenEventBinding;

    
        public override void GetEntity(BattleEntity actor, IBattleAction action, IEnumerable<BattleEntity> ctx)
        {
            chosenEventBinding ??= new EventBinding<SelectableChosenEvent>(OnSelectableChosenEventRaised);
            EventBus<SelectableChosenEvent>.Register(chosenEventBinding);
        
            //Create list of selectable entities
            List<ISelectable> entitiesAsSelectables = new();
            foreach (BattleEntity entity in action.GetValidTargets(actor, ctx))
            {
                if (entity.TryGetComponent(out SelectableComponent selectable))
                    entitiesAsSelectables.Add(selectable);
            }
            SelectionManager.Instance.StartSelection(entitiesAsSelectables);
        }
    
        void OnSelectableChosenEventRaised(SelectableChosenEvent e)
        {
            SelectableComponent selectedEntity = e.SelectedItem as SelectableComponent;
            onEntitySelected?.Invoke(selectedEntity.Entity);
        
            EventBus<SelectableChosenEvent>.Deregister(chosenEventBinding);
        }
    }
}