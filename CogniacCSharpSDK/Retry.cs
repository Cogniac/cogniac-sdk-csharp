using System;
using System.Collections.Generic;
using System.Threading;

namespace Cogniac
{
    /// <summary>
    /// This class performs retry logic for any call that can fail
    /// Use as follows (example with int return): 
    /// int result = Retry.Do(SomeFunctionWhichReturnsInt, TimeSpan.FromSeconds(1), 4);
    /// The above will try 4 times at a 1 second interval
    /// </summary>
    public static class Retry
    {
        /// <summary>
        /// Main method which runs a retry-able action
        /// </summary>
        /// <param name="action">Function call to retry</param>
        /// <param name="retryInterval">TimeSpan object to use as time-out interval</param>
        /// <param name="maxAttemptCount">Maximum number of retries</param>
        public static void Do(Action action, TimeSpan retryInterval, int maxAttemptCount = 3)
        {
            Do<object>(() =>
            {
                action();
                return null;
            }, retryInterval, maxAttemptCount);
        }

        /// <summary>
        /// Same as main method but takes a function and retries it
        /// </summary>
        /// <typeparam name="T">Type (int, etc...)</typeparam>
        /// <param name="action">Function to run</param>
        /// <param name="retryInterval">TimeSpan object to use as time-out interval</param>
        /// <param name="maxAttemptCount">Maximum number of retries</param>
        /// <returns>The return is the same return of the function passed to 'action'</returns>
        public static T Do<T>(Func<T> action, TimeSpan retryInterval, int maxAttemptCount = 3)
        {
            var exceptions = new List<Exception>();
            for (int attempted = 0; attempted < maxAttemptCount; attempted++)
            {
                try
                {
                    if (attempted > 0)
                    {
                        Thread.Sleep(retryInterval);
                    }
                    return action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            throw new AggregateException(exceptions);
        }
    }
}
