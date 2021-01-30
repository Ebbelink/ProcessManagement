using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Madailei.ProcessManagement.BpmClient.BpmProcess
{
    public abstract class BaseBpmProcess
    {
        public string InstanceIdentifier { get; set; }

        public int Version { get; set; }
        
        public abstract string BpmDefinitionName { get; }

        public abstract string ProcessIdentifier { get; }

        public abstract string ProcessName { get; }

        public Task<byte[]> GetBpmBytesAsync()
        {
            return File.ReadAllBytesAsync(BpmDefinitionName);
        }

        public abstract IEnumerable<WorkerDefinition> WorkerDefinitions();

        ///// <summary>
        ///// Create a worker for the <see cref="ProcessIdentifier"/> defined
        ///// </summary>
        ///// <param name="action">The action that needs to be executed</param>
        //public abstract void CreateWorker(Action<string> action);
    }
}
