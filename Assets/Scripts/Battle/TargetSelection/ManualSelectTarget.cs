using System;
using System.Collections.Generic;
using Battle.Events;
using Battle.Interfaces;
using Battle.Requests;
using Core.Enums;
using EventBus;
using UnityEngine;
using SelectableSystem;
using RequestHub;

namespace Battle.TargetSelection
{
    [CreateAssetMenu(menuName = "Battle/Entity Selection/Manual", fileName = "ManualSelectTarget", order = 0)]
    public class ManualSelectTarget : ScriptableBattleEntitySelectionStrategy
    {
        public override event Action<BattleEntity> onEntitySelected;
    
        public override void BeginTargetSelection(BattleEntity actor, IBattleAction action, IEnumerable<BattleEntity> ctx)
        {
            //Create list of selectable entities
            List<ISelectable> entitiesAsSelectables = new();
            foreach (BattleEntity entity in action.GetValidTargets(actor, ctx))
                if (entity.TryGetComponent(out SelectableComponent selectable))
                    entitiesAsSelectables.Add(selectable);
            
            PlayerId actorPlayerId = PlayerId.PlayerOne; 
            if (!RequestHub<RequestablePlayerId>.TryRequest(actor, out var request))
                Debug.Log("Actor does not have a PlayerId. Defaulting to PlayerOne input");
            else
                actorPlayerId = request.PlayerId;
            
            var menu = SelectionManager.Instance.CreateMenu()
                .WithConfirmAction(BattleUtils.PlayerInputData.GetInputActionByPlayerID(actorPlayerId))
                .WithIsLastMenu()
                .Build(entitiesAsSelectables); 
            menu.onItemSelected += OnMenuItemSelected;
            menu.onMenuBacktracked += () => EventBus<CancelSelectEntity>.Raise(new CancelSelectEntity() { Entity = actor });
            
            SelectionManager.Instance.StartSelection(menu);
        }
    
        void OnMenuItemSelected(ISelectable item)
        {
            if (item is SelectableComponent selectable)
                onEntitySelected?.Invoke(selectable.Entity);
        }
    }
}