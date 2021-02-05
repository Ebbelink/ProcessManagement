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

        public async Task<byte[]> GetBpmBytesAsync()
        {
            byte[] result;
            using (FileStream stream = File.Open(BpmDefinitionName, FileMode.Open))
            {
                result = new byte[stream.Length];
                await stream.ReadAsync(result, 0, (int)stream.Length);
                return result;
            }
        }

        public abstract IEnumerable<WorkerDefinition> WorkerDefinitions();
    }
}
