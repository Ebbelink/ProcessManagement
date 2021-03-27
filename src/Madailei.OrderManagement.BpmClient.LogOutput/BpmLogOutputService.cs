using System;
using System.Threading.Tasks;
using FluentValidation;
using Madailei.ProcessManagement.BpmClient.BpmProcess;
using Madailei.ProcessManagement.BpmClient.Config;
using Newtonsoft.Json;

namespace Madailei.ProcessManagement.BpmClient.LogOutput
{
    public class BpmLogOutputService : IBpmClient
    {
        public BpmLogOutputService(BpmServerConnectionSettings bpmOptions)
        {
            var validator = new BpmServerConnectionSettingsValidator();
            validator.ValidateAndThrow(bpmOptions);

            Console.Write("Created a client");
        }

        public void CreateWorker(BaseBpmProcess bpmProcess)
        {
            Console.Write($"Created a worker for bpm process {bpmProcess.ProcessIdentifier}");
        }

        public void CreateWorker(string bpmIdentifier, WorkerDefinition workerDefinition)
        {
            Console.WriteLine("Created tons of workers");
        }

        public void StartWorkers(params BaseBpmProcess[] bpmProcesses)
        {
            Console.WriteLine("Started all workers");
        }

        public Task<WorkflowIdentifierResponse> StartWorkflow(string bpmProcessId)
        {
            Console.WriteLine($"Started workflow for process id {bpmProcessId}");
            
            return Task.FromResult(new WorkflowIdentifierResponse("LOG_INSTANCE", 1));
        }

        public Task<WorkflowIdentifierResponse> StartWorkflow<T>(string bpmProcessId, T instantiationVariables)
        {
            Console.WriteLine($"Started workflow for process id {bpmProcessId}");
            Console.WriteLine($"Using variables: {JsonConvert.SerializeObject(instantiationVariables, Formatting.Indented)}");

            return Task.FromResult(new WorkflowIdentifierResponse("LOG_INSTANCE", 1));
        }

        public Task<bool> DeployFlow(string name, byte[] resourceBytes)
        {
            Console.WriteLine($"Deployed workflow {name}");
            
            return Task.FromResult(true);
        }

        public Task<bool> Status()
        {
            Console.WriteLine("The sun is shining and everything is perfect");
            
            return Task.FromResult(true);
        }

        public Task SendMessage(string messageName, string correlationKey)
        {
            Console.WriteLine($"Sending message {messageName} with correlation {correlationKey}");

            return Task.FromResult(true);
        }

        public Task SendMessage<T>(string messageName, string correlationKey, T variablesObject)
        {
            Console.WriteLine($"Sending message {messageName} with correlation {correlationKey}");
            Console.WriteLine($"Using variables: {JsonConvert.SerializeObject(variablesObject, Formatting.Indented)}");

            return Task.FromResult(true);
        }
    }
}
