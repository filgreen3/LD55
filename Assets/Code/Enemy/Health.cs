using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : IEntityComponent
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _health;

    public Action<int> OnDamage;
    public Action OnDeath;
    public int MaxHealth => _maxHealth;
    public int HealthPoints
    {
        get => _health;
        set
        {
            if (value == _health) return;

            if (value <= 0 && _health > 0)
                OnDeath?.Invoke();
            _health = Mathf.Clamp(value, 0, _maxHealth);
            OnDamage?.Invoke(_health);
        }
    }

    public Health() { }

    public Health(int maxHealth = 1)
    {
        _maxHealth = maxHealth;
    }
}

