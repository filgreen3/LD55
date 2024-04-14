using UnityEngine;
using UnityEngine.Pool;

public class ResourceParticleHelperSystem : MonoBehaviour, ISystem
{
    private static ResourceParticleHelperSystem _instance;
    private ObjectPool<SpriteRenderer> _poolParticle;
    [SerializeField] private Sprite[] _sprites;

    private void Awake()
    {
        _instance = this;
        _sprites = Resources.LoadAll<Sprite>("Icon/Icons");

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
        particle.sprite = SpriteBasedOnResource(organResources);
        return particle;
    }

    public static Sprite SpriteBasedOnResource(OrganResources organResources) => _instance._sprites[(int)organResources];

    public static void ReleaseParticle(SpriteRenderer particle) => _instance._poolParticle.Release(particle);
}