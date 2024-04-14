using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RandomWalker : IEntityComponent, IEntityComponentInit
{
    private Character _character;
    private CenterTarget _centerTarget;

    public void Init(Character character)
    {
        _character = character;
        _centerTarget = character.GetEntityComponent<CenterTarget>();
        Move();
        _character.StartCoroutine(Mover());
    }

    private IEnumerator Mover()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 2f));
            Move();
        }
    }

    private void Move()
    {
        _character.GetEntityComponent<Walker>().Stop();
        var pos = Vector2.zero;
        pos.x = _centerTarget.Center.x + Random.Range(-3, 3);
        pos.y = 0;
        _character.GetEntityComponent<Walker>().SetTarget(pos, Move);
    }
}
