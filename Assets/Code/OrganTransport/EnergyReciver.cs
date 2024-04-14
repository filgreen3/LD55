using UnityEngine;
using System;

public class EnergyReciver : IOrganComponentResourceReceiver, IOrganComponentResourceStorage
{
    public virtual bool CanRecive => ResourcesCout < _maxResources;

    public virtual OrganResources ResourceType => OrganResources.Energy;

    public int ResourcesCout { get; set; }

    public Action<EnergyReciver> OnReciveResource;

    [SerializeField] private int _maxResources;

    public virtual void ReciveResource()
    {
        ResourcesCout++;
        OnReciveResource?.Invoke(this);
    }
}
