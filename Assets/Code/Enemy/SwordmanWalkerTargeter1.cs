using UnityEngine;
using System.Collections;

public class ArcherWalkerTargeter : IEntityComponentInit
{
    [SerializeField] private float _distance = 10;

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
        yield return new WaitForSeconds(0.2f);
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
                yield return new WaitForSeconds(_character.GetEntityComponent<AttackComponent>().WaitAfterAttack);
            }
        }
    }

    private bool CheckOrgan()
    {
        var hit = Physics2D.Raycast(CharacterPosition, MoveDirection, _distance, LayerMask.GetMask("Part"));
        if (!hit) return false;
        _collider = hit.collider;
        if (_collider == null) { return false; }
        return _collider.GetComponent<Organ>();
    }
    private bool CheckOrgan(out Organ organ)
    {
        organ = null;
        var hit = Physics2D.Raycast(CharacterPosition, MoveDirection, _distance, LayerMask.GetMask("Part"));
        if (!hit) return false;
        _collider = hit.collider;
        if (_collider == null) return false;
        return _collider.TryGetComponent<Organ>(out organ);
    }

    private void Move()
    {
        _doneMoving = false;
        _character.GetWalker().SetTarget(_pos, Move);
    }
}
