using System;
using System.Collections.Generic;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.TurnPhase
{
    [Serializable]
    public class Transition
    {
        private List<UniTask> pendingTasks = new(); 
        private Queue<ICommand> queuedCommands = new();
        
        // public IReadOnlyList<UniTask> PendingTasks => pendingTasks;
        // public IReadOnlyCollection<ICommand> QueuedCommands => queuedCommands;

        public UniTask CurrentTransition { get; private set; }
        
        public event Action onTransitionEnd; 

        public void AddPendingTask(UniTask task) => pendingTasks.Add(task);
        public void QueueCommand(ICommand command) => queuedCommands.Enqueue(command);

        public void PrintStatus(object source)
        {
            if (pendingTasks.Count > 0)
                Debug.Log($"{source}: There are pending tasks to be completed before the turn starts, waiting...");
            if (queuedCommands.Count > 0)
                Debug.Log($"{source}: There are still commands that need to be executed");
        }
        
        public async UniTask TransitionAsync()
        {
            CurrentTransition = AwaitPendingTasksAndCommandsAsync();
            await CurrentTransition;
            
            Clear();
            
            onTransitionEnd?.Invoke();
        }

        async UniTask AwaitPendingTasksAndCommandsAsync()
        {
            await AwaitPendingTasksAsync();
            await AwaitQueuedCommandsAsync();
        }
        async UniTask AwaitQueuedCommandsAsync()
        {
            if(queuedCommands.Count > 0)
                while (queuedCommands.Count > 0)
                    await queuedCommands.Dequeue().Execute();
        }
        async UniTask AwaitPendingTasksAsync()
        {
            if (pendingTasks.Count > 0)
                await UniTask.WhenAll(pendingTasks);
        }

        void Clear()
        {
            pendingTasks.Clear(); 
            queuedCommands.Clear();
        }
    }
}