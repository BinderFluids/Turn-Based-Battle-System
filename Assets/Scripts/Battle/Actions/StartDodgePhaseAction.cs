using Battle.Interfaces;
using Cysharp.Threading.Tasks;
using EventBus;
using SerializedInterface;
using UnityEngine;
using Battle.Events; 

namespace Battle.Actions
{
    
    [CreateAssetMenu(menuName = "Battle/Actions/Dodge Phase", fileName = "StartDodgePhaseAction", order = 0)]
    public class StartDodgePhaseAction : ScriptableBattleAction
    {
        [SerializeField] private InterfaceReference<IDodgeFactory> dodgeFactoryRef;
        
        public override void StartAction(BattleEntity actor, BattleEntity target)
        {
            EventBus<StartDodgePhase>.Raise(new StartDodgePhase(dodgeFactoryRef.Value));
            Wait(actor).Forget();
        }

        async UniTaskVoid Wait(BattleEntity actor)
        {
            await UniTask.WaitForSeconds(5f);
            EndAction(actor);
        }
    }
}