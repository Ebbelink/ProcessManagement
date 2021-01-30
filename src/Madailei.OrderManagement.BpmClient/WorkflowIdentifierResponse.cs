namespace Madailei.ProcessManagement.BpmClient
{
    public class WorkflowIdentifierResponse
    {
        public WorkflowIdentifierResponse(string instanceId, int version)
        {
            InstanceIdentifier = instanceId;
            Version = version;
        }
        
        public string InstanceIdentifier { get; }

        public int Version { get; }
    }
}
