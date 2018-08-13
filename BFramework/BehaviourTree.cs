using System.Collections.Generic;

namespace BFramework.BehaviourTree
{
    /// <summary>
    /// 行为树类
    /// </summary>
    public class BehaviourTree
    {
        
    }

    /// <summary>
    /// 用于行为树节点间交互的印象类
    /// </summary>
    public class Cognition
    {
        private Dictionary<string, string> _memory;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="memory"></param>
        public Cognition(Dictionary<string, string> memory)
        {
            _memory = memory;
        }

        /// <summary>
        /// 使用上限初始化
        /// </summary>
        /// <param name="capacity"></param>
        public Cognition(int capacity) : this(new Dictionary<string, string>(capacity)) { }

        /// <summary>
        /// 根据键值取得印象内容
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                if (_memory.ContainsKey(key))
                {
                    return _memory[key];
                }
                return null;
            }
            set
            {
                if (_memory.ContainsKey(key))
                {
                    _memory[key] = value;
                }
                else
                {
                    _memory.Add(key, value);
                }
            }
        }

        /// <summary>
        /// 印象的个数
        /// </summary>
        public int Count { get { return _memory.Count; } }
    }

    /// <summary>
    /// 表明节点状态枚举类
    /// </summary>
    public enum STATUS
    {

        /// <summary>
        /// 节点状态不可用
        /// </summary>
        INVALID = -1,

        /// <summary>
        /// 待执行
        /// </summary>
        SUSPENDED = 0,

        /// <summary>
        /// 执行中
        /// </summary>
        RUNNING = 1,

        /// <summary>
        /// 执行完毕, 结果为成功
        /// </summary>
        SUCCESS = 2,

        /// <summary>
        /// 执行完毕, 结果为失败
        /// </summary>
        FAILURE = 3,
    }

    /// <summary>
    /// 行为类需要带有的方法
    /// </summary>
    public interface IBehaviour
    {

        /// <summary>
        /// 初始化行为
        /// </summary>
        /// <param name="cognition"></param>
        /// <returns></returns>
        STATUS OnInitialize(Cognition cognition);

        /// <summary>
        /// 停止时的动作
        /// </summary>
        void OnTerminate();

        /// <summary>
        /// 刷新行为
        /// </summary>
        /// <returns></returns>
        STATUS Update();
    }

    /// <summary>
    /// 动作行为
    /// </summary>
    public class Action : IBehaviour
    {

        /// <summary>
        /// 构建一个动作行为
        /// </summary>
        /// <param name="method"></param>
        public Action(BDelegate<Cognition, STATUS> method)
        {
            _tick = method;
        }

        /// <summary>
        /// 初始化节点
        /// </summary>
        /// <param name="cognition"></param>
        /// <returns></returns>
        public STATUS OnInitialize(Cognition cognition)
        {
            _cognition = cognition;
            return STATUS.RUNNING;
        }

        /// <summary>
        /// 停止节点时的动作
        /// </summary>
        public void OnTerminate()
        {
            return;
        }

        /// <summary>
        /// 更新节点
        /// </summary>
        /// <returns></returns>
        public STATUS Update()
        {
            return _tick.Execute(_cognition);
        }

        private Cognition _cognition;
        private BDelegate<Cognition, STATUS> _tick;

        /// <summary>
        /// 节点状态
        /// </summary>
        public STATUS Status { get; private set; } = STATUS.INVALID;
    }
}
