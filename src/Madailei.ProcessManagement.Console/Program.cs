using System;
using System.Linq;
using System.Threading.Tasks;
using Madailei.ProcessManagement.BpmClient;
using Madailei.ProcessManagement.BpmClient.BpmProcess;
using Madailei.ProcessManagement.BpmClient.Zeebe;
using Madailei.ProcessManagement.Console.BpmnFlows;
using Madailei.ProcessManagement.Console.BpmnFlows.OrderProcessWithPayment.Models;
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

            client.StartWorkers(new TestProcess(), new SalesProcess(), new OrderWithPaymentProcess());
            System.Console.WriteLine("All workers have been started");

            System.Console.Write("Finished booting, ready to rock!");

            string latestOrderId = null;

            while (true)
            {
                string option = System.Console.ReadLine();
                switch (option)
                {
                    case "1":
                        client.StartWorkflow(new SalesProcess().ProcessIdentifier).GetAwaiter().GetResult();
                        System.Console.WriteLine($"Initiated a new {nameof(SalesProcess)}");
                        break;
                    case "2":
                        latestOrderId = Guid.NewGuid().ToString();

                        client.StartWorkflow(
                            new OrderWithPaymentProcess().ProcessIdentifier, 
                            new OrderWithPayment_InstanstiationRequest { OrderId = latestOrderId})
                                .GetAwaiter().GetResult();

                        System.Console.WriteLine($"Initiated a new {nameof(OrderWithPaymentProcess)}");
                        break;
                    case "3":
                        string messageName = "payment-received";
                        client.SendMessage(
                            messageName, 
                            latestOrderId, 
                            new OrderWithPayment_PaymentReceivedRequest()
                            {
                                OrderId = latestOrderId,
                                OrderValue = new Random().Next(200),                                
                            })
                                .GetAwaiter().GetResult();
                        System.Console.WriteLine($"Send a '{messageName}' message");
                        break;
                    default:
                        continue;
                }
            }
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

