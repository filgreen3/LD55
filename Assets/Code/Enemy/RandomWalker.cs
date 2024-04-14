using UnityEngine;

public class RandomWalker : IEntityComponent, IEntityComponentInit
{
    private Character _character;

    public void Init(Character character)
    {
        _character = character;
        Move();
    }

    private void Move()
    {
        var pos = _character.GetCharacterData().Rig2D.position;
        pos.x += Random.Range(-5, 5);
        pos.y = 0;
        _character.GetEntityComponent<Walker>().SetTarget(pos, Move);
    }
}
