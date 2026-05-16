using Core;
using Cysharp.Threading.Tasks;
using EventBus;

namespace Battle.Events
{
    public struct AddPreTurnTask : IEvent
    {
        public BattleEntity Entity;
        public UniTask Task;
        
        public AddPreTurnTask(BattleEntity entity, UniTask task)
        {
            Entity = entity;
            Task = task;
        }
    }
    public struct QueuePreTurnCommand : IEvent
    {
        public BattleEntity Entity;
        public ICommand Command;
        
        public QueuePreTurnCommand(BattleEntity entity, ICommand command)
        {
            Entity = entity;
            Command = command;
        }
    }
    
    public struct AddPostTurnTask : IEvent
    {
        public BattleEntity Entity;
        public UniTask Task;
        
        public AddPostTurnTask(BattleEntity entity, UniTask task)
        {
            Entity = entity;
            Task = task;
        }   
    }
    public struct QueuePostTurnCommand : IEvent
    {
        public BattleEntity Entity;
        public ICommand Command;
        
        public QueuePostTurnCommand(BattleEntity entity, ICommand command)
        {
            Entity = entity;
            Command = command;
        }
    }
}