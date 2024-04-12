using UnityEngine;

namespace Simple.Tween
{
    public class RotateTween : Tween<float>
    {
        private Transform _rect;

        protected override void TweenAnimation(float t)
        {
            _rect.localRotation = Quaternion.AngleAxis(_target * _curve.Evaluate(t), Vector3.forward);
        }
        protected override void OnStartAnimation()
        {
            _rect ??= transform;
            base.OnStartAnimation();
        }
    }

}
