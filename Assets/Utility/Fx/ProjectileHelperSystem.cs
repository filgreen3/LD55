using UnityEngine;
using UnityEngine.Pool;

public class ProjectileHelperSystem : MonoBehaviour, ISystem
{
    private static ProjectileHelperSystem _instance;
    private ObjectPool<Projectile> _poolProjectile;

    private void Awake()
    {
        _instance = this;

        _poolProjectile = new ObjectPool<Projectile>(
        createFunc: () =>
        {
            var projectile = Instantiate(Resources.Load<Projectile>("Fx/Projectile"));
            projectile.transform.SetParent(transform);
            return projectile;
        },
        actionOnGet: projectile => projectile.gameObject.SetActive(true),
        actionOnRelease: projectile => projectile.gameObject.SetActive(false),
        collectionCheck: false,
        defaultCapacity: 10,
        maxSize: 1000);
    }

    public static Projectile GetProjectile()
    {
        var projectile = _instance._poolProjectile.Get();
        return projectile;
    }


    public static void ReleaseProjectile(Projectile projectile) => _instance._poolProjectile.Release(projectile);
}
