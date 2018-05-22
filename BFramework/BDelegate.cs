
namespace BFramework
{
    enum DELEGATETYPE
    {
        NONE = 0,
        NORMAL = 1,
        REF = 2,
        PARAMS = 3,
        PARAMSTWO = 4,
        RETURN = -1
    }

    public interface IDelegate
    {
        void Execute();
        void Execute(params object[] args);
    }

    public interface IDelegate<T>
    {
        void Execute(T input);
        void Execute(params T[] inputs);
        void Execute(T input0, T input1);
        void Execute(ref T input);
        T Execute();
    }

    public interface IDelegate<TIn, TOut>
    {
        TOut Execute(TIn input);
        TOut Execute(params TIn[] inputs);
        TOut Execute(TIn input0, TIn input1);
        TOut Execute(ref TIn input);
        TOut Execute();
    }
    
    /// <summary>
    /// 指定输入类型及输出类型的委托
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public class BDelegate<TIn, TOut> : IDelegate<TIn, TOut>
    {
        /// <summary>
        /// 添加 TInput 输入 方法
        /// </summary>
        /// <param name="method"></param>
        public BDelegate(Method method)
        {
            _method = method;
            _case = DELEGATETYPE.NORMAL;
        }
        
        /// <summary>
        /// 添加 ref TInput 输入 方法
        /// </summary>
        /// <param name="methodRef"></param>
        public BDelegate(MethodRef methodRef)
        {
            _methodRef = methodRef;
            _case = DELEGATETYPE.REF;
        }

        /// <summary>
        /// 添加 parms TInput[] 输入 的方法
        /// </summary>
        /// <param name="methodParams"></param>
        public BDelegate(MethodParams methodParams)
        {
            _methodParams = methodParams;
            _case = DELEGATETYPE.PARAMS;
        }

        /// <summary>
        /// 添加无输入的方法
        /// </summary>
        /// <param name="methodVoid"></param>
        public BDelegate(MethodNone methodVoid)
        {
            _methodNone = methodVoid;
            _case = DELEGATETYPE.NONE;
        }

        public BDelegate(MethodTwoParams methodTwoParams)
        {
            _methodTwoParams = methodTwoParams;
            _case = DELEGATETYPE.PARAMSTWO;
        }

        public TOut Execute()
        {
            if (_case != DELEGATETYPE.NONE) return default(TOut);
            return _methodNone();
        }

        public TOut Execute(TIn input)
        {
            switch (_case)
            {
                case DELEGATETYPE.NORMAL:
                    return _method(input);
                case DELEGATETYPE.REF:
                    return _methodRef(ref input);
                case DELEGATETYPE.PARAMS:
                    return _methodParams(input);
                case DELEGATETYPE.NONE:
                    return _methodNone();
            }
            return default(TOut);
        }

        public TOut Execute(ref TIn input)
        {
            if (_case != DELEGATETYPE.REF) return default(TOut);
            return _methodRef(ref input);
        }

        public TOut Execute(params TIn[] inputs)
        {
            switch (_case)
            {
                case DELEGATETYPE.NORMAL:
                    return _method(inputs[0]);
                case DELEGATETYPE.REF:
                    return _methodRef(ref inputs[0]);
                case DELEGATETYPE.PARAMS:
                    return _methodParams(inputs);
                case DELEGATETYPE.NONE:
                    return _methodNone();
                case DELEGATETYPE.PARAMSTWO:
                    return _methodTwoParams(inputs[0], inputs[1]);
            }
            return default(TOut);
        }

        public TOut Execute(TIn input0, TIn input1)
        {
            return _methodTwoParams(input0, input1);
        }

        public static explicit operator System.Delegate(BDelegate<TIn, TOut> d)
        {
            switch (d._case)
            {
                case DELEGATETYPE.NONE:
                    return d._methodNone;
                case DELEGATETYPE.NORMAL:
                    return d._method;
                case DELEGATETYPE.REF:
                    return d._methodRef;
                case DELEGATETYPE.PARAMS:
                    return d._methodParams;
                case DELEGATETYPE.PARAMSTWO:
                    return d._methodTwoParams;
            }
            return d._method;
        }

        public delegate TOut Method(TIn input);
        public delegate TOut MethodRef(ref TIn input);
        public delegate TOut MethodTwoParams(TIn input0, TIn input1);
        public delegate TOut MethodParams(params TIn[] input);
        public delegate TOut MethodNone();
        
        private readonly DELEGATETYPE _case;
        private readonly Method _method;
        private readonly MethodRef _methodRef;
        private readonly MethodTwoParams _methodTwoParams;
        private readonly MethodParams _methodParams;
        private readonly MethodNone _methodNone;
    }

    public class BDelegate : IDelegate
    {
        public BDelegate(Method method)
        {
            _method = method;
        }
        public BDelegate(MethodParams method)
        {
            _methodParams = method;
        }

        public void Execute()
        {
            _method();
        }

        public void Execute(params object[] args)
        {
            _methodParams(args);
        }

        public static explicit operator System.Delegate(BDelegate d)
        {
            if (d._method == null) { return d._methodParams; }
            else { return d._method; }
        }

        public delegate void Method();
        public delegate void MethodParams(params object[] args);

        private Method _method;
        private MethodParams _methodParams;
    }
    
    public class BDelegate<T> : IDelegate<T>
    {
        public BDelegate(Method method)
        {
            _method = method;
            _case = DELEGATETYPE.NORMAL;
        }

        public BDelegate(MethodRef method)
        {
            _methodRef = method;
            _case = DELEGATETYPE.REF;
        }

        public BDelegate(MethodParams method)
        {
            _methodParams = method;
            _case = DELEGATETYPE.PARAMS;
        }
        public BDelegate(MethodTwoParams method)
        {
            _methodTwoParams = method;
            _case = DELEGATETYPE.PARAMSTWO;
        }
        public BDelegate(ReturnT method)
        {
            _methodReturnT = method;
            _case = DELEGATETYPE.RETURN;
        }

        public void Execute(T input)
        {
            if (_case == DELEGATETYPE.NORMAL)
                _method(input);
        }

        public void Execute(ref T input)
        {
            if (_case == DELEGATETYPE.REF)
                _methodRef(ref input);
        }

        public void Execute(params T[] inputs)
        {
            if (_case == DELEGATETYPE.PARAMSTWO && inputs.Length == 2)
            {
                _methodTwoParams(inputs[0], inputs[1]);
            }
            else if (_case == DELEGATETYPE.PARAMS) { _methodParams(inputs); }
        }

        public T Execute()
        {
            if (_case != DELEGATETYPE.RETURN) return default(T);
            return _methodReturnT();
        }

        public void Execute(T input0, T input1)
        {
            if (_case != DELEGATETYPE.PARAMSTWO) return;
            _methodTwoParams(input0, input1);
        }

        public static explicit operator System.Delegate(BDelegate<T> d)
        {
            switch (d._case)
            {
                case DELEGATETYPE.NORMAL:
                    return d._method;
                case DELEGATETYPE.REF:
                    return d._methodRef;
                case DELEGATETYPE.PARAMS:
                    return d._methodParams;
                case DELEGATETYPE.PARAMSTWO:
                    return d._methodTwoParams;
                case DELEGATETYPE.RETURN:
                    return d._methodReturnT;
            }
            return d._method;
        }


        public delegate void Method(T input);
        public delegate void MethodRef(ref T input);
        public delegate void MethodTwoParams(T input0, T input1);
        public delegate void MethodParams(params T[] input);
        public delegate T ReturnT();
        

        private readonly DELEGATETYPE _case;
        private readonly Method _method;
        private readonly MethodRef _methodRef;
        private readonly MethodTwoParams _methodTwoParams;
        private readonly MethodParams _methodParams;
        private readonly ReturnT _methodReturnT;
    }
}
