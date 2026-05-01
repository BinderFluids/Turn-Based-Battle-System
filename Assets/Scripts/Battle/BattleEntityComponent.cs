using Battle;
using Battle.Enums;
using UnityEngine;

namespace Battle
{
    [RequireComponent(typeof(BattleEntity))]
    public abstract class BattleEntityComponent : MonoBehaviour
    {
        [SerializeField] private BattleEntity entity;
        public BattleEntity Entity => entity;
        
        protected abstract ComponentType componentType { get; }
        public ComponentType ComponentType => componentType;


        protected virtual void Awake() { }

        protected virtual void Start() => entity ??= GetComponent<BattleEntity>();
    }
}