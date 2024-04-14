using UnityEngine;
using System;

public class ManReciver : IOrganComponentResourceReceiver, IOrganComponentResourceStorage
{
    public virtual bool CanRecive => ResourcesCout < _maxResources;

    public OrganResources ResourceType => OrganResources.Man;

    public int ResourcesCout { get; set; }

    public Action<ManReciver> OnReciveResource;

    [SerializeField] private int _maxResources;

    public virtual void ReciveResource()
    {
        ResourcesCout++;
        OnReciveResource?.Invoke(this);
    }
}