using RequestHub;

namespace Battle.Requests
{
    public struct RequestableAttackValue : IRequestable
    {
        public int AttackValue; 
    }

    public struct RequestableDefenseValue : IRequestable
    {
        public int DefenseValue;
    }

    public struct RequestableSpeedValue : IRequestable
    {
        public int SpeedValue; 
    }
}