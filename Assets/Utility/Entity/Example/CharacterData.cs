using UnityEngine;

public class CharacterData : IEntityComponent
{
    public string Name;
    public Rigidbody2D Rig2D;
    public Collider2D Coll2D;
    public SpriteRenderer SpriteRenderer;
}

