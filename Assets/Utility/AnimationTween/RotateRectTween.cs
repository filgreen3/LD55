using UnityEngine;

namespace Simple.Tween
{
    public class RotateRectTween : Tween<float>
    {
        [SerializeField] private RectTransform _rect;

        protected override void TweenAnimation(float t)
        {
            _rect.localRotation = Quaternion.AngleAxis(_target * _curve.Evaluate(t), Vector3.forward);
        }
        protected override void OnStartAnimation()
        {
            base.OnStartAnimation();
        }
    }

}
