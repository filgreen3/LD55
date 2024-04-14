using UnityEngine;
using System.Collections;

public class Fly : IEntityComponentInit
{
    private Character _character;

    public void Init(Character character)
    {
        character.GetCharacterData().Rig2D.gravityScale = 0;
        _character = character;
        _character.StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        yield return new WaitForSeconds(0.1f);
        var max = Random.Range(1, 3f);
        var t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            var pos = _character.GetCharacterData().Rig2D.position;
            pos.y += Time.deltaTime * max;
            _character.GetCharacterData().Rig2D.position = pos;
            yield return null;
        }
    }
}
