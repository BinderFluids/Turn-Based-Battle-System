using System;
using System.Collections.Generic;

namespace Core.Stats
{
    public class StatsMediator
    {
        private readonly LinkedList<StatModifier> modifiers = new();

        public event EventHandler<Query> Queries;
        public void PerformQuery(object sender, Query query) => Queries?.Invoke(sender, query);

        public void AddModifier(StatModifier modifier)
        {
            modifiers.AddLast(modifier);
            Queries += modifier.Handle;

            modifier.OnDispose += _ =>
            {
                modifiers.Remove(modifier);
                Queries -= modifier.Handle;
            };
        }

        public void Update(float deltaTime)
        {
            var node = modifiers.First;
            while (node != null)
            {
                var nextNode = node.Next; 
                
                if (node.Value.MarkedForRemoval)
                {
                    node.Value.Dispose();
                }

                node = nextNode; 
            }
        }
    }

    public class Query
    {
        public readonly StatType StatType;
        public int Value;

        public Query(StatType statType, int value)
        {
            StatType = statType;
            Value = value;
        }
    }
}