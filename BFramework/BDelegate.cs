
namespace BFramework
{
    enum BDelegateTYPE
    {
        NONE = 0,
        NORMAL = 1,
        REF = 2,
        PARAMS = 3,
        PARAMSTWO = 4,
        RETURN = -1
    }
    
    /// <summary>
    /// 指定输入类型及输出类型的委托
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public class BDelegate<TIn, TOut>
    {
        /// <summary>
        /// 添加 TInput 输入 方法
        /// </summary>
        /// <param name="method"></param>
        public BDelegate(Method method)
        {
            _method = method;
            _case = BDelegateTYPE.NORMAL;
        }
        
        /// <summary>
        /// 添加 ref TInput 输入 方法
        /// </summary>
        /// <param name="methodRef"></param>
        public BDelegate(MethodRef methodRef)
        {
            _methodRef = methodRef;
            _case = BDelegateTYPE.REF;
        }

        /// <summary>
        /// 添加 parms TInput[] 输入 的方法
        /// </summary>
        /// <param name="methodParams"></param>
        public BDelegate(MethodParams methodParams)
        {
            _methodParams = methodParams;
            _case = BDelegateTYPE.PARAMS;
        }

        /// <summary>
        /// 添加无输入的方法
        /// </summary>
        /// <param name="methodVoid"></param>
        public BDelegate(MethodNone methodVoid)
        {
            _methodNone = methodVoid;
            _case = BDelegateTYPE.NONE;
        }

        public BDelegate(MethodTwoParams methodTwoParams)
        {
            _methodTwoParams = methodTwoParams;
            _case = BDelegateTYPE.PARAMSTWO;
        }

        public TOut Execute()
        {
            if (_case != BDelegateTYPE.NONE) return default(TOut);
            return _methodNone();
        }

        public TOut Execute(TIn input)
        {
            switch (_case)
            {
                case BDelegateTYPE.NORMAL:
                    return _method(input);
                case BDelegateTYPE.REF:
                    return _methodRef(ref input);
                case BDelegateTYPE.PARAMS:
                    return _methodParams(input);
                case BDelegateTYPE.NONE:
                    return _methodNone();
            }
            return default(TOut);
        }

        public TOut Execute(ref TIn input)
        {
            if (_case != BDelegateTYPE.REF) return default(TOut);
            return _methodRef(ref input);
        }

        public TOut Execute(params TIn[] inputs)
        {
            switch (_case)
            {
                case BDelegateTYPE.NORMAL:
                    return _method(inputs[0]);
                case BDelegateTYPE.REF:
                    return _methodRef(ref inputs[0]);
                case BDelegateTYPE.PARAMS:
                    return _methodParams(inputs);
                case BDelegateTYPE.NONE:
                    return _methodNone();
                case BDelegateTYPE.PARAMSTWO:
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
                case BDelegateTYPE.NONE:
                    return d._methodNone;
                case BDelegateTYPE.NORMAL:
                    return d._method;
                case BDelegateTYPE.REF:
                    return d._methodRef;
                case BDelegateTYPE.PARAMS:
                    return d._methodParams;
                case BDelegateTYPE.PARAMSTWO:
                    return d._methodTwoParams;
            }
            return d._method;
        }

        public delegate TOut Method(TIn input);
        public delegate TOut MethodRef(ref TIn input);
        public delegate TOut MethodTwoParams(TIn input0, TIn input1);
        public delegate TOut MethodParams(params TIn[] input);
        public delegate TOut MethodNone();
        
        private readonly BDelegateTYPE _case;
        private readonly Method _method;
        private readonly MethodRef _methodRef;
        private readonly MethodTwoParams _methodTwoParams;
        private readonly MethodParams _methodParams;
        private readonly MethodNone _methodNone;
    }

    public class BDelegate
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
    
    public class BDelegate<T>
    {
        public BDelegate(Method method)
        {
            _method = method;
            _case = BDelegateTYPE.NORMAL;
        }

        public BDelegate(MethodRef method)
        {
            _methodRef = method;
            _case = BDelegateTYPE.REF;
        }

        public BDelegate(MethodParams method)
        {
            _methodParams = method;
            _case = BDelegateTYPE.PARAMS;
        }
        public BDelegate(MethodTwoParams method)
        {
            _methodTwoParams = method;
            _case = BDelegateTYPE.PARAMSTWO;
        }
        public BDelegate(ReturnT method)
        {
            _methodReturnT = method;
            _case = BDelegateTYPE.RETURN;
        }

        public void Execute(T input)
        {
            if (_case == BDelegateTYPE.NORMAL)
                _method(input);
        }

        public void Execute(ref T input)
        {
            if (_case == BDelegateTYPE.REF)
                _methodRef(ref input);
        }

        public void Execute(params T[] inputs)
        {
            if (_case == BDelegateTYPE.PARAMSTWO && inputs.Length == 2)
            {
                _methodTwoParams(inputs[0], inputs[1]);
            }
            else if (_case == BDelegateTYPE.PARAMS) { _methodParams(inputs); }
        }

        public T Execute()
        {
            if (_case != BDelegateTYPE.RETURN) return default(T);
            return _methodReturnT();
        }

        public void Execute(T input0, T input1)
        {
            if (_case != BDelegateTYPE.PARAMSTWO) return;
            _methodTwoParams(input0, input1);
        }

        public static explicit operator System.Delegate(BDelegate<T> d)
        {
            switch (d._case)
            {
                case BDelegateTYPE.NORMAL:
                    return d._method;
                case BDelegateTYPE.REF:
                    return d._methodRef;
                case BDelegateTYPE.PARAMS:
                    return d._methodParams;
                case BDelegateTYPE.PARAMSTWO:
                    return d._methodTwoParams;
                case BDelegateTYPE.RETURN:
                    return d._methodReturnT;
            }
            return d._method;
        }


        public delegate void Method(T input);
        public delegate void MethodRef(ref T input);
        public delegate void MethodTwoParams(T input0, T input1);
        public delegate void MethodParams(params T[] input);
        public delegate T ReturnT();
        

        private readonly BDelegateTYPE _case;
        private readonly Method _method;
        private readonly MethodRef _methodRef;
        private readonly MethodTwoParams _methodTwoParams;
        private readonly MethodParams _methodParams;
        private readonly ReturnT _methodReturnT;
    }
}
