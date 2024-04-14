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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.right * Size * 2 + Vector3.up * 2);
        Gizmos.color = Color.Lerp(Color.red, Color.clear, 0.8f);
        Gizmos.DrawCube(transform.position, Vector3.right * Size * 2 + Vector3.up * 2);
    }
}



public interface ITownComponent
{
    void Generate(TownGenerator generator);
}
