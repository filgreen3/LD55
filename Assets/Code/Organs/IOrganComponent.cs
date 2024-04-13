public interface IOrganComponent { }

public interface IOrganComponentInit : IOrganComponent
{
    public abstract void Init(Organ part);
}

public interface IOrganComponentConnect : IOrganComponent
{
    public abstract void OnConnect(Organ parent, Organ target);
    public abstract void OnDisconnect(Organ parent, Organ target);
}

public interface IOrganComponentUpdate : IOrganComponent
{
    public abstract void Update();
}
