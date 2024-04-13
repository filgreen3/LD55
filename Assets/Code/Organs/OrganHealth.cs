using UnityEngine;

public class OrganHealth : IOrganComponent
{
    [SerializeField] private int _value;
    [SerializeField] private int _maxValue;

    public int Value { get => _value; set => _value = value; }
    public int MaxValue { get => _maxValue; set => _maxValue = value; }
    public float Percentage => (float)Value / MaxValue;
    public bool IsDead => Value <= 0;
    public bool IsFull => Value >= MaxValue;
}
