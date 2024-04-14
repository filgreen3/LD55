using UnityEngine;
using System;

public class BloodReciver : IOrganComponentResourceReceiver, IOrganComponentResourceStorage
{
    public virtual bool CanRecive => ResourcesCout < _maxResources;

    public OrganResources ResourceType => OrganResources.Blood;

    public int ResourcesCout { get; set; }

    public Action<BloodReciver> OnReciveResource;

    [SerializeField] private int _maxResources;

    public virtual void ReciveResource()
    {
        ResourcesCout++;
        OnReciveResource?.Invoke(this);
    }
}