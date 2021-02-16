using System;
using System.Collections.Generic;
using Madailei.ProcessManagement.BpmClient.BpmProcess;

namespace Madailei.ProcessManagement.Console.BpmnFlows
{
    public class OrderWithPaymentProcess : BaseBpmProcess
    {
        public override string BpmDefinitionName { get; } = "./BpmnFlows/OrderProcessWithPayment.bpmn";

        public override string ProcessIdentifier { get; } = "order-process-with-payment";
        
        public override string ProcessName { get; } = "Order Process With Payment";
        
        public override IEnumerable<WorkerDefinition> WorkerDefinitions()
        {
            var list = new List<WorkerDefinition>()
            {
                new WorkerDefinition(InitiatePayment_WorkerIdentifier, InitiatePayment_WorkerAction),
                new WorkerDefinition(ShipWithoutInsurance_WorkerIdentifier, ShipWithoutInsurance_WorkerAction),
                new WorkerDefinition(ShipWithInsurance_WorkerIdentifier, ShipWithInsurance_WorkerAction),
            };

            return list;
        }

        private const string InitiatePayment_WorkerIdentifier = "initiate-payment";
        
        private string InitiatePayment_WorkerAction(string instanceKey)
        {
            System.Console.WriteLine("Initiate Payment");

            return null;
        }

        private const string ShipWithoutInsurance_WorkerIdentifier = "ship-without-insurance";

        private string ShipWithoutInsurance_WorkerAction(string instanceKey)
        {
            System.Console.WriteLine("Ship Without Insurance");
            
            return null;
        }

        private const string ShipWithInsurance_WorkerIdentifier = "ship-with-insurance";

        private string ShipWithInsurance_WorkerAction(string instanceKey)
        {
            System.Console.WriteLine("Ship With Insurance");

            return null;
        }
    }

    public class OrderWithPayment_NewProcessRequest
    {
        public string OrderId { get; set; }
    }
}
