using UnityEngine;

namespace Simple.Tween
{
    public class SimpleMoveTweenRect : Tween<Vector3>
    {
        [SerializeField] private RectTransform _transf;
        private Vector3 _initPos;

        protected override void OnStartAnimation()
        {
            _initPos = _transf.anchoredPosition;
        }

        protected override void TweenAnimation(float t)
        {
            _transf.anchoredPosition = Vector3.LerpUnclamped(_initPos, _target, _curve.Evaluate(t));
        }

        private void Reset()
        {
            _transf = GetComponent<RectTransform>();
            _target = _transf.localPosition;
        }
    }
}
