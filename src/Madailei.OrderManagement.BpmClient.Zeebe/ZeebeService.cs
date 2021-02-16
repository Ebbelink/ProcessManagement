using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Madailei.ProcessManagement.BpmClient.BpmProcess;
using Madailei.ProcessManagement.BpmClient.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Impl.Builder;

namespace Madailei.ProcessManagement.BpmClient.Zeebe
{
    public class ZeebeService : IBpmClient
    {
        private readonly IZeebeClient _client;

        private static JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };

        public ZeebeService(BpmServerConnectionSettings bpmOptions)
        {
            var validator = new BpmServerConnectionSettingsValidator();
            validator.ValidateAndThrow(bpmOptions);

            //Discard any potential :443 port at the end
            char[] port = { '4', '3', ':' };
            var audience = bpmOptions.Address.TrimEnd(port);

            _client =
                ZeebeClient.Builder()
                    .UseGatewayAddress(bpmOptions.Address)
                    .UseTransportEncryption()
                    .UseAccessTokenSupplier(
                        CamundaCloudTokenProvider.Builder()
                            .UseAuthServer(bpmOptions.AuthServer)
                            .UseClientId(bpmOptions.ClientId)
                            .UseClientSecret(bpmOptions.ClientSecret)
                            .UseAudience(audience)
                            .Build())
                    .Build();
        }

        public void CreateWorker(string bpmIdentifier, WorkerDefinition workerDefinition)
        {
            _client.NewWorker()
                .JobType(workerDefinition.Identifier)
                .Handler(async (client, job) =>
                {
                    Console.WriteLine("Received job: " + job);
                    Console.WriteLine($"Start executing custom action for process {bpmIdentifier}");

                    string result = workerDefinition.Action.Invoke(job.Key.ToString());

                    Console.WriteLine($"Finished executing custom action for process {bpmIdentifier}");

                    if (result == null)   
                    {
                        await client.NewCompleteJobCommand(job.Key)
                            .Send();
                    }
                    else
                    {
                        await client.NewCompleteJobCommand(job.Key)
                            .Variables(result)
                            .Send();
                    }
                })
                .MaxJobsActive(500)
                .Name(workerDefinition.Identifier)
                .PollInterval(TimeSpan.FromSeconds(50))
                .PollingTimeout(TimeSpan.FromSeconds(50))
                .Timeout(TimeSpan.FromSeconds(10))
                .Open();
        }

        public void StartWorkers(params BaseBpmProcess[] bpmProcesses)
        {
            foreach (var baseBpmProcess in bpmProcesses)
            {
                foreach (var workerDefinition in baseBpmProcess.WorkerDefinitions())
                {
                    CreateWorker(baseBpmProcess.ProcessIdentifier, workerDefinition);
                }
            }
        }

        public async Task<WorkflowIdentifierResponse> StartWorkflow(string bpmProcessId)
        {
            var workflowInstance = await _client
                .NewCreateWorkflowInstanceCommand()
                .BpmnProcessId(bpmProcessId)
                .LatestVersion()
                .Send();

            return new WorkflowIdentifierResponse(workflowInstance.WorkflowInstanceKey.ToString(), workflowInstance.Version);
        }

        public async Task<WorkflowIdentifierResponse> StartWorkflow<T>(string bpmProcessId, T instantiationVariables)
        {
            string variablesJson = JsonConvert.SerializeObject(instantiationVariables, _jsonSettings);

            var workflowInstance = await _client
                .NewCreateWorkflowInstanceCommand()
                .BpmnProcessId(bpmProcessId)
                .LatestVersion()
                .Variables(variablesJson)
                .Send();

            return new WorkflowIdentifierResponse(workflowInstance.WorkflowInstanceKey.ToString(), workflowInstance.Version);
        }

        public Task<ITopology> Status()
        {
            return _client.TopologyRequest().Send();
        }

        public async Task<bool> DeployFlow(string name, byte[] resourceBytes)
        {
            var result = await _client.NewDeployCommand()
                .AddResourceBytes(resourceBytes, name)
                .Send();

            return result != null && result.Key >= 0;
        }

        async Task<bool> IBpmClient.Status()
        {
            var statusResult = await Status();
            return statusResult.Brokers.Any() ? true : false;
        }

        public async Task SendMessage(string messageName, string correlationKey)
        {
            await _client.NewPublishMessageCommand()
                .MessageName(messageName)
                .CorrelationKey(correlationKey)
                .Send();
        }

        public async Task SendMessage<T>(string messageName, string correlationKey, T variablesObject)
        {
            string variablesJson = JsonConvert.SerializeObject(variablesObject, _jsonSettings);

            await _client.NewPublishMessageCommand()
                .MessageName(messageName)
                .CorrelationKey(correlationKey)
                .Variables(variablesJson)
                .Send();
        }
    }
}
