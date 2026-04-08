using System;
using System.Threading.Tasks;

public static class EventAwaiter
{
    public static Task<T> WaitForEvent<T>(
        Action<Action<T>> addHandler,
        Action<Action<T>> removeHandler)
    {
        var tcs = new TaskCompletionSource<T>();
        Action<T> handler = null;
        handler = value =>
        {
            removeHandler(handler);
            tcs.SetResult(value);
        };
        addHandler(handler);
        return tcs.Task;
    }

    public static async Task<T> WaitForEvent<T>(Action<T> @action)
    {
        return await WaitForEvent<T>(
            addHandler: h => @action += h,
            removeHandler: h => @action -= h
        );
    }
}