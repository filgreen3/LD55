using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transport : IOrganComponent, IOrganComponentUpdate
{
    private List<Connection> _connections = new();

    public void MakeConnection(Organ self, Organ target)
    {
        _connections.Add(new Connection(LineHelperSystem.GetLine(), self, target));
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

        private LineRenderer _line;
        private Organ _self;
        private Organ _target;

        public void Update()
        {
            _line.SetPosition(0, _self.Position);
            _line.SetPosition(1, _target.Position);
        }
    }
}
