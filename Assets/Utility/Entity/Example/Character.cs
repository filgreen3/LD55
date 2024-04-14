using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity<IEntityComponent>
{
    public override bool AllowDuplication => false;
    private readonly HashSet<IEntityComponentUpdate> _updateComponents = new();

    public Action AfterInit;

    protected override void Awake()
    {
        base.Awake();
        foreach (var component in Components)
        {
            if (component is IEntityComponentInit initable)
                initable.Init(this);
            if (component is IEntityComponentUpdate updateable)
                _updateComponents.Add(updateable);
        }

        AfterInit?.Invoke();
        AfterInit = null;
    }

    private void Update()
    {
        foreach (var comp in _updateComponents)
        {
            comp.Update();
        }
    }

    public override T AddEntityComponent<T>(T component)
    {
        var result = base.AddEntityComponent(component);
        if (Init)
        {
            (component as IEntityComponentInit)?.Init(this);
        }
        if (component is IEntityComponentUpdate updateComp)
            _updateComponents.Add(updateComp);
        return result;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (Mathf.Abs(other.relativeVelocity.y) > 10 && other.gameObject.layer == LayerMask.NameToLayer("Part"))
        {
            this.GetHealth().HealthPoints = 0;
        }
    }
}

public static class CharacterHelper
{
    public static CharacterData GetCharacterData(this Character character) => character.GetEntityComponent<CharacterData>();
    public static Health GetHealth(this Character character) => character.GetEntityComponent<Health>();
    public static ChracterAnimator GetAnimator(this Character character) => character.GetEntityComponent<ChracterAnimator>();
    public static Walker GetWalker(this Character character) => character.GetEntityComponent<Walker>();

}

