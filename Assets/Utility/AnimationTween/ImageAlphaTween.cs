using UnityEngine;
using UnityEngine.UI;

namespace Simple.Tween
{
    public class ImageAlphaTween : Tween<float>
    {
        [SerializeField] private Image _image;

        private float _initAlpha;
        private Color _color;

        public void SetAlpha(float alpha)
        {
            _color = _image.color;
            _color.a = alpha;
            _image.color = _color;
        }

        protected override void OnStartAnimation()
        {
            _initAlpha = _image.color.a;
        }
        protected override void TweenAnimation(float t)
        {
            _color = _image.color;
            _color.a = Mathf.Lerp(_initAlpha, _target, _curve.Evaluate(t));
            _image.color = _color;
        }
    }
}
