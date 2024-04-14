public class BrainWin : IOrganComponentConnect
{
    public void OnConnect(Organ parent, Organ target)
    {
        WinSystem.ReadyToWin = true;
    }

    public void OnDisconnect(Organ parent, Organ target)
    {

    }
}

public class KingDeadWin : IEntityComponentInit
{
    public void Init(Character character)
    {
        character.GetHealth().OnDeath += () =>
        {
            WinSystem.Win();
        };
    }
}
