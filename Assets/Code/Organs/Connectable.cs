using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Connectable : Entity<IOrganComponent>
{
    public override bool AllowDuplication => false;
    public bool CanConnect
    {
        get => _canConnect;
        set
        {
            if (_canConnect && !value)
            {
                OnConnect?.Invoke(this);
            }
            _canConnect = value;
        }
    }

    [SerializeField] public Vector2[] AttachPoints;
    [SerializeField] public Vector2[] Structure;

    [SerializeField] protected Transform _transf;
    [SerializeField] protected Collider2D _collider;
    [SerializeField] protected Rigidbody2D _rig;
    [SerializeField] private bool _canConnect = true;

    public HashSet<Joint2D> Joints = new HashSet<Joint2D>();
    public HashSet<Connectable> ConnectedParts = new HashSet<Connectable>();

    public static Action<Connectable> OnDisconnect;
    public static Action<Connectable> OnConnect;

    public Vector3 Position
    {
        get => _rig.position;
        set => _rig.position = value;
    }

    public float Rotation
    {
        get => _rig.rotation;
        set => _rig.rotation = value;
    }

    public Rigidbody2D Rig
    {
        get => _rig;
    }

    public Collider2D PartCollider
    {
        get => _collider;
    }

    public T GetOrganComponent<T>() where T : IOrganComponent
    {
        return (T)_componentsList.FirstOrDefault(t => t is T);
    }

    public T[] GetOrganComponents<T>() where T : IOrganComponent
    {
        return _componentsList.Where(t => t is T).Cast<T>().ToArray();
    }

    public virtual void Connect(Connectable target)
    {
        CanConnect = false;

        var joint = target.gameObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = Rig;
        joint.autoConfigureConnectedAnchor = false;

        GetClosetStructurePoint(target, out var strA, out var strB);

        var targetAttachPoint = target.GetClosetAnchorPoint(GetTransformedPoint(strA));

        var anchor = (targetAttachPoint + strB) * 0.5f;
        joint.anchor = anchor;

        var t = target.GetTransformedPoint(targetAttachPoint) - GetTransformedPoint(strA);
        var connectedAnchor = GetInverseTransformedPoint(target.GetTransformedPoint(anchor) - t);
        RoundVector(ref connectedAnchor);

        joint.connectedAnchor = connectedAnchor;

        //joint.breakForce = part.Rig.bodyType == RigidbodyType2D.Dynamic ? 500f : Mathf.Infinity;
        //joint.breakTorque = part.Rig.bodyType == RigidbodyType2D.Dynamic ? 500f : Mathf.Infinity;

        joint.breakForce = Mathf.Infinity;
        joint.breakTorque = Mathf.Infinity;

        joint.frequency = 10f;
        joint.enableCollision = true;

        target.Joints.Add(joint);
        target.CanConnect = false;

        Joints.Add(joint);
    }

    private void RoundVector(ref Vector3 vector)
    {
        vector.x = Mathf.Round(vector.x * 2) / 2f;
        vector.y = Mathf.Round(vector.y * 2) / 2f;
    }

    public bool ContainsCollider(Collider2D collider)
    {
        for (int i = 0; i < AttachPoints.Length; i++)
        {

            if (collider == Physics2D.OverlapCircle(GetTransformedPoint(AttachPoints[i]), 0.49f, LayerMask.GetMask("CurrentPart")))
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (var point in AttachPoints)
        {
            Gizmos.DrawWireSphere(_transf.TransformPoint(point), 0.1f);
        }
        foreach (var point in Structure)
        {
            Gizmos.DrawWireSphere(_transf.TransformPoint(point), 0.5f);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="point"></param>
    /// <returns>Return world coordinates of closest attach point to given point</returns>
    private Vector3 GetClosetAnchorPoint(Vector3 point)
    {
        var min = float.MaxValue;
        var closest = Vector3.zero;
        foreach (var anchor in AttachPoints)
        {
            var dist = (point - _transf.TransformPoint(anchor)).sqrMagnitude;
            if (dist < min)
            {
                min = dist;
                closest = anchor;
            }
        }

        return closest;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="point"></param>
    /// <returns>Transform local space to world space</returns>
    public Vector3 GetTransformedPoint(Vector3 point) => _transf.TransformPoint(point);

    public Vector3 GetInverseTransformedPoint(Vector3 point) => _transf.InverseTransformPoint(point);

    /// <summary>
    /// Return local coordinates of closest structure point to given point
    /// </summary>
    /// <param name="part"></param>
    /// <returns></returns>
    public Vector3 GetClosetStructurePoint(Connectable part)
    {
        var min = float.MaxValue;
        var pos = Vector3.zero;
        foreach (var astruct in Structure)
        {
            foreach (var bstruct in part.Structure)
            {
                var dist = (GetTransformedPoint(astruct) - part.GetTransformedPoint(bstruct)).sqrMagnitude;
                if (dist < min)
                {
                    min = dist;
                    pos = astruct;
                }
            }
        }
        return pos;
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="part"> Set part to get points</param>
    /// <param name="posA"> Return closest structure point in theier local space of self part </param>
    /// <param name="posB"> Return closest structure point in theier local space of given part </param>
    /// <param name="dist"> Return dist from to closes structure point to closest structure point of given part </param>
    public void GetClosetStructurePoint(Connectable part, out Vector3 posA, out Vector3 posB)
    {
        var min = float.MaxValue;
        var pos = Vector3.zero;
        posA = Position;
        posB = part.Position;
        foreach (var astruct in Structure)
        {
            foreach (var bstruct in part.Structure)
            {
                var dist = (GetTransformedPoint(astruct) - part.GetTransformedPoint(bstruct)).sqrMagnitude;
                if (dist < min)
                {
                    min = dist;
                    posA = astruct;
                    posB = bstruct;
                }
            }
        }
    }

    public virtual void DisconnectAll()
    {

    }

    public virtual void Disconnect(Connectable part)
    {

    }

}
