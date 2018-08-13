using System.Collections.Generic;

namespace BFramework.StateMachines
{
    /// <summary>
    /// 用于状态机的状态类
    /// </summary>
    public class State
    {
        /// <summary>
        /// 使用字符串为名称新建一个状态
        /// </summary>
        /// <param name="name"></param>
        public State(string name) : this(name, new BDelegate<object, string>(delegate (object o) { return name; })) { }


        /// <summary>
        /// 使用给定的名称与现有的方法新建状态
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public State(string name, BDelegate<object, string>.Method action) : this(name, new BDelegate<object, string>(action)) { }

        /// <summary>
        /// 使用字符串为名称，已有的委托新建一个状态
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public State(string name, BDelegate<object, string> action)
        {
            Name = name;
            Action = action;
            _translations = new Dictionary<string, Translation>();
        }
        
        private BDelegate<object, string> _action;
        /// <summary>
        /// 状态节点的行为
        /// </summary>
        public BDelegate<object, string> Action
        {
            get
            {
                return _action;
            }
            set
            {
                _action = value ?? new BDelegate<object, string>(() => Name);
            }
        }

        private Dictionary<string, Translation> _translations { get; set; }

        /// <summary>
        /// 状态名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 状态节点对状态机的指针
        /// </summary>
        public StateMachine StateMachine { get; private set; }
        /// <summary>
        /// 使用给定的状态机进行初始化
        /// </summary>
        /// <param name="machine"></param>
        public void Initiate(StateMachine machine)
        {
            StateMachine = machine;
        }

        /// <summary>
        /// 执行该状态的行为
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Act(object input)
        {
            foreach (Translation t in _translations.Values)
            {
                if (t.Determine(input))
                {
                    return t.Callback(input);
                }
            }
            return Action.Execute(input);
        }

        /// <summary>
        /// 条件有参数, 回调有参数
        /// </summary>
        /// <param name="targetState"></param>
        /// <param name="conditionMethod"></param>
        /// <param name="callbackMethod"></param>
        /// <returns></returns>
        public Translation AddTranslation(string targetState, BDelegate<object, bool>.Method conditionMethod, BDelegate<object, string>.Method callbackMethod)
        {
            callbackMethod = callbackMethod ?? (i => targetState);
            return AddTranslation(targetState, new BDelegate<object, bool>(conditionMethod), new BDelegate<object, string>(callbackMethod));
        }

        /// <summary>
        /// 条件无参数, 回调无参数
        /// </summary>
        /// <param name="targetState"></param>
        /// <param name="conditionMethod"></param>
        /// <param name="callbackMethod"></param>
        /// <returns></returns>
        public Translation AddTranslation(string targetState, BDelegate<object, bool>.MethodNone conditionMethod, BDelegate<object, string>.MethodNone callbackMethod)
        {
            callbackMethod = callbackMethod ?? (() => targetState);
            return AddTranslation(targetState, new BDelegate<object, bool>(conditionMethod), new BDelegate<object, string>(callbackMethod));
        }

        /// <summary>
        /// 条件无参数, 回调有参数
        /// </summary>
        /// <param name="targetState"></param>
        /// <param name="conditionMethod"></param>
        /// <param name="callbackMethod"></param>
        /// <returns></returns>
        public Translation AddTranslation(string targetState, BDelegate<object, bool>.MethodNone conditionMethod, BDelegate<object, string>.Method callbackMethod)
        {
            callbackMethod = callbackMethod ?? (i => targetState);
            return AddTranslation(targetState, new BDelegate<object, bool>(conditionMethod), new BDelegate<object, string>(callbackMethod));
        }

        /// <summary>
        /// 指定目标状态, 条件, 回调函数后添加转移
        /// </summary>
        /// <param name="targetState"></param>
        /// <param name="condition"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public Translation AddTranslation(string targetState, BDelegate<object, bool> condition, BDelegate<object, string> callback)
        {
            callback = callback ?? new BDelegate<object, string>(() => targetState);
            Translation trans = new Translation(Name, targetState, condition, callback);
            _translations.Add(trans.Name, trans);
            return trans;
        }

        /// <summary>
        /// 移除通向目标状态的转移
        /// </summary>
        /// <param name="targetState"></param>
        /// <returns></returns>
        public Translation RemoveTranslationTo(string targetState)
        {
            string tName = Name + "TO" + targetState;
            return RemoveTranslation(tName);
        }

        /// <summary>
        /// 移除转移
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Translation RemoveTranslation(string name)
        {
            if (_translations.ContainsKey(name))
            {
                Translation trans = _translations[name];
                _translations.Remove(name);
                return trans;
            }
            else
            {
                return null;
            }
        }
    }

}
