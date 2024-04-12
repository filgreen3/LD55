using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simple.Tween
{
    public class SimpleMoveTween : Tween<Vector3>
    {
        [SerializeField] private Transform _transf;
        private Vector3 _initPos;

        protected override void OnStartAnimation()
        {
            _initPos = _transf.localPosition;
        }

        protected override void TweenAnimation(float t)
        {
            _transf.localPosition = Vector3.LerpUnclamped(_initPos, _target, _curve.Evaluate(t));
        }

        private void Reset()
        {
            _transf = transform;
            _target = _transf.localPosition;
        }
    }
}
