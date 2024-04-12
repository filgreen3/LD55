using System.Collections;
using UnityEngine;

namespace Simple.Tween
{
    public abstract class Tween<T> : MonoBehaviour
    {
        [SerializeField] protected AnimationCurve _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] protected float _speed = 1f;

        public AnimationCurve Curve { get => _curve; set => _curve = value; }

        protected float _mainSpeed;
        protected T _target;

        private Coroutine _coroutine;

        protected void Start()
        {
            OnInit();
        }

        public virtual void Stop()
        {
            if (_coroutine != null && this.isActiveAndEnabled)
            {
                StopCoroutine(_coroutine);
            }
        }

        public void AnimateTo(T target, float speed = -1f)
        {
            if (speed > 0f)
            {
                _mainSpeed = Time.fixedDeltaTime * speed;
            }
            else
            {
                _mainSpeed = Time.fixedDeltaTime * _speed;
            }

            _target = target;
            Stop();
            OnStartAnimation();
            _coroutine = StartCoroutine(TweenProcess());
        }


        protected virtual void OnInit()
        {

        }

        protected virtual void OnStartAnimation()
        {

        }

        private IEnumerator TweenProcess()
        {
            var t = 0f;
            var waiter = new WaitForFixedUpdate();

            while (t < 1f)
            {
                TweenAnimation(t);
                t += _mainSpeed;
                yield return waiter;
            }
            TweenAnimation(1f);
        }

        protected abstract void TweenAnimation(float t);
    }
}
