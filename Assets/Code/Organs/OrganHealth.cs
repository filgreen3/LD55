using UnityEngine;
using System;

public class OrganHealth : IOrganComponent, IOrganComponentInit
{
    [SerializeField] private int _value;
    [SerializeField] private int _maxValue;

    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            if (IsDead)
                OnDeath?.Invoke();
        }
    }
    public int MaxValue { get => _maxValue; set => _maxValue = value; }
    public float Percentage => (float)Value / MaxValue;
    public bool IsDead => Value <= 0;
    public bool IsFull => Value >= MaxValue;

    public Action OnDeath;

    public void Init(Organ part)
    {
        Value = MaxValue;

        OnDeath += () => part.DisconnectAll();
        OnDeath += () => GameObject.Destroy(part.gameObject);
    }
}
