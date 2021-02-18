using System;

namespace Madailei.ProcessManagement.Console.BpmnFlows.CrtOverVat.Models
{
    public class WorkflowInstanstiationRequest
    {
        public WorkflowInstanstiationRequest(string vin, string countryCode, DateTime calculationDateTime)
        {
            Vin = vin ?? throw new ArgumentNullException(nameof(vin));
            CountryCode = countryCode ?? throw new ArgumentNullException(nameof(countryCode));
            _calculationDateTime = calculationDateTime;
        }

        public string Vin;

        public string CountryCode;

        private DateTime _calculationDateTime;

        public string CalculationDate => _calculationDateTime.ToString("yyyy-MM-dd");
    }
}
