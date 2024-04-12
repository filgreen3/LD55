
public interface IEntityComponent
{

}

public interface IEntityComponentInit : IEntityComponent
{
    void Init(Character character);
}

public interface IEntityComponentUpdate : IEntityComponent
{
    void Update();
}

public interface IEntityComponentDestroy : IEntityComponent
{
    void Destroy();
}