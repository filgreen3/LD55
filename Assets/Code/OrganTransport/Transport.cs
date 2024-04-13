using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Transport : IOrganComponent, IOrganComponentUpdate
{
    private List<Connection> _connections = new();

    public Connection MakeConnection(Organ self, Organ target)
    {
        var connection = new Connection(LineHelperSystem.GetLine(), self, target);
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

    public void Update()
    {
        foreach (var connection in _connections)
        {
            connection.Update();
        }
    }

    public class Connection
    {
        public Connection(LineRenderer line, Organ self, Organ target)
        {
            _line = line;
            _self = self;
            _target = target;
        }

        public bool CheckData(Organ target) => _target == target;

        private LineRenderer _line;
        private Organ _self;
        private Organ _target;

        private List<TransportParticle> _particles = new();

        public void Update()
        {
            _line.SetPosition(0, _self.Position);
            _line.SetPosition(1, _target.Position);

            foreach (var particle in _particles)
            {
                particle.PathValue += Time.deltaTime;
                particle.Particle.position = Vector3.Lerp(_self.Position, _target.Position, particle.PathValue);
                if (particle.PathValue >= 1)
                {
                    particle.OnParticleFinishTarget?.Invoke(_target);
                }
            }
            _particles.RemoveAll(x => x.PathValue >= 1);
        }

        public void PutResource(OrganResources resources, Action<Organ> OnParticleFinishTarget)
        {
            var particleRender = ResourceParticleHelperSystem.GetParticle(resources);
            OnParticleFinishTarget += (t) => ResourceParticleHelperSystem.ReleaseParticle(particleRender);
            var particle = new TransportParticle(OnParticleFinishTarget, particleRender.transform);
            particleRender.transform.position = _self.Position;
            _particles.Add(particle);
        }

        public class TransportParticle
        {
            public Action<Organ> OnParticleFinishTarget;
            public Transform Particle;
            public float PathValue = 0;

            public TransportParticle(Action<Organ> onParticleFinishTarget, Transform particle)
            {
                OnParticleFinishTarget = onParticleFinishTarget;
                Particle = particle;
            }
        }
    }
}
