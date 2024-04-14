using UnityEngine;
using System.Collections;

public class Walker : IEntityComponent, IEntityComponentUpdate, IEntityComponentInit
{
    [SerializeField] private float _speed;
    [SerializeField] private float _wavingSpeed;
    [SerializeField] private float _wavingAmplitude;

    public float CurrentSpeed { get => _currentSpeed; private set => _currentSpeed = Mathf.Clamp01(value); }
    public Vector2 CurrentPosition { get => _transf.position; set => _transf.position = value; }
    public Vector2 CurrentTarget => _target;
    public bool IsMoving => CurrentSpeed > 0;

    private float _currentSpeed;
    private Vector2 _target;
    private Rigidbody2D _rigidbody2d;
    private Transform _transf;
    private Character _character;
    private Coroutine _moveCoroutine;
    private System.Action _onCompletMove;
    private bool _isMoving;

    public virtual void Init(Character character)
    {
        _character = character;
        _transf = character.transform;
        _target = character.transform.position;
        _currentSpeed = 0;
        _rigidbody2d = character.GetCharacterData().Rig2D;
    }

    public virtual void Update()
    {
        var rot = Mathf.Sin(Time.timeSinceLevelLoad * _wavingSpeed) * _wavingAmplitude * CurrentSpeed;
        _character.transform.localRotation = Quaternion.AngleAxis(rot, Vector3.forward);

        if (!_isMoving) return;

        _target.y = CurrentPosition.y;
        _rigidbody2d.position = CurrentPosition + (_target - CurrentPosition).normalized * _speed * CurrentSpeed * Time.deltaTime;
        //_animator.PlayWalk();

        if ((CurrentPosition - _target).sqrMagnitude < 0.01f)
        {
            Stop();
            _onCompletMove?.Invoke();
        }
    }

    public virtual void SetTarget(Vector2 position, System.Action onCompletMove = null)
    {
        _target = position;
        _isMoving = true;
        if (_moveCoroutine != null)
            _character.StopCoroutine(_moveCoroutine);
        _moveCoroutine = _character.StartCoroutine(StartMove());
        _character.GetCharacterData().SpriteRenderer.flipX = _target.x < _character.transform.position.x;
        _onCompletMove = null;
        _onCompletMove = onCompletMove;
    }

    private IEnumerator StartMove()
    {
        var t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime;
            CurrentSpeed = Mathf.LerpUnclamped(CurrentSpeed, 1, t);
            yield return null;
        }
        yield break;
    }

    private IEnumerator StopMove()
    {
        var t = 0f;
        //_animator.PlayIdle();
        while (t <= 1f)
        {
            t += Time.deltaTime;
            CurrentSpeed = Mathf.LerpUnclamped(CurrentSpeed, 0, t);
            yield return null;
        }
        _isMoving = false;
        yield break;
    }

    public void Stop()
    {
        _isMoving = false;
        if (_moveCoroutine != null)
            _character.StopCoroutine(_moveCoroutine);
        _moveCoroutine = _character.StartCoroutine(StopMove());
        _target = _character.transform.position;
        CurrentSpeed = 0;
        CurrentPosition = _target;
    }

    public void OnCompletMoveClear() => _onCompletMove = null;
}
