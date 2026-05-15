using Battle;
using Battle.Requests;
using Battle.Enums;
using UnityEngine;
using RequestHub; 

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Battle.Components
{
    public class FormationOffsetsComponent : BattleEntityComponent
    {

        protected override ComponentType componentType => ComponentType.FormationSlots;

    }
    
}