using UnityEngine;

public abstract class OrganComponentRotation : IOrganComponent
{
    public abstract void Rotate(Organ organ);
}

public class OrganComponentRotationFlip : OrganComponentRotation, IOrganComponentInit
{
    private Organ _organ;

    public bool SidePositive => _organ.transform.localScale.x > 0;

    public void Init(Organ part)
    {
        _organ = part;
    }

    public override void Rotate(Organ organ)
    {
        organ.transform.localScale = Vector3.right * -Mathf.Sign(organ.transform.localScale.x) + Vector3.up + Vector3.forward;
    }
}

public class OrganComponentRotationAxisZ : OrganComponentRotation
{
    public override void Rotate(Organ organ)
    {
        organ.transform.Rotate(Vector3.forward, 90);
    }
}
