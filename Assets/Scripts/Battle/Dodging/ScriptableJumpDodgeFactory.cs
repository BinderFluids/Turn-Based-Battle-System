using UnityEngine;

namespace Battle.Dodging
{
    [CreateAssetMenu(menuName = "Battle/Dodge/Jump Factory", fileName = "ScriptableJumpDodgeFactory", order = 0)]
    public class ScriptableJumpDodgeFactory : ScriptableDodgeFactory<JumpDodge>
    {
        protected override JumpDodge CreateDodgeBehaviour() 
            => new();
    }
}