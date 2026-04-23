using Battle;
using UnityEngine;

namespace Battle.Components
{
    [RequireComponent(typeof(BattleEntity))]
    public abstract class BattleEntityComponent : MonoBehaviour
    {
        [SerializeField] private BattleEntity entity;
        public BattleEntity Entity => entity;


        protected virtual void Awake() { }

        protected virtual void Start() => entity ??= GetComponent<BattleEntity>();
    }
}