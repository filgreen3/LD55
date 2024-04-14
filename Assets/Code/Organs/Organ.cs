using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Organ : Connectable
{
    private readonly HashSet<IOrganComponentConnect> _connectComponents = new();
    private readonly HashSet<IOrganComponentUpdate> _updateComponents = new();

    public string Display => name;
    public Action AfterInit;

    public static Action<Organ> OnPartDown;
    public static Action<Organ> OnPartUp;

    public static Action<Organ> OrganDestroyed;

    protected override void Awake()
    {
        base.Awake();
        foreach (var component in Components)
        {
            if (component is IOrganComponentInit initable)
                initable.Init(this);
            if (component is IOrganComponentUpdate updateable)
                _updateComponents.Add(updateable);
            if (component is IOrganComponentConnect connectable)
                _connectComponents.Add(connectable);
        }

        AfterInit?.Invoke();
        AfterInit = null;
    }

    private void Start()
    {
        Joints = GetComponents<Joint2D>().ToHashSet();
    }

    public override T AddEntityComponent<T>(T component)
    {
        var result = base.AddEntityComponent(component);
        if (Init)
        {
            (component as IOrganComponentInit)?.Init(this);
        }
        if (component is IOrganComponentUpdate updateComp)
            _updateComponents.Add(updateComp);
        return result;
    }

    public override void Connect(Connectable target)
    {
        base.Connect(target);
        foreach (var comp in _connectComponents)
        {
            comp.OnConnect(this, target as Organ);
        }
        var list = target.GetOrganComponents<IOrganComponentConnectNotify>();
        foreach (var comp in list)
        {
            comp.OnConnect(target as Organ, this);
        }
    }

    public override void DisconnectAll()
    {
        foreach (var comp in _connectComponents)
        {
            foreach (var connectable in ConnectedParts)
            {
                comp.OnDisconnect(this, connectable as Organ);
            }
        }
        foreach (var part in ConnectedParts)
        {
            part.Disconnect(this);
        }
        foreach (var joint in Joints)
        {
            Destroy(joint);
        }
        Joints.Clear();
        ConnectedParts.Clear();
        PartCollider.enabled = false;
        OnDisconnect?.Invoke(this);
    }

    private void FixedUpdate()
    {
        foreach (var comp in _updateComponents)
        {
            comp.Update();
        }
    }

    public override void Disconnect(Connectable conected)
    {
        base.Disconnect(conected);

        foreach (var comp in _connectComponents)
        {
            comp.OnDisconnect(this, conected as Organ);
        }
        ConnectedParts.Remove(conected);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (CanConnect && other.relativeVelocity.sqrMagnitude > 1f)
        {

        }
    }

    public bool HasOrganComponent<T>()
    {
        foreach (var comp in Components)
        {
            if (comp is T)
                return true;
        }
        return false;
    }


    private void OnDestroy()
    {
        if (this != null)
            OrganDestroyed?.Invoke(this);
    }
}

public static class OrganExtensions
{
    public static OrganRender GetRender(this Organ part) => part.GetEntityComponent<OrganRender>();
    public static OrganTireComponent GetTier(this Organ part) => part.GetEntityComponent<OrganTireComponent>();
    public static Transport GetTransport(this Organ part) => part.GetEntityComponent<Transport>();
    public static OrganHealth GetHealth(this Organ part) => part.GetEntityComponent<OrganHealth>();
}
