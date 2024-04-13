public class BasicInfo : OrganComponentInfo, IOrganComponentInit
{
    public override string Name => _name;
    public override string Description => "Basic Info";

    private string _name;

    public void Init(Organ part)
    {
        _name = part.gameObject.name;
    }
}