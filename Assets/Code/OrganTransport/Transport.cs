using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Transport : IOrganComponent, IOrganComponentUpdate, IOrganComponentInit
{
    private List<Connection> _connections = new();

    public Connection MakeConnection(Organ self, Organ target)
    {
        var connection = new Connection(self, target);
        _connections.Add(connection);
        return connection;
    }

    public void GetConnection(Organ target)
    {
        _connections.Find(x => x.CheckData(target));
    }

    public void RemoveConnection(Organ target)
    {
        _connections.RemoveAll(x => x.CheckData(target));
    }

    public void RemoveAll()
    {
        for (int i = 0; i < _connections.Count; i++)
        {
            _connections[i].Drop();
        }
        _connections.Clear();
    }

    public void Init(Organ part)
    {
        part.OnOrganDestroyed += (t) => RemoveAll();
    }

    public void Update()
    {
        for (int i = 0; i < _connections.Count; i++)
        {
            if (_connections[i] == null || !_connections[i].NotNull)
            {
                _connections[i].Drop();
                _connections.RemoveAt(i);
                i--;
                continue;
            }
            _connections[i].Update();
        }
    }

    public class Connection
    {
        public Connection(Organ self, Organ target)
        {
            _line = LineHelperSystem.GetLine();
            _self = self;
            _target = target;
        }

        public bool CheckData(Organ target) => _target == target;

        private LineRenderer _line;
        private Organ _self;
        private Organ _target;

        public bool NotNull => _self != null && _target != null;

        private List<TransportParticle> _particles = new();

        public void Update()
        {
            _line.SetPosition(0, _self.Position);
            _line.SetPosition(1, _target.Position);

            foreach (var particle in _particles)
            {
                particle.PathValue += Time.deltaTime;
                particle.ParticleTransf.position = Vector3.Lerp(_self.Position, _target.Position, particle.PathValue);
                if (particle.PathValue >= 1)
                {
                    particle.OnParticleFinishTarget?.Invoke(_target);
                }
            }
            _particles.RemoveAll(x => x.PathValue >= 1);
        }

        public void Drop()
        {
            LineHelperSystem.ReleaseLine(_line);
            foreach (var particle in _particles)
            {
                ResourceParticleHelperSystem.ReleaseParticle(particle.Particle);
            }
        }

        public void PutResource(OrganResources resources, Action<Organ> OnParticleFinishTarget)
        {
            var particleRender = ResourceParticleHelperSystem.GetParticle(resources);
            OnParticleFinishTarget += (t) => ResourceParticleHelperSystem.ReleaseParticle(particleRender);
            var particle = new TransportParticle(OnParticleFinishTarget, particleRender);
            particleRender.transform.position = _self.Position;
            _particles.Add(particle);
        }

        public class TransportParticle
        {
            public Action<Organ> OnParticleFinishTarget;
            public Transform ParticleTransf;
            public SpriteRenderer Particle;
            public float PathValue = 0;

            public TransportParticle(Action<Organ> onParticleFinishTarget, SpriteRenderer particle)
            {
                OnParticleFinishTarget = onParticleFinishTarget;
                Particle = particle;
                ParticleTransf = particle.transform;

            }
        }
    }
}
