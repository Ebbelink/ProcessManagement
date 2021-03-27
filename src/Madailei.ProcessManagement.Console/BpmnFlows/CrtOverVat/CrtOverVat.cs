using System;
using System.Collections.Generic;
using AutoFixture;
using Madailei.ProcessManagement.BpmClient.BpmProcess;
using Madailei.ProcessManagement.Console.BpmnFlows.CrtOverVat.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Madailei.ProcessManagement.Console.BpmnFlows.CrtOverVat
{
    public class CrtOverVat : BaseBpmProcess
    {
        public override string BpmDefinitionName { get; } = "./BpmnFlows/CrtOverVat/CrtOverVat.bpmn";

        public override string ProcessIdentifier { get; } = "crt-over-vat";
        
        public override string ProcessName { get; } = "CRT over VAT";

        private static JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };

        public override IEnumerable<WorkerDefinition> WorkerDefinitions()
        {
            var list = new List<WorkerDefinition>()
            {
                new WorkerDefinition(RetrieveVehicleInformation_WorkerIdentifier, RetrieveVehicleInformation_WorkerAction),
                new WorkerDefinition(NotifyUnknownDate_WorkerIdentifier, NotifyUnknownDate_WorkerAction),
                new WorkerDefinition(CalculateTax_WorkerIdentifier, CalculateTax_WorkerAction),
                new WorkerDefinition(CalculateCrt_WorkerIdentifier, CalculateCrt_WorkerAction),
            };

            return list;
        }

        private const string RetrieveVehicleInformation_WorkerIdentifier = "retrieve-vehicle-information";
        
        private string RetrieveVehicleInformation_WorkerAction(string instanceKey)
        {
            Fixture fix = new Fixture();
            var vehicle = fix.Create<Vehicle>();

            return JsonConvert.SerializeObject(vehicle);
        }

        private const string NotifyUnknownDate_WorkerIdentifier = "notify-unknown-date";

        private string NotifyUnknownDate_WorkerAction(string instanceKey)
        {
            var tmpColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.Red;
            
            System.Console.WriteLine("The date is not known");

            System.Console.ForegroundColor = tmpColor;

            return null;
        }

        private const string CalculateTax_WorkerIdentifier = "calculate-tax";

        private string CalculateTax_WorkerAction(string instanceKey)
        {
            System.Console.WriteLine("Calculating taxes");

            return null;
        }

        private const string CalculateCrt_WorkerIdentifier = "calculate-crt";

        private string CalculateCrt_WorkerAction(string instanceKey)
        {
            System.Console.WriteLine("Calculating registration taxes");

            return null;
        }
    }
}
