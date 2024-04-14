using UnityEngine;

public class OrganBloodShooter : BloodReciver
{
    [SerializeField] private int _damage = 1;
    public override bool CanRecive => base.CanRecive && TownSystem.IsBattle;

    public override void ReciveResource()
    {
        base.ReciveResource();
        Shoot();
    }

    private void Shoot()
    {
        ResourcesCout = 0;
        var enemy = TownEnemes.CurrentTownEnemes.CreatedCharacters[UnityEngine.Random.Range(0, TownEnemes.CurrentTownEnemes.CreatedCharacters.Count)];
        if (enemy != null)
            enemy.GetHealth().HealthPoints -= _damage;
    }
}
