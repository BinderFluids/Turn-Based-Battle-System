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
            if (template == null)
            {
                Debug.LogError($"{name} is missing a StatBlockTemplate.");
                return;
            }
            
            statBlock = new StatBlock(new StatsMediator(), template); 
        }
    }
}