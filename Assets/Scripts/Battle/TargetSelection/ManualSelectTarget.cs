using System;
using System.Collections.Generic;
using Battle.Interfaces;
using Battle.Requests;
using Core.Enums;
using EventBus;
using UnityEngine;
using SelectableSystem;
using SelectableSystem.Events;
using RequestHub;

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

            PlayerId actorPlayerId = PlayerId.PlayerOne; 
            if (!RequestHub<RequestPlayerId>.TryRequest(actor, out var request))
                Debug.Log("Actor does not have a PlayerId. Defaulting to PlayerOne input");
            else
                actorPlayerId = request.PlayerId;
            
            SelectionManager.Instance.SetConfirmAction(BattleUtils.PlayerInputData.GetInputActionByPlayerID(actorPlayerId));
        }
    
        void OnSelectableChosenEventRaised(SelectableChosenEvent e)
        {
            SelectableComponent selectedEntity = e.SelectedItem as SelectableComponent;
            onEntitySelected?.Invoke(selectedEntity.Entity);
        
            EventBus<SelectableChosenEvent>.Deregister(chosenEventBinding);
        }
    }
}