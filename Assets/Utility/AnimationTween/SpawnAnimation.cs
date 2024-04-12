using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simple.Tween
{
    public class SpawnAnimation : ShockTween<float>
    {
        private Renderer[] _renderers;

        protected override void OnInit()
        {
            base.OnInit();
            _renderers = GetComponentsInChildren<Renderer>();
        }

        public void ForceInit()
        {
            OnInit();
        }

        protected override void OnStartAnimation()
        {
            base.OnStartAnimation();
        }

        protected override void TweenAnimation(float t)
        {
            for (int i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].material.SetFloat("_HitAmount", _curve.Evaluate(t));
            }
        }
    }
}
