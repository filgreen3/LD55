using UnityEngine;

public abstract class OrganComponentRotation : IOrganComponent
{
    public abstract void Rotate(Organ organ);
}

public class OrganComponentRotationFlip : OrganComponentRotation
{
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
