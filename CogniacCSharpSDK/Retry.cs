/*
    Copyright 2018 Cogniac Corporation.

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/

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
    internal static class Retry
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
