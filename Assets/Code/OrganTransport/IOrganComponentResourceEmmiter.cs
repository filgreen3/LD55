public interface IOrganComponentResourceEmmiter : IOrganComponent
{
    OrganResources ResourceType { get; }

    void EmitResource();
}
