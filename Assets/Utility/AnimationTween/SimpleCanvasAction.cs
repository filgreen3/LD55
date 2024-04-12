using UnityEngine;

namespace Simple.Tween
{
    public class SimpleCanvasAction : MonoBehaviour
    {
        [SerializeField] private CanvasTween _tween;
        private int _turn;

        [ContextMenu("Action")]

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SimpleCanvasUp();
            }
        }

        private void SimpleCanvasUp()
        {
            _tween.GetComponent<CanvasGroup>().alpha = (_turn % 2 > 0.5f) ? -1 : 1;
            _tween.AnimateTo((_turn % 2 > 0.5f) ? 1 : -1);
            _turn++;
        }
    }
}
