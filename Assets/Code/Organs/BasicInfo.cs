using UnityEngine;

public class BasicInfo : OrganComponentInfo, IOrganComponentInit
{
    public override string Name => _name;
    public override string Description => $"<sprite=\"icon\" name=hp> {_organ.GetHealth().Value}x" + _description;


    private Organ _organ;
    private string _name;
    [TextArea] private string _description;

    public void Init(Organ part)
    {
        _organ = part;
        _name = part.gameObject.name.Replace("(Clone)", "");
    }
}