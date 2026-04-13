using UnityEngine;

namespace Battle.BattleEntity
{
    public enum FormationSlots
    {
        Top,
        Bottom,
        Front,
        Back,
        Middle
    }
    
    public class FormationSlotComponent : BattleEntityComponent
    {
        public Vector2 topPosition; 
    }
}