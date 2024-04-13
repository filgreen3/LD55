using UnityEngine;

public class OrganTireComponent : IOrganComponent
{
    [SerializeField] private int _tire;
    public bool IsOpen => OrganTierSystem.TireLevel >= _tire;
}
