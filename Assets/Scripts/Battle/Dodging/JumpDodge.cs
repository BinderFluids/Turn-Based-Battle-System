using Battle.Enums;
using Battle.Interfaces;
using Battle.Requests;
using Cysharp.Threading.Tasks;
using RequestHub;

namespace Battle.Dodging
{
    public class JumpDodge : IDodgeBehaviour
    {
        private UniTask jumpTask; 
        
        public void UpdateDodge(BattleEntity entity)
        {
            if (!RequestHub<RequestPlayerId>.TryRequest(entity, out var request)) return;

            if (BattleUtils.PlayerInputData.GetInputActionByPlayerID(request.PlayerId).WasPressedThisFrame())
            {
                if (jumpTask.Status.IsCompleted())
                    jumpTask = Jump(entity); 
            }
        }

        async UniTask Jump(BattleEntity entity)
        {
            await entity.Jump(entity.Transform.position, 2f, 1, EntityMotionType.Duration); 
        }
    }
}