using System;

namespace Madailei.ProcessManagement.BpmClient.BpmProcess
{
    public class WorkerDefinition
    {
        public WorkerDefinition(string identifier, Func<string, string> action)
        {
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }
        
        public string Identifier { get; }

        /// <summary>
        /// Custom action to take when a worker is invoked. Input param is the unique id for the process.
        /// Output is optional. If any should be JSON that will serve as input for the process
        /// </summary>
        public Func<string, string> Action { get; }
    }
}
