using System;
using System.Collections.Generic;
using Madailei.ProcessManagement.BpmClient.BpmProcess;

namespace Madailei.ProcessManagement.Console.BpmnFlows
{
    public class TestProcess : BaseBpmProcess
    {
        public override string BpmDefinitionName { get; } = "./BpmnFlows/TestProcess/TestProcess.bpmn";

        public override string ProcessIdentifier { get; } = "test-process";
        
        public override string ProcessName { get; } = "Test Process";
        
        public override IEnumerable<WorkerDefinition> WorkerDefinitions()
        {
            var list = new List<WorkerDefinition>();
            
            list.Add(new WorkerDefinition(GetTimeWorkerIdentifier, GetTimeWorkerAction));
            
            return list;
        }

        private const string GetTimeWorkerIdentifier = "get-time";
        
        private string GetTimeWorkerAction(string instanceKey)
        {
            return DateTime.Now.ToString("yyyy-MM-dd:hh:mm:ss");
        }
    }
}
