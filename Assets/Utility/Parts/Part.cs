using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;


namespace Game.Parts
{
    public class Part : Connectable, IPointerDownHandler, IPointerUpHandler
    {
        private readonly HashSet<IPartComponentConnect> _connectComponents = new();
        private readonly HashSet<IPartComponentUpdate> _updateComponents = new();

        public string Display => name;
        public Action AfterInit;

        public static Action<Part> OnPartDown;
        public static Action<Part> OnPartUp;

        protected override void Awake()
        {
            base.Awake();
            foreach (var component in Components)
            {
                if (component is IPartComponentInit initable)
                    initable.Init(this);
                if (component is IPartComponentUpdate updateable)
                    _updateComponents.Add(updateable);
                if (component is IPartComponentConnect connectable)
                    _connectComponents.Add(connectable);
            }

            AfterInit?.Invoke();
            AfterInit = null;
        }

        private void Start()
        {
            Joints = GetComponents<Joint2D>().ToHashSet();
        }

        public override T AddEntityComponent<T>(T component)
        {
            var result = base.AddEntityComponent(component);
            if (Init)
            {
                (component as IPartComponentInit)?.Init(this);
            }
            if (component is IPartComponentUpdate updateComp)
                _updateComponents.Add(updateComp);
            return result;
        }

        public override void Connect(Connectable target)
        {
            base.Connect(target);
            foreach (var comp in _connectComponents)
            {
                comp.OnConnect(this, target as Part);
            }
        }

        public override void DisconnectAll()
        {
            foreach (var comp in _connectComponents)
            {
                foreach (var connectable in ConnectedParts)
                {
                    comp.OnDisconnect(this, connectable as Part);
                }
            }
            foreach (var part in ConnectedParts)
            {
                part.Disconnect(this);
            }
            foreach (var joint in Joints)
            {
                Destroy(joint);
            }
            Joints.Clear();
            ConnectedParts.Clear();
            PartCollider.enabled = false;
            OnDisconnect?.Invoke(this);
        }

        private void FixedUpdate()
        {
            foreach (var comp in _updateComponents)
            {
                comp.Update();
            }
        }

        public override void Disconnect(Connectable conected)
        {
            base.Disconnect(conected);

            foreach (var comp in _connectComponents)
            {
                comp.OnDisconnect(this, conected as Part);
            }
            ConnectedParts.Remove(conected);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (CanConnect && other.relativeVelocity.sqrMagnitude > 1f)
            {

            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPartUp?.Invoke(this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPartDown?.Invoke(this);
        }
    }

    public static class PartExtensions
    {
        public static PartRender GetRender(this Part part) => part.GetEntityComponent<PartRender>();
    }
}