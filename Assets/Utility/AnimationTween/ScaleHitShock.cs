using UnityEngine;

namespace Simple.Tween
{
    public class ScaleHitShock : ScaleShockTween
    {
        [SerializeField] float _disbalance = 1f;
        private Vector3 _scale;

        protected override void OnInit()
        {
            base.OnInit();
            _scale = _initScale;
        }

        protected override void OnStartAnimation()
        {
            _initScale = _transf.localScale;
        }

        protected override void TweenAnimation(float t)
        {
            // scale with y reduce and xz increase

            _scale = Vector3.Lerp(_initScale, _target, t);

            _scale.y = _initScale.y - _disbalance * _curve.Evaluate(t);
            _scale.x = _initScale.x + _disbalance * _curve.Evaluate(t);
            _scale.z = _initScale.z + _disbalance * _curve.Evaluate(t);

            _transf.localScale = _scale;
        }
    }
}

