using System;
using System.Collections.Generic;
using Battle.Enums;
using Battle.Events;
using Core;
using Cysharp.Threading.Tasks;
using UnityUtils;
using EventBus;
using UnityEngine;

namespace Battle.TurnPhase
{
    /// <summary>
    /// Manages the state of the battle phases.
    /// </summary>
    public class BattlePhaseManager : Singleton<BattlePhaseManager>
    {
        [field: SerializeField] public BattlePhases CurrentPhase { get; private set; }
        
        private Dictionary<BattlePhases, Transition> phaseTransitions = new();
        private UniTask pendingTransition; 

        public event Action<BattlePhases> onPhaseChangeBegin = delegate { };
        public event Action<BattlePhases> onPhaseChangeEnd = delegate { };
        
        private EventBinding<TransitionToPhase> transitionToPhaseBinding;
        
        void Start()
        {
            RegisterEventBindings(); 
            InitializeTransitions();
            CurrentPhase = BattlePhases.Intro; 
        }

        void RegisterEventBindings()
        {
            transitionToPhaseBinding = new EventBinding<TransitionToPhase>(e => TransitionToPhase(e.Phase));
            EventBus<TransitionToPhase>.Register(transitionToPhaseBinding);
        }

        void DeregisterEventBindings()
        {
            EventBus<TransitionToPhase>.Deregister(transitionToPhaseBinding);
        }
        
        public void AddPendingTask(BattlePhases phase, UniTask task) => phaseTransitions[phase].AddPendingTask(task);
        public void QueueCommand(BattlePhases phase, ICommand command) => phaseTransitions[phase].QueueCommand(command);
        
        void InitializeTransitions()
        {
            foreach (BattlePhases phase in Enum.GetValues(typeof(BattlePhases)))
                phaseTransitions.Add(phase, new Transition());
        }

        public async UniTask AwaitCurrentTransitionAsync() => await pendingTransition;
        
        public void TransitionToPhase(BattlePhases phase) => TransitionToPhaseAsync(phase).Forget();
        public async UniTask TransitionToPhaseAsync(BattlePhases phase)
        {
            var timeStartedTransition = Time.frameCount;  
            
            if (phase == CurrentPhase)
            {
                Debug.LogWarning($"Tried to transition to phase {phase} when already in that phase, so the transition is cancelled.");
                return;
            }
            
            onPhaseChangeBegin?.Invoke(phase);
            
            EventBus<PhaseEnded>.Raise(new PhaseEnded(CurrentPhase));
            
            Transition transition = phaseTransitions[phase];
            UniTask transitionAsync = transition.TransitionAsync();
            pendingTransition = transitionAsync;
            
            transition.PrintStatus(gameObject.name);
            await transitionAsync;

            CurrentPhase = phase;
            EventBus<PhaseStarted>.Raise(new PhaseStarted(CurrentPhase));
            onPhaseChangeEnd?.Invoke(phase);
            
            HandleNewPhase();
            
            Debug.Log($"Ended transition to {phase}; {timeStartedTransition}");
        }

        void HandleNewPhase()
        {
            switch (CurrentPhase)
            {
                case BattlePhases.SelectingAction:
                    EventBus<ActorChooseAction>.Raise(
                        new ActorChooseAction { Entity = BattleManager.Instance.ActiveEntity });
                    break;
                default:
                    break;
            }
        }

        private void OnDestroy()
        {
            DeregisterEventBindings();
        }
    }
}