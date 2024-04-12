using UnityEngine;
using Simple.Tween;
using TMPro;

namespace Simple.Tween
{
    public class TextAlphaTween : Tween<float>
    {
        [SerializeField] private TextMeshProUGUI _text;

        private float _initAlpha;
        private Color _color;

        protected override void OnStartAnimation()
        {
            _initAlpha = _text.color.a;
        }
        protected override void TweenAnimation(float t)
        {
            _color = _text.color;
            _color.a = Mathf.Lerp(_initAlpha, _target, _curve.Evaluate(t));
            _text.color = _color;
        }
    }
}