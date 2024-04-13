using System;
using System.Collections.Generic;

public class BloodEmitter : IOrganComponentResourceEmmiter, IOrganComponentConnectNotify, IOrganComponentConnect
{
    public OrganResources ResourceType => OrganResources.Blood;

    private Dictionary<BloodReciver, Transport.Connection> _connectedOrganReciver = new();

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
        if (target.GetOrganComponent<BloodReciver>() != null)
        {
            _connectedOrganReciver.Add(target.GetOrganComponent<BloodReciver>(), parent.GetTransport().MakeConnection(parent, target));
        }
    }

    public void OnDisconnect(Organ parent, Organ target)
    {
        if (target.GetOrganComponent<IOrganComponentResourceReceiver>() != null)
        {
            if (target.GetOrganComponent<IOrganComponentResourceReceiver>().ResourceType != ResourceType) return;
            _connectedOrganReciver.Remove(target.GetOrganComponent<BloodReciver>());
            parent.GetTransport().RemoveConnection(target);
        }
    }
}
