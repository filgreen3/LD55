using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simple.Tween
{
    public class WavevingAnimation : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Vector3 _waveSize;
        [SerializeField] private SimpleMoveTween _tween;

        private Transform _transf;

        private IEnumerator Start()
        {
            _transf = transform;
            var waiter = new WaitForSeconds(1f / _speed);
            while (true)
            {
                _tween.AnimateTo(_waveSize * 0.5f, _speed);
                yield return waiter;
                _tween.AnimateTo(-_waveSize * 0.5f, _speed);
                yield return waiter;
            }
        }
    }
}