using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simple.Tween
{
    public class CanvasTween : Tween<float>
    {
        [SerializeField] private CanvasGroup _canvas;

        private float _initPos;

        protected override void OnStartAnimation()
        {
            _initPos = _canvas.alpha;
        }

        protected override void TweenAnimation(float t)
        {
            _canvas.alpha = Mathf.Lerp(_initPos, _target, _curve.Evaluate(t));
            _canvas.interactable = _canvas.alpha > 0.99f;
            _canvas.blocksRaycasts = _canvas.interactable;
        }
    }
}
