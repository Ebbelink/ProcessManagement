using System.Threading.Tasks;
using Madailei.ProcessManagement.BpmClient.BpmProcess;

namespace Madailei.ProcessManagement.BpmClient
{
    public interface IBpmClient
    {
        public Task<WorkflowIdentifierResponse> StartWorkflow(string bpmProcessId);

        public Task SendMessage(string messageName);

        public Task SendMessage<T>(string messageName, T variablesObject);

        public Task<bool> Status();

        public Task<bool> DeployFlow(string name, byte[] resourceBytes);

        void CreateWorker(string bpmIdentifier, WorkerDefinition workerDefinition);

        void StartWorkers(params BaseBpmProcess[] bpmProcesses);
    }
}
