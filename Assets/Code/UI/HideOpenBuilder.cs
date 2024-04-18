using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOpenBuilder : InstanceSystem<HideOpenBuilder>, ISystem
{
    private const float c_maxWidth = 960;

    [SerializeField] private RectTransform _builderView;
    [SerializeField] private RectTransform _mainView;
    [SerializeField] private float _builderWidth;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _speed;


    private Coroutine _coroutine;
    private float t;

    private void Amount(float value)
    {
        Instance._mainView.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.LerpUnclamped(c_maxWidth, c_maxWidth - Instance._builderWidth, value));
        Instance._mainView.anchoredPosition = Vector2.LerpUnclamped(Vector2.zero, 0.5f * Instance._builderWidth * -Vector3.right, value);
        Instance._builderView.anchoredPosition = 0.5f * Mathf.LerpUnclamped(c_maxWidth + Instance._builderWidth - 10, c_maxWidth - Instance._builderWidth - 10, value) * Vector2.right;
    }

    private IEnumerator ShowAnimation()
    {
        while (t < 1)
        {
            t += Time.deltaTime * _speed;
            Amount(_curve.Evaluate(t));
            yield return null;
        }
        t = 1;
        Amount(t);
        _coroutine = null;
    }

    private IEnumerator HideAnimation()
    {
        while (t > 0)
        {
            t -= Time.deltaTime * _speed;
            Amount(_curve.Evaluate(t));
            yield return null;
        }
        t = 0;
        Amount(t);
        _coroutine = null;
    }

    public static void Hide()
    {
        if (Instance._coroutine != null)
            Instance.StopCoroutine(Instance._coroutine);
        Instance._coroutine = Instance.StartCoroutine(Instance.HideAnimation());
    }

    public static void Show()
    {
        if (Instance._coroutine != null)
            Instance.StopCoroutine(Instance._coroutine);
        Instance._coroutine = Instance.StartCoroutine(Instance.ShowAnimation());
    }
}
