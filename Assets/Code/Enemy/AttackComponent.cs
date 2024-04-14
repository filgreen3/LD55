using UnityEngine;

public class AttackComponent : IEntityComponent
{
    [SerializeField] private int _damage;
    [SerializeField] private float _waitAfterAttack;

    public int Damage => _damage;
    public float WaitAfterAttack => _waitAfterAttack;

    public void DamageOrgan(Organ organ)
    {
        organ.GetEntityComponent<OrganHealth>().Value -= _damage;
    }
}
