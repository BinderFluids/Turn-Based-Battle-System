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
        [SerializeField] private ActionCommandTierGradient gradient; 
        [SerializeField] private InterfaceReference<IBattleActionSelectionStrategy> selectActionStrategyRef; 
        private IBattleActionSelectionStrategy actionSelectionStrategy => selectActionStrategyRef.Value;
    
        [SerializeField] private InterfaceReference<IBattleEntitySelectionStrategy> targetSelectionStrategyRef; 
        private IBattleEntitySelectionStrategy targetSelectionStrategy => targetSelectionStrategyRef.Value;
    
        [SerializeField] private List<InterfaceReference<IBattleAction>> actionsRef;
        public IReadOnlyList<IBattleAction> Actions => actionsRef.Select(a => a.Value).ToList();
        private IBattleAction chosenAction;

        private EventBinding<ActorChooseAction> chooseActionBinding; 
        
        public event Action onActionStarted = delegate {}; 
        public event Action onActionEnded = delegate { };

        protected override ComponentType componentType => ComponentType.Actor; 

        protected override void Awake()
        {
            base.Awake();

            chooseActionBinding = new EventBinding<ActorChooseAction>(HandleChooseActionEvent);
            EventBus<ActorChooseAction>.Register(chooseActionBinding); 
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
    
        void OnActionSelected(IBattleAction action)
        {
            chosenAction = action;
            actionSelectionStrategy.onActionSelected -= OnActionSelected;
        
            targetSelectionStrategy.onEntitySelected += OnTargetSelected;
            targetSelectionStrategy.GetEntity(Entity, action, BattleEntity.AllEntities); 
        }
    
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
            EventBus<ActorChooseAction>.Deregister(chooseActionBinding);
        }
    }
}