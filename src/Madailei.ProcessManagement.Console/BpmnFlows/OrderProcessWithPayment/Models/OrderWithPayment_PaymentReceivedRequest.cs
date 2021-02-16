using System;
using System.Collections.Generic;
using System.Text;

namespace Madailei.ProcessManagement.Console.BpmnFlows.OrderProcessWithPayment.Models
{
    public class OrderWithPayment_PaymentReceivedRequest
    {
        public string OrderId { get; set; }
        
        public int OrderValue { get; set; }
    }
}
