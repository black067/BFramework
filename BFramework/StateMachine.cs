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
            public string Name { get => _name; set => _name = value; }
            public BDelegate<String> Action { get => _action; set => _action = value; }
        }
        
        public Dictionary<String, State> states;
        public String current;
        private String _nextState;
        public void Run()
        {
            _nextState = states[current].Action.method();
            ChangeTo(_nextState);
        }

        public void ChangeTo(String name)
        {
            if (_nextState != current && states.ContainsKey(_nextState))
            {
                current = _nextState;
            }
        }
    }
}
