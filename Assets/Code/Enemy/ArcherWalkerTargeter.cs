using UnityEngine;
using System.Collections;

public class ArcherWalkerTargeter : IEntityComponentInit
{
    [SerializeField] private float _distance = 10;
    [SerializeField] private float _shootDelay;
    [SerializeField] private int _damage;

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

        while (true)
        {
            yield return new WaitForSeconds(_shootDelay + Random.value);
            if (OrganBuilder.ConnectedOrgans.Count > 0)
            {
                var target = OrganBuilder.ConnectedOrgans[Random.Range(0, OrganBuilder.ConnectedOrgans.Count)];
                Shoot(target);
            }
        }
    }

    private void Shoot(Organ target)
    {
        _doneMoving = true;
        var projectile = ProjectileHelperSystem.GetProjectile();
        projectile.transform.position = _character.GetCharacterData().Rig2D.position;
        projectile.SetDamage(_damage);
        projectile.SetLayer(ProjectileType.ToOrgan);
        projectile.SetColor(Color.magenta);
        var dir = (target.Rig.position - _character.GetCharacterData().Rig2D.position).normalized;
        projectile.Launch(dir, 15);

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
