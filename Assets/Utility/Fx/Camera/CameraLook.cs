using UnityEngine;

public class CameraLook : MonoBehaviour, ISystem
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _camera;
    [SerializeField] private Vector3 _offset;
    [SerializeField, Range(0, 1)] private float _lerp;

    private Vector3 _targetPosition;

    public float XOffest { get => _offset.x; set => _offset.x = value; }

    public Vector3 TargetPosition
    {
        get
        {
            _targetPosition = _target.position;
            return _targetPosition;
        }
    }


    public void FixedUpdate()
    {
        _camera.position = Vector3.Lerp(_camera.position, TargetPosition + _offset, 1 - _lerp);
    }
}
