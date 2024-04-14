using System;

public class BaseOrganComponent : IOrganComponentInit
{
    public static Organ BaseOrgan;
    private static Action<Organ> OnBaseOrganUpdate;

    public void Init(Organ part)
    {
        BaseOrgan = part;
        OnBaseOrganUpdate?.Invoke(part);
    }

    public static void AddAction(Action<Organ> action)
    {
        if (BaseOrgan != null)
        {
            action.Invoke(BaseOrgan);
        }
        OnBaseOrganUpdate += action;
    }
}

