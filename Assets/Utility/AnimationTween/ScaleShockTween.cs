using UnityEngine;

namespace Simple.Tween
{
    public class ScaleShockTween : ShockTween<Vector3>
    {
        protected Transform _transf;
        protected Vector3 _initScale;

        public void ForceInit()
        {
            OnInit();
        }

        protected override void OnInit()
        {
            base.OnInit();
            _transf = transform;
            _initScale = _transf.localScale;
        }

        protected override void OnStartAnimation()
        {
            base.OnStartAnimation();
        }

        protected override void TweenAnimation(float t)
        {
            _transf.localScale = _initScale + Vector3.one * _curve.Evaluate(t) * _power;
        }

        public override void Stop()
        {
            base.Stop();
            _transf.localScale = _initScale;
        }
    }
}

