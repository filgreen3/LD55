using UnityEngine;

public class BasicInfo : OrganComponentInfo, IOrganComponentInit
{
    public override string Name => _name;
    public override string Description => _organ ? $"{_organ.GetHealth().Value}<sprite=\"icon\" name=hp>\n" + _description : _description;

    [SerializeField] private Organ _organ;

    [SerializeField] private string _name;
    [SerializeField, TextArea] private string _description;

    public void Init(Organ part)
    {
        _organ = part;
    }
}