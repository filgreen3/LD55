using System;
using System.Collections.Generic;

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
}

public static class CharacterHelper
{
    public static CharacterData GetCharacterData(this Character character) => character.GetEntityComponent<CharacterData>();
}
