using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework.StateMachines
{
    public class Translation
    {
        public string name;

        public string FromState { get; set; }

        public string TargetState { get; set; }

        public BDelegate<Object, String> Callback { get; set; }

        public Translation(string name, string formState, string targetState, BDelegate<Object, string> callback)
        {
            this.name = name;
            FromState = formState;
            TargetState = targetState;
            Callback = callback;
        }
    }
}
