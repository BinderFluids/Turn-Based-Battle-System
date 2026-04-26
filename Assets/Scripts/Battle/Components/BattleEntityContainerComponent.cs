using System.Collections.Generic;
using Battle;
using UnityEngine;

namespace Battle.Components
{
    public class BattleEntityContainerComponent : BattleEntityComponent
    {
        [SerializeField] private List<BattleEntity> entities;
        public IReadOnlyList<BattleEntity> Entities => entities;
    
        public void AddEntity(BattleEntity entity) => entities.Add(entity);
        public void RemoveEntity(BattleEntity entity) => entities.Remove(entity);
    }
}