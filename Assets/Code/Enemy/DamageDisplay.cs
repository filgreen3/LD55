using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDisplay : IEntityComponent, IEntityComponentInit
{
    private Health _healthComp;
    private CharacterData _dataComp;
    private Character _character;
    private Coroutine _flashCoroutine;

    public void Init(Character character)
    {
        _healthComp = character.GetHealth();
        _dataComp = character.GetCharacterData();
        _character = character;

        _healthComp.OnDamage += DamageHandle;
    }

    private void DamageHandle(int damage)
    {
        if (_flashCoroutine != null)
            _character.StopCoroutine(_flashCoroutine);
        _flashCoroutine = _character.StartCoroutine(DamageFlash());
    }


    private IEnumerator DamageFlash()
    {
        yield return null;
        _dataComp.SpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _dataComp.SpriteRenderer.color = Color.white;
    }
}
