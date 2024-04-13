using UnityEngine;
using Game.Input;
using System.Collections.Generic;

public class OrganBuilder : MonoBehaviour, ISystem
{
    const float c_colliderDownScale = 0.65f;

    [SerializeField] private LineRenderer _line;
    private static OrganBuilder _instance;


    public Organ CurrentOrgan
    {
        get => _currentOrgan;
        set
        {
            if (value != null && value.CanConnect == false)
            {
                return;
            }

            if (_currentOrgan != null)
            {
                _currentOrgan.PartCollider.isTrigger = false;
                ((BoxCollider2D)_currentOrgan.PartCollider).size = _colliderSize;
                _currentOrgan.Rig.velocity = Vector2.zero;
                _currentOrgan.Rig.angularVelocity = 0;
                _currentOrgan.Rig.gravityScale = 1;
                _currentOrgan.GetRender().Color = Color.white;
                _currentOrgan.gameObject.layer = LayerMask.NameToLayer("Part");
            }
            _currentOrgan = value;
            if (_currentOrgan != null)
            {
                _currentOrgan.PartCollider.isTrigger = true;
                _colliderSize = ((BoxCollider2D)_currentOrgan.PartCollider).size;
                ((BoxCollider2D)_currentOrgan.PartCollider).size *= c_colliderDownScale;
                _currentOrgan.Rig.rotation = (Mathf.Round((_currentOrgan.Rig.rotation / 360) * 4) / 4f) * 360;
                _additionalRotation = CurrentOrgan.Rotation;
                _currentOrgan.Rig.velocity = Vector2.zero;
                _currentOrgan.Rig.angularVelocity = 0;
                _currentOrgan.Rig.gravityScale = 0;
                _currentOrgan.gameObject.layer = LayerMask.NameToLayer("CurrentPart");
                _line.enabled = _currentOrgan.CanConnect;
                _targetParts = new Organ[_currentOrgan.AttachPoints.Length];
            }
            else
            {
                _line.enabled = false;
            }
        }
    }

    private Organ _currentOrgan;
    private Vector2 _colliderSize;
    private Organ[] _targetParts;
    private Collider2D[] _overlappingColliders = new Collider2D[3];
    private ContactFilter2D _contactFilter = new ContactFilter2D();
    private int _overlappingCollidersCount;
    private float _additionalRotation = 0;


    public static void CallToBuild(Organ organ)
    {
        _instance.CurrentOrgan = organ;
    }

    private void Awake()
    {
        GameControl.Instance.Control.Press1.started += ctx => Point();
        GameControl.Instance.Control.Press1.canceled += ctx => Release();
        GameControl.Instance.Control.Rotate.performed += ctx => Rotate();
        _contactFilter.useTriggers = true;
        _instance = this;
    }

    private void OnDisable()
    {
        GameControl.Instance.Control.Press1.started -= ctx => Point();
        GameControl.Instance.Control.Press1.canceled -= ctx => Release();
        GameControl.Instance.Control.Rotate.performed -= ctx => Rotate();
    }

    private void Point()
    {
        var obj = Physics2D.OverlapPoint(GetMousePosition(), LayerMask.GetMask("Part"));
        if (obj && obj.TryGetComponent<Organ>(out var part))
        {
            CurrentOrgan = part;
        }
    }

    private void Rotate()
    {
        if (CurrentOrgan == null) return;
        if (CurrentOrgan.HasEntityComponent(typeof(OrganComponentRotation)))
        {
            if (CurrentOrgan.HasEntityComponent(typeof(OrganComponentRotationAxisZ)))
            {
                _additionalRotation += 90;
            }
            CurrentOrgan.GetEntityComponent<OrganComponentRotation>().Rotate(CurrentOrgan);
        }
    }

    private void Release()
    {
        if (CurrentOrgan == null) return;
        var connected = false;
        if (CurrentOrgan.CanConnect && _overlappingCollidersCount <= 0)
        {
            var list = new List<Organ>();
            for (int i = 0; i < _targetParts.Length; i++)
            {
                if (_targetParts[i] == null || _targetParts[i].CanConnect) continue;
                CurrentOrgan.Connect(_targetParts[i]);
                connected = true;
            }
        }
        if (!connected) Destroy(CurrentOrgan.gameObject);
        CurrentOrgan = null;
    }

    private void Update()
    {
        if (CurrentOrgan == null)
        {
            return;
        }
        CurrentOrgan.Position = (Vector3)GetMousePosition() + Vector3.forward * 10;


        for (int i = 0; i < CurrentOrgan.AttachPoints.Length; i++)
        {
            var collider = Physics2D.OverlapPoint(CurrentOrgan.GetTransformedPoint(CurrentOrgan.AttachPoints[i]), LayerMask.GetMask("Part"));
            if (collider != null && collider.TryGetComponent<Organ>(out var part) && part.ContainsCollider(CurrentOrgan.PartCollider))
            {
                _targetParts[i] = part;
            }
            else
            {
                _targetParts[i] = null;
            }
        }

        if ((_overlappingCollidersCount = CurrentOrgan.PartCollider.OverlapCollider(_contactFilter, _overlappingColliders)) <= 0)
        {
            CurrentOrgan.GetRender().Color = Color.white;
        }
        else
        {
            CurrentOrgan.GetRender().Color = Color.white - Color.black * 0.5f;
        }

        _line.positionCount = _overlappingCollidersCount > 0 ? 0 : _targetParts.Length * 3;
        if (_overlappingCollidersCount > 0) return;


        if (FilledSides() >= 1)
        {
            for (int i = 0; i < _targetParts.Length; i++)
            {
                if (_targetParts[i] != null && !_targetParts[i].CanConnect)
                {
                    CurrentOrgan.Rotation = _additionalRotation + (_targetParts[i].Rotation - _additionalRotation) % 90;
                    break;
                }
            }
            CurrentOrgan.GetRender().Color = Color.LerpUnclamped(Color.white, Color.green, 0.5f);
        }
        else
        {
            //CurrentPart.Rotation = _additionalRotation;
            CurrentOrgan.GetRender().Color = Color.LerpUnclamped(Color.white, Color.green, 0f);
        }



        for (int i = 0; i < _targetParts.Length; i++)
        {
            if (_targetParts[i] != null && !_targetParts[i].CanConnect)
            {
                CurrentOrgan.GetClosetStructurePoint(_targetParts[i], out var posStart, out var posEnd);

                posStart = CurrentOrgan.GetTransformedPoint(posStart);
                posEnd = _targetParts[i].GetTransformedPoint(posEnd);

                _line.SetPosition(i * 3, posStart);
                _line.SetPosition(i * 3 + 1, posEnd);
                _line.SetPosition(i * 3 + 2, posStart);
            }
            else
            {
                _line.SetPosition(i * 3, CurrentOrgan.Position);
                _line.SetPosition(i * 3 + 1, CurrentOrgan.Position);
                _line.SetPosition(i * 3 + 2, CurrentOrgan.Position);
            }
        }
    }

    private int FilledSides()
    {
        int sides = 0;
        for (int i = 0; i < _targetParts.Length; i++)
        {
            if (_targetParts[i] != null && !_targetParts[i].CanConnect)
            {
                sides++;
            }
        }
        return sides;
    }

    private Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(GameControl.Instance.Control.MouseLook.ReadValue<Vector2>());
    }
}
