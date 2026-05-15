using Battle.Events;
using Battle.Interfaces;
using Cysharp.Threading.Tasks;
using EventBus;

namespace Battle.Graph.Runtime.Nodes
{
    public class StartDodgePhaseRuntimeNode : RuntimeNode
    {
        public IDodgeBehaviour DodgeBehaviour;
        public override async UniTask Execute(BattleActionDirector ctx, BattleEntity actor, BattleEntity target)
        {
            //TODO (undo comment) EventBus<StartDodgePhase>.Raise(new StartDodgePhase(DodgeBehaviour)); 
            await UniTask.CompletedTask;
        }
    }
}