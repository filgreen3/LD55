using UnityEngine;
using Game.Input;
using System.Collections.Generic;

namespace Game.Parts
{
    public class Builder : MonoBehaviour, ISystem
    {
        const float c_colliderDownScale = 0.65f;

        [SerializeField] private LineRenderer _line;

        public Part CurrentPart
        {
            get => _currentPart;
            set
            {
                if (value != null && value.CanConnect == false)
                {
                    return;
                }

                if (_currentPart != null)
                {
                    _currentPart.PartCollider.isTrigger = false;
                    ((BoxCollider2D)_currentPart.PartCollider).size = _colliderSize;
                    _currentPart.Rig.velocity = Vector2.zero;
                    _currentPart.Rig.angularVelocity = 0;
                    _currentPart.Rig.gravityScale = 1;
                    _currentPart.GetRender().Color = Color.white;
                    _currentPart.gameObject.layer = LayerMask.NameToLayer("Part");
                }
                _currentPart = value;
                if (_currentPart != null)
                {
                    _currentPart.PartCollider.isTrigger = true;
                    _colliderSize = ((BoxCollider2D)_currentPart.PartCollider).size;
                    ((BoxCollider2D)_currentPart.PartCollider).size *= c_colliderDownScale;
                    _currentPart.Rig.rotation = (Mathf.Round((_currentPart.Rig.rotation / 360) * 4) / 4f) * 360;
                    _additionalRotation = CurrentPart.Rotation;
                    _currentPart.Rig.velocity = Vector2.zero;
                    _currentPart.Rig.angularVelocity = 0;
                    _currentPart.Rig.gravityScale = 0;
                    _currentPart.gameObject.layer = LayerMask.NameToLayer("CurrentPart");
                    _line.enabled = _currentPart.CanConnect;
                    _targetParts = new Part[_currentPart.AttachPoints.Length];
                }
                else
                {
                    _line.enabled = false;
                }
            }
        }

        [SerializeField] private Part _currentPart;
        private Vector2 _colliderSize;
        private Part[] _targetParts;
        private Collider2D[] _overlappingColliders = new Collider2D[3];
        private ContactFilter2D _contactFilter = new ContactFilter2D();
        private int _overlappingCollidersCount;
        private float _additionalRotation = 0;


        private void Awake()
        {
            GameControl.Instance.Control.Press1.started += ctx => Point();
            GameControl.Instance.Control.Press1.canceled += ctx => Release();
            _contactFilter.useTriggers = true;

        }

        private void OnDisable()
        {
            GameControl.Instance.Control.Press1.started -= ctx => Point();
            GameControl.Instance.Control.Press1.canceled -= ctx => Release();
        }

        private void Point()
        {
            var obj = Physics2D.OverlapPoint(GetMousePosition(), LayerMask.GetMask("Part"));
            if (obj && obj.TryGetComponent<Part>(out var part))
            {
                CurrentPart = part;
            }
        }

        private void Release()
        {
            if (CurrentPart == null) return;
            if (CurrentPart.CanConnect && _overlappingCollidersCount <= 0)
            {
                var list = new List<Part>();
                for (int i = 0; i < _targetParts.Length; i++)
                {
                    if (_targetParts[i] == null || _targetParts[i].CanConnect) continue;
                    CurrentPart.Connect(_targetParts[i]);
                }
            }
            CurrentPart = null;
        }

        private void Update()
        {
            if (CurrentPart == null)
            {
                return;
            }
            CurrentPart.Position = (Vector3)GetMousePosition() + Vector3.forward * 10;

            if (GameControl.Instance.Player.Use.IsPressed())
            {
                _additionalRotation += 90;
                CurrentPart.Rotation += 90;
            }

            for (int i = 0; i < CurrentPart.AttachPoints.Length; i++)
            {
                var collider = Physics2D.OverlapPoint(CurrentPart.GetTransformedPoint(CurrentPart.AttachPoints[i]), LayerMask.GetMask("Part"));
                if (collider != null && collider.TryGetComponent<Part>(out var part) && part.ContainsCollider(CurrentPart.PartCollider))
                {
                    _targetParts[i] = part;
                }
                else
                {
                    _targetParts[i] = null;
                }
            }

            if ((_overlappingCollidersCount = CurrentPart.PartCollider.OverlapCollider(_contactFilter, _overlappingColliders)) <= 0)
            {
                CurrentPart.GetRender().Color = Color.white;
            }
            else
            {
                CurrentPart.GetRender().Color = Color.white - Color.black * 0.5f;
            }

            _line.positionCount = _overlappingCollidersCount > 0 ? 0 : _targetParts.Length * 3;
            if (_overlappingCollidersCount > 0) return;


            if (FilledSides() >= 1)
            {
                for (int i = 0; i < _targetParts.Length; i++)
                {
                    if (_targetParts[i] != null && !_targetParts[i].CanConnect)
                    {
                        CurrentPart.Rotation = _additionalRotation + (_targetParts[i].Rotation - _additionalRotation) % 90;
                        break;
                    }
                }
                CurrentPart.GetRender().Color = Color.LerpUnclamped(Color.white, Color.green, 0.5f);
            }
            else
            {
                //CurrentPart.Rotation = _additionalRotation;
                CurrentPart.GetRender().Color = Color.LerpUnclamped(Color.white, Color.green, 0f);
            }



            for (int i = 0; i < _targetParts.Length; i++)
            {
                if (_targetParts[i] != null && !_targetParts[i].CanConnect)
                {
                    CurrentPart.GetClosetStructurePoint(_targetParts[i], out var posStart, out var posEnd);

                    posStart = CurrentPart.GetTransformedPoint(posStart);
                    posEnd = _targetParts[i].GetTransformedPoint(posEnd);

                    _line.SetPosition(i * 3, posStart);
                    _line.SetPosition(i * 3 + 1, posEnd);
                    _line.SetPosition(i * 3 + 2, posStart);
                }
                else
                {
                    _line.SetPosition(i * 3, CurrentPart.Position);
                    _line.SetPosition(i * 3 + 1, CurrentPart.Position);
                    _line.SetPosition(i * 3 + 2, CurrentPart.Position);
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
}
