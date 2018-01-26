using System;
using System.Collections.Generic;

namespace BFramework
{
    public class StatusMachine
    { 
        public class Status
        {
            public Status(String name)
            {
                Name = name;
            }

            String _name;
            BDelegate<String> _action;
            private StatusMachine _statusMachine;
            public string Name { get => _name; set => _name = value; }
            public BDelegate<String> Action { get => _action; set => _action = value; }
            public StatusMachine StatusMachine { get => _statusMachine; private set => _statusMachine = value; }

            public void Initiate(StatusMachine machine)
            {
                StatusMachine = machine;
            }
        }
        public StatusMachine(params Status[] statuses)
        {
            Statuses = new Dictionary<string, Status>();
            Tags = new List<string>();
            foreach(Status s in statuses)
            {
                AddStatus(s);
            }
            Current = statuses[0].Name;
            Params = null;
        }
        private Dictionary<String, Status> _statuses;
        public void AddStatus(Status status)
        {
            Statuses.Add(status.Name, status);
            Tags.Add(status.Name);
            status.Initiate(this);
        }
        private String _current;
        private String _nextStatus;
        private Object _params;
        private List<string> _tags;

        public string Current { get => _current; private set => _current = value; }
        public object Params { get => _params; set => _params = value; }
        public Dictionary<string, Status> Statuses { get => _statuses; private set => _statuses = value; }
        public List<string> Tags { get => _tags; private set => _tags = value; }

        public void Run()
        {
            _nextStatus = Statuses[Current].Action.method(_params);
            ChangeTo(ref _nextStatus);
        }

        public void ChangeTo(ref String name)
        {
            if (name != Current && Statuses.ContainsKey(name))
            {
                Current = name;
            }
        }
    }
}
