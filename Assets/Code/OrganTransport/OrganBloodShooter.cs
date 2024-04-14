using UnityEngine;

public class OrganBloodShooter : BloodReciver, IOrganComponentInit
{
    [SerializeField] private int _damage = 1;
    public override bool CanRecive => base.CanRecive && TownSystem.IsBattle;

    private Transform _transf;

    public void Init(Organ part)
    {
        _transf = part.transform;
    }

    public override void ReciveResource()
    {
        base.ReciveResource();
        Shoot();
    }

    private void Shoot()
    {
        if (TownSystem.IsBattle == false) return;
        if (TownEnemes.CurrentTownEnemes.CreatedCharacters.Count == 0) return;
        ResourcesCout = 0;
        var enemy = TownEnemes.CurrentTownEnemes.CreatedCharacters[UnityEngine.Random.Range(0, TownEnemes.CurrentTownEnemes.CreatedCharacters.Count)];
        if (enemy != null)
            enemy.GetHealth().HealthPoints -= _damage;

        var projectile = ProjectileHelperSystem.GetProjectile();
        projectile.transform.position = _transf.position;
        projectile.SetDamage(_damage);
        projectile.SetLayer(ProjectileType.ToCharacter);
        projectile.SetColor(Color.red);
        var dir = (enemy.transform.position - _transf.position).normalized;
        projectile.Launch(dir, 15);

    }
}

