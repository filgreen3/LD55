using UnityEngine;

namespace Simple.Tween
{
    public class TimeScaleTween : Tween<float>
    {
        private float _initScale;

        protected override void OnStartAnimation()
        {
            _initScale = Time.timeScale;
        }
        protected override void TweenAnimation(float t)
        {
            Time.timeScale = Mathf.Lerp(_initScale, _target, _curve.Evaluate(t));
            Time.fixedDeltaTime = 0.01f * Mathf.Lerp(_initScale, _target, _curve.Evaluate(t));
        }
        public override void Stop()
        {
            base.Stop();
            TweenAnimation(_target);
        }
    }
}
