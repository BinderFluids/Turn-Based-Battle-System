using UnityEngine;

namespace Core.Stats
{
    [CreateAssetMenu(menuName = "Stats/Template", fileName = "StatBlockTemplate", order = 0)]
    public class StatBlockTemplate : ScriptableObject
    {
        [Min(1)] public int attack;
        public int defense;
        [Min(1)] public int speed; 
        [Min(5)] public int health; 
    }
}