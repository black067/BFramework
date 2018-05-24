using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework.StateMachines
{

    /// <summary>
    /// 状态转换类
    /// </summary>
    public class Translation
    {
        public string Name
        {
            get;
            private set;
        }

        public string FromState { get; set; }

        public string TargetState { get; set; }

        public BDelegate<object, string> CallbackAction { get; set; }

        public BDelegate<object, bool> Condition
        {
            get; set;
        }
        
        public Translation(string fromState, string targetState, BDelegate<object, bool> condition, BDelegate<object, string> callback)
        {
            Name = fromState + "TO" + targetState;
            FromState = fromState;
            TargetState = targetState;
            Condition = condition;
            CallbackAction = callback;
        }

        public bool Determine(object input)
        {
            if (Condition == null)
            {
                return false;
            }
            return Condition.Execute(input);
        }

        public string Callback(object input)
        {
            if (CallbackAction == null)
            {
                return TargetState;
            }
            return CallbackAction.Execute(input);
        }
    }
}
