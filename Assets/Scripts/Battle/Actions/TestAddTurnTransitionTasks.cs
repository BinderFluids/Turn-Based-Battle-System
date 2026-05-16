using Battle.Events;
using Battle.Phase;
using Battle.TargetSelection;
using Core;
using Core.Enums;
using Cysharp.Threading.Tasks;
using EventBus;
using UnityEngine;

namespace Battle.Actions
{
    [CreateAssetMenu(menuName = "Battle/Actions/Add Post Turn Transition", fileName = "TestAddTurnTransitionTasks", order = 0)]
    public class TestAddTurnTransitionTasks : ScriptableBattleAction
    {
        public override IBattleEntitySelectionStrategy ForcedTargetSelectionStrategy
        {
            get
            {
                targetSelectionStrategy ??= new NoSelection();
                return targetSelectionStrategy;
            }
        } 
        private IBattleEntitySelectionStrategy targetSelectionStrategy;
        
        public override void StartAction(BattleEntity actor, BattleEntity target)
        {
            if (!actor.TryGetComponent(out TurnComponent turnComponent)) return; 
            
            Command jumpCommand = new Command(() => Jump(actor)); 
            turnComponent.QueuePostTurnCommand(jumpCommand);
            
            EndAction(actor);
        }

        async UniTask Jump(BattleEntity entity)
        {
            await entity.Jump(entity.transform.position, 1f, .5f, MotionType.Duration);
        }

        async UniTask WaitForSeconds()
        {
            await UniTask.WaitForSeconds(3); 
        }
    }
}