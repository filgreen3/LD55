using System.Collections;
using UnityEngine;

public class ShakeText : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text _text;
    [SerializeField] Color _targetColor;
    [SerializeField] float _duration;
    [SerializeField] float _magnitude;
    [SerializeField] AnimationCurve _curve;

    private Coroutine _shakeCoroutine;

    public void Shake()
    {
        if (_shakeCoroutine != null) return;
        _shakeCoroutine = StartCoroutine(ShakeCoroutine(_duration, _magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        var originalPos = transform.localPosition;
        var originalColor = _text.color;
        var elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localPosition = originalPos + magnitude * Vector3.right * _curve.Evaluate(elapsed / duration);
            _text.color = Color.Lerp(originalColor, _targetColor, _curve.Evaluate(elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }
        _text.color = originalColor;
        transform.localPosition = originalPos;
        _shakeCoroutine = null;
        yield break;
    }
}
