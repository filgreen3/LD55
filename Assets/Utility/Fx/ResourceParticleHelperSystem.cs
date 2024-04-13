using UnityEngine;
using UnityEngine.Pool;

public class ResourceParticleHelperSystem : MonoBehaviour, ISystem
{
    private static ResourceParticleHelperSystem _instance;
    private ObjectPool<SpriteRenderer> _poolParticle;

    private void Awake()
    {
        _instance = this;

        _poolParticle = new ObjectPool<SpriteRenderer>(
        createFunc: () =>
        {
            var particle = Instantiate(Resources.Load<SpriteRenderer>("Fx/ResourceParticle"));
            particle.transform.SetParent(transform);
            return particle;
        },
        actionOnGet: particle => particle.gameObject.SetActive(true),
        actionOnRelease: particle => particle.gameObject.SetActive(false),
        collectionCheck: false,
        defaultCapacity: 10,
        maxSize: 1000);
    }

    public static SpriteRenderer GetParticle(OrganResources organResources)
    {
        var particle = _instance._poolParticle.Get();
        particle.color = ColorBasedOnResource(organResources);
        return particle;
    }

    public static Color ColorBasedOnResource(OrganResources organResources) => organResources switch
    {
        OrganResources.Food => Color.green,
        OrganResources.Blood => Color.red,
        OrganResources.Energy => Color.yellow,
        _ => Color.white,
    };

    public static void ReleaseParticle(SpriteRenderer particle) => _instance._poolParticle.Release(particle);
}