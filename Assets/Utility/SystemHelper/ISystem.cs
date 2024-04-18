
using UnityEngine;

public interface ISystem
{

}

public class InstanceSystem<T> : MonoBehaviour, ISystem where T : InstanceSystem<T>
{
    protected static T Instance;

    protected void Awake()
    {
        Instance = this as T;
    }
}