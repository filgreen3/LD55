using System.Collections;
using UnityEngine;

public class ShakeText : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text _text;
    [SerializeField] Color _targetColor;
    [SerializeField] float _duration;
    [SerializeField] float _magnitude;
    [SerializeField] AnimationCurve _curve;

    public void Shake()
    {
        StartCoroutine(ShakeCoroutine(_duration, _magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        var originalPos = transform.localPosition;
        var elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localPosition = originalPos + magnitude * Vector3.right * _curve.Evaluate(elapsed / duration);
            _text.color = Color.Lerp(_text.color, _targetColor, _curve.Evaluate(elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
