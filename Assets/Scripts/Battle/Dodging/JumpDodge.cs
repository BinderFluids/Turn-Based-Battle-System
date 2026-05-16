using Core.Enums;
using Battle.Interfaces;
using Battle.Requests;
using Cysharp.Threading.Tasks;
using RequestHub;
using EventBus;
using Battle.Events; 

namespace Battle.Dodging
{
    public class JumpDodge : IDodgeBehaviour
    {
        private UniTask jumpTask; 
        
        public void UpdateDodge(BattleEntity entity)
        {
            if (!RequestHub<RequestablePlayerId>.TryRequest(entity, out var request)) return;

            if (BattleUtils.PlayerInputData.GetInputActionByPlayerID(request.PlayerId).WasPressedThisFrame())
            {
                if (jumpTask.Status.IsCompleted())
                {
                    jumpTask = Jump(entity);
                    EventBus<AddPreTurnTask>.Raise(new AddPreTurnTask()
                    {
                        Entity =  entity,
                        Task = jumpTask
                    });
                } 
            }
        }

        async UniTask Jump(BattleEntity entity)
        {
            await entity.Jump(entity.Transform.position, 2f, 1, MotionType.Duration); 
        }
    }
}