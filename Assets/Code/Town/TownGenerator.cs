using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGenerator : Entity<ITownComponent>
{
    public override bool AllowDuplication => true;

    public int TownLevel { get; set; }
    public Vector3 CenterPosition => transform.position;
    public float Size;

    public Action OnTownLost;

    public void Generate(TownGenerator generator)
    {
        foreach (var component in Components)
        {
            (component as ITownComponent)?.Generate(generator);
        }
    }
}

public interface ITownComponent
{
    void Generate(TownGenerator generator);
}
