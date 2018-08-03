using System;

namespace BFramework.Tools
{
    [Serializable]
    public class BWatch
    {
        protected int _cycle = 60;
        public int Cycle
        {
            get { return _cycle; }
            set { _cycle = value != 0 ? (value > 0 ? value : -value) : 1; }
        }
        protected int _interval = 10;
        public int Interval
        {
            get { return _interval; }
            set { _interval = value != 0 ? (value > 0 ? value : -value) : 1; }
        }
        protected int _step = 1;
        public int Step
        {
            get { return _step; }
            set
            {
                _step = value != 0 ? (value > 0 ? value : -value) : 1;
            }
        }
        public int Current { get; protected set; } = 0;
        public bool AutoReset = true;
        public bool IsRun { get; protected set; } = false;
        public BDelegate CycleAction = null;
        public BDelegate IntervalAction = null;
        public BDelegate StepAction = null;

        public BWatch(int cycle = 60, int interval = 10, int step = 1, BDelegate.Method cycleMethod = null, BDelegate.Method intervalMethod = null, BDelegate.Method stepMethod = null)
        {
            Cycle = cycle;
            CycleAction = new BDelegate(CycleActionOriginal);
            CycleAction.Add(cycleMethod);
            Interval = interval;
            IntervalAction = new BDelegate(IntervalActionOriginal);
            IntervalAction.Add(intervalMethod);
            Step = step;
            StepAction = new BDelegate(StepActionOriginal);
            StepAction.Add(stepMethod);
        }

        public BWatch Start()
        {
            Reset();
            IsRun = true;
            return this;
        }

        public BWatch Tick()
        {
            if (!IsRun)
            {
                return this;
            }
            StepAction.Execute();
            if (Current % Interval == 0)
            {
                IntervalAction.Execute();
            }
            if(Current >= Cycle)
            {
                CycleAction.Execute();
            }
            return this;
        }

        public BWatch Stop()
        {
            IsRun = false;
            return this;
        }

        public BWatch Reset()
        {
            Current = 0;
            return this;
        }

        public void CycleActionOriginal()
        {
            if (AutoReset)
            {
                Reset();
            }
            else
            {
                Stop();
            }
        }

        public void IntervalActionOriginal()
        {

        }

        public void StepActionOriginal()
        {
            Current += Step;
        }
    }
}
