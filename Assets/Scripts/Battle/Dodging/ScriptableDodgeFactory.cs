using System;
using Battle.Interfaces;
using UnityEngine;

namespace Battle.Dodging
{
    public abstract class ScriptableDodgeFactory<T> : ScriptableObject, IDodgeFactory where T : IDodgeBehaviour
    {
        public IDodgeBehaviour GetDodgeBehaviour() => CreateDodgeBehaviour();
        protected abstract T CreateDodgeBehaviour();
    }
}