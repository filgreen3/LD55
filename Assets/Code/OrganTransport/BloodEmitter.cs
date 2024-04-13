using System.Collections.Generic;
using UnityEngine;

public class BloodEmitter : IOrganComponentResourceEmmiter, IOrganComponentConnectNotify, IOrganComponentConnect, IOrganComponentUpdate
{
    public OrganResources ResourceType { get; } = OrganResources.Blood;

    private Dictionary<IOrganComponentResourceReceiver, Transport.Connection> _connectedOrganReciver = new();


    [SerializeField] private float _emitRate = 2f;
    private float _emitTimer = 0f;

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
        if (target.GetOrganComponent<IOrganComponentResourceReceiver>() != null)
        {
            if (target.GetOrganComponent<IOrganComponentResourceReceiver>().ResourceType != ResourceType) return;
            _connectedOrganReciver.Add(target.GetOrganComponent<IOrganComponentResourceReceiver>(), parent.GetTransport().MakeConnection(parent, target));
        }
    }

    public void OnDisconnect(Organ parent, Organ target)
    {
        if (target.GetOrganComponent<IOrganComponentResourceReceiver>() != null)
        {
            if (target.GetOrganComponent<IOrganComponentResourceReceiver>().ResourceType != ResourceType) return;
            _connectedOrganReciver.Remove(target.GetOrganComponent<IOrganComponentResourceReceiver>());
            parent.GetTransport().RemoveConnection(target);
            _connectedOrganReciver.Remove(target.GetOrganComponent<IOrganComponentResourceReceiver>());
        }
    }

    public void Update()
    {
        _emitTimer += Time.deltaTime;
        if (_emitTimer >= _emitRate)
        {
            EmitResource();
            _emitTimer = 0f;
        }
    }
}
