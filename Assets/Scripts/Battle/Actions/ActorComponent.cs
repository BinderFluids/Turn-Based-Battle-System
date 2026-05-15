using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Enums;
using Battle.Events;
using Battle.Interfaces;
using Battle.Requests;
using Battle.TargetSelection;
using Battle.Window;
using EventBus;
using SerializedInterface;
using UnityEngine;
using RequestHub;
using UnityEditor.PackageManager.Requests;

namespace Battle.Actions
{
    public class ActorComponent : BattleEntityComponent
    {
        [SerializeField] private InterfaceReference<IBattleActionSelectionStrategy> selectActionStrategyRef; 
        private IBattleActionSelectionStrategy actionSelectionStrategy => selectActionStrategyRef.Value;
    
        [SerializeField] private InterfaceReference<IBattleEntitySelectionStrategy> targetSelectionStrategyRef; 
        private IBattleEntitySelectionStrategy targetSelectionStrategy => targetSelectionStrategyRef.Value;
    
        [SerializeField] private List<InterfaceReference<IBattleAction>> actionsRef;
        public IReadOnlyList<IBattleAction> Actions => actionsRef.Select(a => a.Value).ToList();
        private IBattleAction chosenAction;

        private EventBinding<ActorChooseAction> chooseActionBinding; 
        private EventBinding<CancelChooseAction> cancelChooseActionBinding;
        private EventBinding<CancelSelectEntity> cancelSelectEntityBinding;
        
        public event Action onActionStarted = delegate {}; 
        public event Action onActionEnded = delegate { };

        protected override ComponentType componentType => ComponentType.Actor; 

        protected override void Awake()
        {
            base.Awake();

            chooseActionBinding = new EventBinding<ActorChooseAction>(HandleChooseActionEvent);
            EventBus<ActorChooseAction>.Register(chooseActionBinding);

            cancelChooseActionBinding = new EventBinding<CancelChooseAction>(e => CancelChooseAction(e.Entity));
            EventBus<CancelChooseAction>.Register(cancelChooseActionBinding);
            
            cancelSelectEntityBinding = new EventBinding<CancelSelectEntity>(e => CancelSelectEntity(e.Entity));
            EventBus<CancelSelectEntity>.Register(cancelSelectEntityBinding);
        }
        
        void HandleChooseActionEvent(ActorChooseAction e)
        {
            if (e.Entity == Entity)  
                ChooseAction();
        }
        public void ChooseAction()
        {
            actionSelectionStrategy.onActionSelected += OnActionSelected;
            actionSelectionStrategy.GetAction(Actions);
        }

        void CancelChooseAction(BattleEntity entity)
        {
            if (entity == Entity)
                CancelChooseAction();
        }
        public void CancelChooseAction() => actionSelectionStrategy.onActionSelected -= OnActionSelected;

        void OnActionSelected(IBattleAction action)
        {
            chosenAction = action;
            actionSelectionStrategy.onActionSelected -= OnActionSelected;
        
            targetSelectionStrategy.onEntitySelected += OnTargetSelected;
            targetSelectionStrategy.GetEntity(Entity, action, BattleEntity.AllEntities); 
        }

        void CancelSelectEntity(BattleEntity entity)
        {
            if (entity == Entity)
                CancelSelectEntity();
        }
        public void CancelSelectEntity() => targetSelectionStrategy.onEntitySelected -= OnTargetSelected;
    
        void OnTargetSelected(BattleEntity target)
        {
            targetSelectionStrategy.onEntitySelected -= OnTargetSelected; 
            StartAction(chosenAction, target);
        }
    
        public void StartAction(IBattleAction action, BattleEntity target)
        {
            if (action is null)
            {
                Debug.LogError($"Tried to start action on {gameObject.name} with null action");
                return;
            }

            if (target is null)
            {
                Debug.LogError($"Tried to start action on {gameObject.name} with null target");
                return;
            }
        
            chosenAction = action; 
            chosenAction.onActionEnded += OnActionEnded;
        
            chosenAction.StartAction(Entity, target);
        }
        void OnActionEnded()
        {
            chosenAction.onActionEnded -= OnActionEnded;
            onActionEnded?.Invoke();
        }

        private void OnDestroy()
        {
            DeregisterEventBindings();
        }
        
        void DeregisterEventBindings()
        {
            EventBus<ActorChooseAction>.Deregister(chooseActionBinding);
            EventBus<CancelChooseAction>.Deregister(cancelChooseActionBinding);
            EventBus<CancelSelectEntity>.Deregister(cancelSelectEntityBinding);
        }
    }
}