using System.Collections;
using System.Collections.Generic;

public class Transport : IOrganComponent
{

}

public interface IOrganComponentResourceEmmiter : IOrganComponent
{
    OrganResources ResourceType { get; }

    void EmitResource();
}

public interface IOrganComponentResourceReceiver : IOrganComponent
{
    bool CanRecive { get; }
    OrganResources ResourceType { get; }

    void ReciveResource();
}

public interface IOrganComponentResourceStorage : IOrganComponentResourceReceiver
{
    int ResourcesCout { get; }
}

public enum OrganResources
{
    None,
    Food,
    Blood,
    Energy
}


public class BloodEmitter : IOrganComponentResourceEmmiter, IOrganComponentConnect
{
    public OrganResources ResourceType { get; } = OrganResources.Blood;

    private List<IOrganComponentResourceReceiver> _connectedOrganReciver = new();

    public void EmitResource()
    {
        foreach (var receiver in _connectedOrganReciver)
        {
            receiver.ReciveResource();
        }
    }

    public void OnConnect(Organ parent, Organ target)
    {
        if (target.GetOrganComponent<IOrganComponentResourceReceiver>() != null)
        {
            _connectedOrganReciver.Add(target.GetOrganComponent<IOrganComponentResourceReceiver>());
        }
    }

    public void OnDisconnect(Organ parent, Organ target)
    {
        if (target.GetOrganComponent<IOrganComponentResourceReceiver>() != null)
        {
            _connectedOrganReciver.Remove(target.GetOrganComponent<IOrganComponentResourceReceiver>());
        }
    }
}
