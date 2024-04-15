using UnityEngine;
using UnityEngine.Pool;
using System;

public class LineHelperSystem : MonoBehaviour, ISystem
{
    private static LineHelperSystem _instance;
    private ObjectPool<LineRenderer> _poolLines;

    private Action<float> OnAlphaChange;


    public static void SetAlpha(float a)
    {
        _instance.OnAlphaChange?.Invoke(a);
    }

    public void SetLineAlpha(float a, LineRenderer line)
    {
        var scolor = line.startColor;
        scolor.a = a;
        line.startColor = scolor;
        var ecolor = line.endColor;
        ecolor.a = a;
        line.endColor = ecolor;
    }

    private void Awake()
    {
        _instance = this;

        _poolLines = new ObjectPool<LineRenderer>(
        createFunc: () =>
        {
            var line = Instantiate(Resources.Load<LineRenderer>("Fx/Line"));
            OnAlphaChange += a => SetLineAlpha(a, line);
            line.transform.SetParent(transform);
            return line;
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
