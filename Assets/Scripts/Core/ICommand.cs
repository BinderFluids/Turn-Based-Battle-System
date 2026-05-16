using System;
using Cysharp.Threading.Tasks;

namespace Core
{
    public interface ICommand
    {
        event Action onCommandStarted;
        event Action onCommandComplete;

        UniTask Execute(); 
    }

    public class Command : ICommand
    {
        public event Action onCommandStarted;
        public event Action onCommandComplete;
        private Func<UniTask> execute;

        public Command(Func<UniTask> execute) => this.execute = execute;

        public async UniTask Execute()
        {
            onCommandStarted?.Invoke();
            await execute();
            onCommandComplete?.Invoke();
        }
    }

    public class EventCommand : ICommand
    {
        public event Action onCommandStarted;
        public event Action onCommandComplete;
        private bool completed = false;
        private float timeOut = 600f; 
        
        public async UniTask Execute()
        {
            onCommandStarted?.Invoke();

            UniTask waitForCompletion = UniTask.WaitWhile(() => !completed);
            // UniTask waitForTimeout = UniTask.WaitForSeconds(timeOut);
            //
            // await UniTask.WhenAny(waitForCompletion, waitForTimeout);
            await waitForCompletion; 
            
            onCommandComplete?.Invoke();
        }
        
        public void Complete() => completed = true;
    }
}