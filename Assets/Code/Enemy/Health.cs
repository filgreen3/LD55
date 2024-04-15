using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : IEntityComponent
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _health;

    private bool _isDead;

    public Action<int> OnDamage;
    public Action OnDeath;
    public int MaxHealth => _maxHealth;
    public int HealthPoints
    {
        get => _health;
        set
        {
            if (value == _health) return;


            _health = Mathf.Clamp(value, 0, _maxHealth);
            OnDamage?.Invoke(_health);

            if (value <= 0 && !_isDead)
            {
                _isDead = true;
                OnDeath?.Invoke();
            }
        }
    }

    public Health() { }

    public Health(int maxHealth = 1)
    {
        _maxHealth = maxHealth;
    }
}


public class BloodStainDeath : IEntityComponentInit
{
    private Character _character;

    public void Init(Character character)
    {
        _character = character;
        _character.GetHealth().OnDeath += CreateBloodStain;
    }

    public void CreateBloodStain()
    {
        var bloodStain = Resources.Load<GameObject>("Fx/BloodStain");
        var pos = _character.GetCharacterData().Rig2D.position;
        pos.y = 0;
        bloodStain = GameObject.Instantiate(bloodStain, pos, Quaternion.identity);
        AudioHelper.PlayClip(ClipStorage.Instance._kill, 0.1f);
    }
}


public class DestroyOnDeath : IEntityComponentInit
{
    private Character _character;

    public void Init(Character character)
    {
        _character = character;
        _character.GetHealth().OnDeath += DestroyCharacter;
    }

    public void DestroyCharacter()
    {
        GameObject.Destroy(_character.gameObject);
    }
}