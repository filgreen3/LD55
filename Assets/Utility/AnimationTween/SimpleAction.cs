using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simple.Tween
{
    public class SimpleAction : MonoBehaviour
    {
        [SerializeField] private SimpleMoveTween _tween;

        private Vector3 _pos;
        private int _turn;

        private void Start()
        {
            _pos = transform.position;
        }

        [ContextMenu("Action")]
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SimpleMoveUp();
            }
        }

        private void SimpleMoveUp()
        {

            transform.localPosition = _pos;
            _tween.AnimateTo(_tween.transform.localPosition + Vector3.up * ((_turn % 2 > 0.5f) ? 1 : -1));
            _pos = _tween.transform.localPosition + Vector3.up * ((_turn % 2 > 0.5f) ? 1 : -1);
            _turn++;

        }
    }
}
