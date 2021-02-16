using System;
using System.Collections.Generic;
using Madailei.ProcessManagement.BpmClient.BpmProcess;

namespace Madailei.ProcessManagement.Console.BpmnFlows
{
    public class SalesProcess : BaseBpmProcess
    {
        public override string BpmDefinitionName { get; } = "./BpmnFlows/SalesProcess/SalesProcess.bpmn";

        public override string ProcessIdentifier { get; } = "sales-process";
        
        public override string ProcessName { get; } = "Sales Process";
        
        public override IEnumerable<WorkerDefinition> WorkerDefinitions()
        {
            var list = new List<WorkerDefinition>()
            {
                new WorkerDefinition(GetSalesChannel_WorkerIdentifier, GetSalesChannel_WorkerAction),
                new WorkerDefinition(SendConsumerInvoice_WorkerIdentifier, SendConsumerInvoice_WorkerAction),
                new WorkerDefinition(SendBusinessInvoice_WorkerIdentifier, SendBusinessInvoice_WorkerAction),
            };

            return list;
        }

        private const string GetSalesChannel_WorkerIdentifier = "get-sales-channel";
        
        private string GetSalesChannel_WorkerAction(string instanceKey)
        {
            int mode = new Random().Next(4);
            string salesChannel = null;
            if (mode == 1)
            {
                salesChannel = "consumer";
            }
            else if (mode == 2)
            {
                salesChannel = "business";
            }
            else if (mode == 3)
            {
                salesChannel = "broken";
            }
            else 
            {
                throw new Exception("Shit hit the fan");
            }

            System.Console.WriteLine($"The sales channel is {salesChannel}");

            return $"{{\"salesChannel\":\"{salesChannel}\"}}"; ;
        }

        private const string SendConsumerInvoice_WorkerIdentifier = "send-consumer-invoice";

        private string SendConsumerInvoice_WorkerAction(string instanceKey)
        {
            System.Console.WriteLine("Send the consumer invoice");
            
            return null;
        }

        private const string SendBusinessInvoice_WorkerIdentifier = "send-business-invoice";

        private string SendBusinessInvoice_WorkerAction(string instanceKey)
        {
            System.Console.WriteLine("Send the business invoice");

            return null;
        }
    }
}
