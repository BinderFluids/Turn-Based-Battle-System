using RequestHub;

namespace Battle.Requests
{
    public struct RequestAttackValue : IRequest
    {
        public int AttackValue; 
    }

    public struct RequestDefenseValue : IRequest
    {
        public int DefenseValue;
    }

    public struct RequestSpeedValue : IRequest
    {
        public int SpeedValue; 
    }
}