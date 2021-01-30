using System;
using System.Linq;
using System.Threading.Tasks;
using Madailei.ProcessManagement.BpmClient;
using Madailei.ProcessManagement.BpmClient.BpmProcess;
using Madailei.ProcessManagement.BpmClient.Zeebe;
using Madailei.ProcessManagement.Console.BpmnFlows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Madailei.ProcessManagement.Console
{
    class Program
    {
        private const string BpmDefinitions = "./BpmnFlows";

        static void Main(string[] args)
        {
            System.Console.Write("Starting up");

            var serviceProvider = SetupServiceProvider();
            System.Console.Write(" ...");
            
            var client = serviceProvider.GetService<IBpmClient>();
            System.Console.WriteLine(" ...");

            var status = client.Status().GetAwaiter().GetResult();
            System.Console.WriteLine($"The client is {(status ? "connected" : "dead")}");

            System.Console.WriteLine("Deploying BPM flows");
            InitializeBpmFlows(client).GetAwaiter().GetResult();

            System.Console.Write("Finished booting, ready to rock!");
            
            var salesProces = new SalesProcess();
            var result = client.StartWorkflow(salesProces.ProcessIdentifier).GetAwaiter().GetResult();
            System.Console.WriteLine("Initiated a new workflow");

            client.StartWorkers(new TestProcess(), new SalesProcess());
            System.Console.WriteLine($"All workers have been started");

            System.Console.WriteLine("Press enter to exit");
            System.Console.ReadLine();
        }

        private static async Task InitializeBpmFlows(IBpmClient client)
        {
            var bpmProcesses = typeof(Program)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BaseBpmProcess)) && !t.IsAbstract)
                .Select(t => (BaseBpmProcess)Activator.CreateInstance(t));

            foreach (var bpmProcess in bpmProcesses)
            {
                await client.DeployFlow(bpmProcess.BpmDefinitionName, await bpmProcess.GetBpmBytesAsync());
            }
        }

        private static ServiceProvider SetupServiceProvider()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .AddOptions()
                .AddConfiguration()
                .AddBpmClientZeebe()
                .BuildServiceProvider();
            return serviceProvider;
        }
    }
}

