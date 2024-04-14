using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Entity : MonoBehaviour
{
    public abstract void AddEntityComponent(Type compType);
    public abstract bool HasEntityComponent(Type t);
    public abstract Type ComponentType { get; }
    public abstract IEnumerable Components { get; }
    public abstract bool AllowDuplication { get; }

    public abstract System.Object GetComponentByIndex(int index);
}

public abstract class Entity<W> : Entity where W : class
{
    [SerializeReference, SerializeField] public List<W> _componentsList = new List<W>();
    [SerializeReference, SerializeField] public Dictionary<Type, W> _componentsDictionary = new Dictionary<Type, W>();

    public override Type ComponentType => typeof(W);
    public override IEnumerable Components => _componentsList;
    public bool Init { get; private set; }
    public override System.Object GetComponentByIndex(int index) => _componentsList[index];

    protected virtual void Awake()
    {
        if (AllowDuplication) return;
        foreach (var component in _componentsList)
        {
            AddCompToDictionary(component, component.GetType());
        }
        Init = true;
    }

    private void AddCompToDictionary(W component, Type type)
    {
        if (typeof(W).IsAssignableFrom(type))
        {
            if (!_componentsDictionary.TryAdd(type, component))
            {
                Debug.LogError($"Duplication of component {type.Name} in {gameObject.name}!", gameObject);
                return;
            }
            AddCompToDictionary(component, type.BaseType);
        }
    }

    public virtual T AddEntityComponent<T>(T component) where T : W
    {
        _componentsList.Add(component);
        if (!AllowDuplication && Init)
            _componentsDictionary.Add(component.GetType(), component);
        return component;
    }

    public override void AddEntityComponent(Type compType)
    {
        _componentsList.Add((W)Activator.CreateInstance(compType));
        if (!AllowDuplication && Init)
            _componentsDictionary.Add(compType, _componentsList[^1]);
    }

    public virtual T GetEntityComponent<T>() where T : W
    {
        if (!Init || AllowDuplication)
        {
            var index = _componentsList.FindIndex(x => x.GetType() == typeof(T));
            if (index == -1) throw new Exception($"No such component {typeof(T)}");
            return (T)_componentsList[index];
        }
        else
        {
            if (!_componentsDictionary.ContainsKey(typeof(T))) throw new Exception($"No such component {typeof(T)}");
            return (T)_componentsDictionary[typeof(T)];
        }
    }

    public virtual bool TryGetEntityComponent<T>(out T component) where T : W
    {
        if (HasEntityComponent(typeof(T)))
        {
            component = GetEntityComponent<T>();
            return true;
        }
        component = default(T);
        return false;
    }

    public override bool HasEntityComponent(Type t)
    {
        if (!Init || AllowDuplication)
        {
            foreach (var component in _componentsList)
            {
                if (component != null && t == component.GetType())
                {
                    return true;
                }
            }
        }
        else
        {
            return _componentsDictionary.ContainsKey(t);
        }
        return false;
    }
}
