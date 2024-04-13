using UnityEngine;
using UnityEngine.Pool;

public class LineHelperSystem : MonoBehaviour, ISystem
{
    private static LineHelperSystem _instance;
    private ObjectPool<LineRenderer> _poolLines;

    private void Awake()
    {
        _instance = this;

        _poolLines = new ObjectPool<LineRenderer>(
        createFunc: () =>
        {
            var noise = Instantiate(Resources.Load<LineRenderer>("Fx/Line"));
            noise.transform.SetParent(transform);
            return noise;
        },
        actionOnGet: line => line.gameObject.SetActive(true),
        actionOnRelease: line => line.gameObject.SetActive(false),
        collectionCheck: false,
        defaultCapacity: 10,
        maxSize: 1000);
    }

    public static LineRenderer GetLine() => _instance._poolLines.Get();
    public static void ReleaseLine(LineRenderer line) => _instance._poolLines.Release(line);
}
