using UnityEngine;
using System.Collections;

public class SwordmanWalkerTargeter : IEntityComponentInit
{
    private Vector2 _pos;
    private Character _character;
    private bool _doneMoving;

    private Vector2 CharacterPosition => _character.GetCharacterData().Rig2D.position;
    private Vector2 MoveDirection => (_pos - CharacterPosition).normalized;

    private Collider2D _collider;

    public void Init(Character character)
    {
        _character = character;
        _character.StartCoroutine(Behaviour());
    }

    private IEnumerator Behaviour()
    {
        yield return new WaitForSeconds(2f);
        _pos = _character.GetEntityComponent<CenterTarget>().Center;
        Move();

        while (true)
        {
            while (!CheckOrgan())
            {
                yield return new WaitForSeconds(0.4f);
            }
            if (CheckOrgan(out Organ organ))
            {
                _character.GetEntityComponent<AttackComponent>().DamageOrgan(organ);
                AudioHelper.PlayClip(ClipStorage.Instance._hit, 0.1f);
                yield return new WaitForSeconds(3f);
            }
        }
    }

    private bool CheckOrgan()
    {
        _collider = Physics2D.OverlapCircle(CharacterPosition + MoveDirection * 0.5f, 0.6f, LayerMask.GetMask("Part"));
        if (_collider == null) { return false; }
        return _collider.GetComponent<Organ>();
    }

    private bool CheckOrgan(out Organ organ)
    {
        _collider = Physics2D.OverlapCircle(CharacterPosition + MoveDirection * 0.5f, 0.6f, LayerMask.GetMask("Part"));
        if (_collider == null) { organ = null; return false; }
        return _collider.TryGetComponent<Organ>(out organ);
    }

    private void Move()
    {
        _doneMoving = false;
        _character.GetWalker().SetTarget(_pos, Move);
    }
}
