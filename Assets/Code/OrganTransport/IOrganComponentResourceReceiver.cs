public interface IOrganComponentResourceReceiver : IOrganComponent
{
    bool CanRecive { get; }
    OrganResources ResourceType { get; }

    void ReciveResource();
}
