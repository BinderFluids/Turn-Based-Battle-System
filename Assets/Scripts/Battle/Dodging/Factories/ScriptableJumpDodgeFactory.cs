using Battle.Interfaces;
using UnityEngine;

namespace Battle.Dodging.Factories
{
    [CreateAssetMenu(menuName = "Battle/Dodge/Jump Factory", fileName = "ScriptableJumpDodgeFactory", order = 0)]
    public class ScriptableJumpDodgeFactory : ScriptableDodgeFactory
    {
        public override IDodgeBehaviour GetDodgeBehaviour() => new JumpDodge();
    }
}