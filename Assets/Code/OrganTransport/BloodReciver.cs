using UnityEngine;
using System;

public class BloodReciver : IOrganComponentResourceReceiver, IOrganComponentResourceStorage
{
    public bool CanRecive => ResourcesCout < _maxResources;

    public OrganResources ResourceType => OrganResources.Blood;

    public int ResourcesCout { get; set; }

    public Action<BloodReciver> OnReciveResource;

    [SerializeField] private int _maxResources;

    public void ReciveResource()
    {
        ResourcesCout++;
        OnReciveResource?.Invoke(this);
    }

}
