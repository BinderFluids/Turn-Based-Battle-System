using System;
using Battle.Interfaces;
using UnityEngine;

namespace Battle.Dodging.Factories
{
    public abstract class ScriptableDodgeFactory : ScriptableObject, IDodgeFactory 
    {
        public abstract IDodgeBehaviour GetDodgeBehaviour();
    }
}