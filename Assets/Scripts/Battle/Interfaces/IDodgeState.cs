namespace Battle.Interfaces
{
    public interface IDodgeState
    {
        void Enter();
        void Exit();
        void Update(); 
        void FixedUpdate();
    }
}