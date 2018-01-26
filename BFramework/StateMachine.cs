using System;
using System.Collections.Generic;

namespace BFramework
{
    public class StateMachine
    { 
        public class State
        {
            public State(String name)
            {
                Name = name;
            }

            String _name;
            BDelegate<String> _action;
            private StateMachine _stateMachine;
            public string Name { get => _name; set => _name = value; }
            public BDelegate<String> Action { get => _action; set => _action = value; }
            public StateMachine StateMachine { get => _stateMachine; private set => _stateMachine = value; }

            public void Initiate(StateMachine stateMachine)
            {
                StateMachine = stateMachine;
            }
        }
        public StateMachine(params State[] states)
        {
            States = new Dictionary<string, State>();
            Tags = new List<string>();
            foreach(State s in states)
            {
                AddState(s);
            }
            Current = states[0].Name;
            Params = null;
        }
        private Dictionary<String, State> _states;
        public void AddState(State state)
        {
            States.Add(state.Name, state);
            Tags.Add(state.Name);
            state.Initiate(this);
        }
        private String _current;
        private String _nextState;
        private Object _params;
        private List<string> _tags;

        public string Current { get => _current; private set => _current = value; }
        public object Params { get => _params; set => _params = value; }
        public Dictionary<string, State> States { get => _states; private set => _states = value; }
        public List<string> Tags { get => _tags; private set => _tags = value; }

        public string GetStateByIndex(int index)
        {
            index = index >= Tags.Count ? Tags.Count - 1 : index;
            return Tags[index];
        }

        public void Run()
        {
            _nextState = States[Current].Action.method(_params);
            ChangeTo(ref _nextState);
        }

        public void ChangeTo(ref String name)
        {
            if (name != Current && States.ContainsKey(name))
            {
                Current = name;
            }
        }
    }
}
