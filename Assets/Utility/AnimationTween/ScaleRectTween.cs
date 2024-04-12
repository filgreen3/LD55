using UnityEngine;

namespace Simple.Tween
{
    public class ScaleRectTween : Tween<Vector3>
    {
        [SerializeField] private RectTransform _rect;

        private Vector3 _initScale;


        protected override void OnStartAnimation()
        {
            _initScale = _rect.localScale;
        }
        protected override void TweenAnimation(float t)
        {
            _rect.localScale = Vector3.Lerp(_initScale, _target, _curve.Evaluate(t));
        }
    }
}
