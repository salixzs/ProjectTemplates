namespace WebApiTemplate.Crosscut.Extensions;

public static class TaskExtensions
{
    /// <summary>
    /// Start a task but you don't want to wait for it to finish.<br/>
    /// This is useful when you want to start a task but you don't care about the result (non-critical tasks).<br/>
    /// For example when you want to start a task that sends an email. You don't want to wait for the email to be sent before you can continue with your code.
    /// <code>
    /// SendEmailAsync().FireAndForget(errorHandler => Console.WriteLine(errorHandler.Message));
    /// </code>
    /// </summary>
    /// <param name="task">Task to be executed.</param>
    /// <param name="errorHandler">Handler for case when task throws exception.</param>
    public static void FireAndForget(
        this Task task,
        Action<Exception?>? errorHandler = null) =>
            task.ContinueWith(
                tsk =>
                {
                    if (!tsk.IsFaulted || errorHandler == null)
                    {
                        return;
                    }

                    errorHandler(tsk.Exception);
                },
                TaskContinuationOptions.OnlyOnFaulted);

    /// <summary>
    /// This method will retry the task until it succeeds or the maximum number of retries is reached.<br/>
    /// You can pass a delay between retries. This delay will be used between each retry.
    /// <code>
    /// var result = await (() => GetResultAsync()).Retry(3, TimeSpan.FromSeconds(1));
    /// </code>
    /// </summary>
    /// <typeparam name="TResult">Result type.</typeparam>
    /// <param name="taskFactory">Original task process.</param>
    /// <param name="maxRetries">Maximum retries before fail.</param>
    /// <param name="delay">Delay between retries.</param>
    public static async Task<TResult?> Retry<TResult>(this Func<Task<TResult>> taskFactory, int maxRetries, TimeSpan delay)
    {
        for (var i = 0; i < maxRetries; i++)
        {
            try
            {
                return await taskFactory().ConfigureAwait(false);
            }
            catch when (i != maxRetries - 1)
            {
                await Task.Delay(delay);
            }
        }

        return default; // Should not be reached
    }

    /// <summary>
    /// Executes a callback function when a Task encounters an exception.
    /// <code>
    /// await GetResultAsync().OnFailure(ex => Console.WriteLine(ex.Message));
    /// </code>
    /// </summary>
    /// <param name="task">Executable task.</param>
    /// <param name="onFailure">Action to be executed when error occurs in task execution.</param>
    public static async Task OnFailure(this Task task, Action<Exception> onFailure)
    {
        try
        {
            await task;
        }
        catch (Exception ex)
        {
            onFailure(ex);
        }
    }

    /// <summary>
    /// When Original task fails, this extension method allows to return predefined fallback value.
    /// <code>
    /// var result = await GetResultAsync().Fallback("fallback");
    /// </code>
    /// </summary>
    /// <typeparam name="TResult">Task result type.</typeparam>
    /// <param name="task">Actual executable task.</param>
    /// <param name="fallbackValue">Value to be returned when original task fails.</param>
    public static async Task<TResult> Fallback<TResult>(this Task<TResult> task, TResult fallbackValue)
    {
        try
        {
            return await task.ConfigureAwait(false);
        }
        catch
        {
            return fallbackValue;
        }
    }
}
