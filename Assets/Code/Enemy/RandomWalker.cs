using UnityEngine;
using System.Collections;

public class RandomWalker : IEntityComponent, IEntityComponentInit
{
    private Character _character;

    public void Init(Character character)
    {
        _character = character;
        _character.StartCoroutine(RandomWalk());
    }

    private IEnumerator RandomWalk()
    {
        while (true)
        {
            _character.GetEntityComponent<Walker>().SetTarget(new Vector2(Random.Range(-5, 5), Random.Range(-5, 5)));
            yield return new WaitForSeconds(Random.Range(5, 7));

        }
    }
}