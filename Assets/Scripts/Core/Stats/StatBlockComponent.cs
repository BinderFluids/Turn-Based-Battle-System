using System;
using UnityEngine;

namespace Core.Stats
{
    public class StatBlockComponent : MonoBehaviour
    {
        [SerializeField] private StatBlockTemplate template; 
        [SerializeField] private StatBlock statBlock;
        public StatBlock StatBlock => statBlock;

        private void Awake()
        {
            statBlock = new StatBlock(new StatsMediator(), template); 
        }
    }
}