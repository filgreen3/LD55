using UnityEngine;

namespace Simple.Tween
{
    public class SimpleActionCustom : MonoBehaviour
    {
        [SerializeField] private Tween<float> _tween;

        private Vector3 _pos;
        private int _turn;

        private void Start()
        {
            _pos = transform.position;
        }

        [ContextMenu("Action")]
        private void SimpleMoveUp()
        {
            _tween.AnimateTo(100);
        }
    }
}
