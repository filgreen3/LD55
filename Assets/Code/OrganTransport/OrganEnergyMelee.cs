using UnityEngine;
using System.Collections;

public class OrganEnergyMelee : EnergyReciver, IOrganComponentInit
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private GameObject _fx;
    public override bool CanRecive => TownSystem.IsBattle;

    private Transform _transf;
    private Coroutine _hitCoroutine;
    private Organ _organ;

    public void Init(Organ part)
    {
        _organ = part;
        _transf = part.transform;
    }

    public override void ReciveResource()
    {
        base.ReciveResource();
        Hit();
    }

    private void Hit()
    {
        var pos = _transf.position + _transf.right * _transf.localScale.x;
        var list = Physics2D.OverlapCircleAll(pos, 1f, LayerMask.GetMask("Character"));

        if (_hitCoroutine == null)
            _hitCoroutine = _organ.StartCoroutine(Animate());

        foreach (var item in list)
        {
            if (item.TryGetComponent(out Character character))
            {
                character.GetHealth().HealthPoints -= _damage;
            }
        }
    }

    private IEnumerator Animate()
    {
        _fx.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _fx.SetActive(false);
        _hitCoroutine = null;
    }
}

