using System.Collections.Generic;

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
            if (target.GetOrganComponent<IOrganComponentResourceReceiver>().ResourceType != ResourceType) return;
            _connectedOrganReciver.Add(target.GetOrganComponent<IOrganComponentResourceReceiver>());
            parent.GetTransport().MakeConnection(parent, target);
        }
    }

    public void OnDisconnect(Organ parent, Organ target)
    {
        if (target.GetOrganComponent<IOrganComponentResourceReceiver>() != null)
        {
            if (target.GetOrganComponent<IOrganComponentResourceReceiver>().ResourceType != ResourceType) return;
            _connectedOrganReciver.Remove(target.GetOrganComponent<IOrganComponentResourceReceiver>());
            parent.GetTransport().RemoveConnection(parent, target);
        }
    }
}
