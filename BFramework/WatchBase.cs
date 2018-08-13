using System;
using System.Runtime.Serialization;
using System.Collections;

namespace BFramework
{

    /// <summary>
    /// 计时器基类
    /// </summary>
    [Serializable]
    public abstract class WatchBase : ISerializable
    {

        /// <summary>
        /// 计时器完成一个循环的时长
        /// </summary>
        public float Cycle = 600;

        /// <summary>
        /// 计时器的初始值
        /// </summary>
        public float StartValue = 0;

        /// <summary>
        /// 计时器的间隔(每隔一段长度为该值的时间, 计时器将执行一次间隔事件)
        /// </summary>
        public float Interval = 60;

        /// <summary>
        /// 计时器的步长
        /// </summary>
        public float Step = 1;

        /// <summary>
        /// 计时器的当前值
        /// </summary>
        public float Current = 0;

        /// <summary>
        /// 计时器是否循环计时(即当前值为循环时长时, 是否重置当前值到初始值)
        /// </summary>
        public bool IsLoop = true;

        /// <summary>
        /// 记录上一次计时动作中, Current 与 Interval 的除数
        /// </summary>
        [NonSerialized]
        public int quotientLast = 0;

        /// <summary>
        /// 记录当前计时动作中, Current 与 Interval 的除数
        /// </summary>
        [NonSerialized]
        public int quotient = 0;

        /// <summary>
        /// 记录当前计时动作中, Current 与 Interval 的余数
        /// </summary>
        [NonSerialized]
        public float remainder = 0;

        /// <summary>
        /// 计时器是否在运行
        /// </summary>
        [NonSerialized]
        public bool IsRun = false;
        private BDelegate startAction = null;
        private BDelegate cycleAction = null;
        private BDelegate intervalAction = null;
        private BDelegate stepAction = null;

        /// <summary>
        /// 循环事件, 在 Current 的值打到循环时长时执行
        /// </summary>
        public BDelegate CycleAction
        {
            get
            {
                if (cycleAction == null)
                {
                    cycleAction = new BDelegate(CycleActionOriginal);
                }
                return cycleAction;
            }
            set => cycleAction = value;
        }

        /// <summary>
        /// 间隔事件, 在 Current 值为 Interval 的整数倍时执行
        /// </summary>
        public BDelegate IntervalAction
        {
            get
            {
                if (intervalAction == null)
                {
                    intervalAction = new BDelegate(IntervalActionOriginal);
                }
                return intervalAction;
            }
            set => intervalAction = value;
        }

        /// <summary>
        /// 计数事件, 每一次计时动作执行
        /// </summary>
        public BDelegate StepAction
        {
            get
            {
                if (stepAction == null)
                {
                    stepAction = new BDelegate(StepActionOriginal);
                }
                return stepAction;
            }
            set => stepAction = value;
        }

        /// <summary>
        /// 开始事件, 在一轮计时循环开启时执行
        /// </summary>
        public BDelegate OnStart
        {
            get
            {
                if (startAction == null)
                {
                    startAction = new BDelegate(OnStartOriginal);
                }
                return startAction;
            }
            set => startAction = value;
        }

        /// <summary>
        /// 启动计时器
        /// </summary>
        /// <param name="mytick"></param>
        protected abstract void StartUp(IEnumerator mytick);

        /// <summary>
        /// 等待指定事件(秒)
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        protected abstract IEnumerator WaitForSeconds(float seconds);

        /// <summary>
        /// 计时动作
        /// </summary>
        /// <returns></returns>
        public IEnumerator Tick()
        {
            for (; IsRun;)
            {
                quotientLast = quotient;
                StepAction.Execute();
                if (quotient != quotientLast)
                {
                    IntervalAction.Execute();
                }
                if ((Step > 0 && Current >= Cycle) || (Step < 0 && Current <= StartValue))
                {
                    CycleAction.Execute();
                }
                yield return WaitForSeconds(Step > 0 ? Step : -Step);
            }
        }

        /// <summary>
        /// 开始计时
        /// </summary>
        /// <returns></returns>
        public WatchBase Start()
        {
            OnStart.Execute();
            StartUp(Tick());
            return this;
        }

        /// <summary>
        /// 停止计时, 将重置计时器
        /// </summary>
        /// <returns></returns>
        public WatchBase Stop()
        {
            IsRun = false;
            Reset();
            return this;
        }

        /// <summary>
        /// 重置计时器
        /// </summary>
        /// <returns></returns>
        public WatchBase Reset()
        {
            Current = Step > 0 ? StartValue : Cycle;
            return this;
        }

        /// <summary>
        /// 暂停计时
        /// </summary>
        /// <returns></returns>
        public WatchBase Pause()
        {
            IsRun = false;
            return this;
        }

        /// <summary>
        /// 继续计时
        /// </summary>
        /// <returns></returns>
        public WatchBase Continue()
        {
            IsRun = true;
            StartUp(Tick());
            return this;
        }

        private void OnStartOriginal()
        {
            IsRun = true;
            Reset();
        }

        private void CycleActionOriginal() {
            if (IsLoop) { Reset(); }
            else { Stop(); }
        }

        private void IntervalActionOriginal() { }

        private void StepActionOriginal()
        {
            Current += Step;
            remainder = Current % Interval;
            quotient = (int)(Current - remainder);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Cycle", Cycle);
            info.AddValue("StartValue", StartValue);
            info.AddValue("Interval", Interval);
            info.AddValue("Step", Step);
            info.AddValue("IsLoop", IsLoop);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected WatchBase(SerializationInfo info, StreamingContext context)
        {
            Cycle = info.GetSingle("Cycle");
            StartValue = info.GetSingle("StartValue");
            Interval = info.GetSingle("Interval");
            Step = info.GetSingle("Step");
            IsLoop = info.GetBoolean("IsLoop");
        }

        /// <summary>
        /// 无参构造一个计时器
        /// </summary>
        public WatchBase()
        {
        }
    }
}
