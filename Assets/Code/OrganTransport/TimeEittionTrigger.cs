using UnityEngine;

public class TimeEmitersTrigger : IOrganComponentUpdate, IOrganComponentInit
{
    [SerializeField] private float _emitRate = 2f;
    private float _emitTimer = 0f;

    private IOrganComponentResourceEmmiter[] _emiters;

    public void Init(Organ part)
    {
        _emiters = part.GetOrganComponents<IOrganComponentResourceEmmiter>();
    }

    public void Update()
    {
        if (_emiters.Length == 0 || !TownSystem.IsBattle) return;
        _emitTimer += Time.deltaTime;
        if (_emitTimer >= _emitRate)
        {
            EmitResource();
            _emitTimer = 0f;
        }
    }

    private void EmitResource()
    {
        foreach (var item in _emiters)
        {
            item.EmitResource();
        }
    }
}


