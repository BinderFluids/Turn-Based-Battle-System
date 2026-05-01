using Battle.Enums;
using UnityEngine;

namespace Battle.Components
{
    public class FormationSlotComponent : BattleEntityComponent
    {
        public Vector2 topPosition;
        protected override ComponentType componentType => ComponentType.FormationSlots; 
    }
}