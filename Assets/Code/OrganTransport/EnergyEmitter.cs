using System.Collections.Generic;

public class EnergyEmitter : IOrganComponentResourceEmmiter, IOrganComponentConnectNotify, IOrganComponentConnect
{
    public OrganResources ResourceType => OrganResources.Blood;

    private Dictionary<EnergyReciver, Transport.Connection> _connectedOrganReciver = new();

    public void EmitResource()
    {
        foreach (var item in _connectedOrganReciver)
        {
            if (item.Key.CanRecive)
            {
                var temp = item;
                temp.Value.PutResource(ResourceType, (t) => temp.Key.ReciveResource());
            }
        }
    }

    public void OnConnect(Organ parent, Organ target)
    {
        if (target.GetOrganComponent<EnergyReciver>() != null)
        {
            _connectedOrganReciver.Add(target.GetOrganComponent<EnergyReciver>(), parent.GetTransport().MakeConnection(parent, target));
        }
    }

    public void OnDisconnect(Organ parent, Organ target)
    {
        if (target.GetOrganComponent<IOrganComponentResourceReceiver>() != null)
        {
            if (target.GetOrganComponent<IOrganComponentResourceReceiver>().ResourceType != ResourceType) return;
            _connectedOrganReciver.Remove(target.GetOrganComponent<EnergyReciver>());
            parent.GetTransport().RemoveConnection(target);
        }
    }
}
