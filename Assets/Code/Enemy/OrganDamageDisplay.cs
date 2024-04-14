using System.Collections;
using UnityEngine;

public class OrganDamageDisplay : IOrganComponent, IOrganComponentInit
{
    private OrganHealth _healthComp;
    private Organ _organ;
    private OrganRender _organRender;
    private Coroutine _flashCoroutine;

    public void Init(Organ organ)
    {
        _healthComp = organ.GetHealth();
        _organ = organ;
        _organ.GetHealth().OnDamage += (t) => CameraShake.Shake(0.1f, 3, 0.5f);
        _organRender = organ.GetRender();
        _healthComp.OnDamage += (t) => DamageHandle(1);
    }

    private void DamageHandle(int damage)
    {
        if (_flashCoroutine != null)
            _organ.StopCoroutine(_flashCoroutine);
        _flashCoroutine = _organ.StartCoroutine(DamageFlash());
    }


    private IEnumerator DamageFlash()
    {
        yield return null;
        _organRender.Color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _organRender.Color = Color.white;
    }
}




public class OrganDamageDisplayShake : IOrganComponent, IOrganComponentInit
{
    public void Init(Organ organ)
    {
    }
}