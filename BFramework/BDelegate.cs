
namespace BFramework
{
    enum BDelegateTYPE
    {
        NONE = 0,
        NORMAL = 1,
        REF = 2,
        PARAMS = 3,
        PARAMSTWO = 4,
        PARAMSTHREE = 5,
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
        public BDelegate(MethodThreeParams methodThreeParams)
        {
            _methodThreeParams = methodThreeParams;
            _case = BDelegateTYPE.PARAMSTHREE;
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
                case BDelegateTYPE.PARAMSTHREE:
                    return _methodThreeParams(inputs[0], inputs[1], inputs[2]);
            }
            return default(TOut);
        }

        public TOut Execute(TIn input0, TIn input1)
        {
            if (_case != BDelegateTYPE.PARAMSTWO)
            {
                return default(TOut);
            }
            return _methodTwoParams(input0, input1);
        }

        public TOut Execute(TIn input0, TIn input1, TIn input2)
        {
            if (_case != BDelegateTYPE.PARAMSTHREE)
            {
                return default(TOut);
            }
            return _methodThreeParams(input0, input1, input2);
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
        public delegate TOut MethodThreeParams(TIn input0, TIn Input1, TIn Input2);
        public delegate TOut MethodParams(params TIn[] input);
        public delegate TOut MethodNone();
        
        private readonly BDelegateTYPE _case;
        private readonly Method _method;
        private readonly MethodRef _methodRef;
        private readonly MethodTwoParams _methodTwoParams;
        private readonly MethodThreeParams _methodThreeParams;
        private readonly MethodParams _methodParams;
        private readonly MethodNone _methodNone;
    }

    public class BDelegate
    {
        public BDelegate()
        {
            _method = delegate () { };
        }
        public BDelegate(Method method)
        {
            _method = method ?? delegate () { };
        }
        public BDelegate(MethodParams method)
        {
            _methodParams = method ?? delegate (object[] args) { };
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

        public void Add(Method method)
        {
            if (method == null) return;
            _method += method;
        }

        public void Add(MethodParams methodParams)
        {
            if (methodParams == null) return;
            _methodParams += methodParams;
        }

        public void Remove(Method method)
        {
            if (method == null) return;
            _method -= method;
        }

        public void Remove(MethodParams methodParams)
        {
            if (methodParams == null) return;
            _methodParams -= methodParams;
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
            switch (_case)
            {
                case BDelegateTYPE.NONE:
                    _methodNone();
                    return;
                case BDelegateTYPE.NORMAL:
                    _method(input);
                    return;
                case BDelegateTYPE.REF:
                    _methodRef(ref input);
                    return;
                case BDelegateTYPE.PARAMS:
                    _methodParams(input);
                    return;
                case BDelegateTYPE.PARAMSTWO:
                    _methodTwoParams(input, default(T));
                    return;
                case BDelegateTYPE.RETURN:
                    _methodReturnT();
                    return;
            }
        }

        public void Execute(ref T input)
        {
            switch (_case)
            {
                case BDelegateTYPE.NONE:
                    _methodNone();
                    return;
                case BDelegateTYPE.NORMAL:
                    _method(input);
                    return;
                case BDelegateTYPE.REF:
                    _methodRef(ref input);
                    return;
                case BDelegateTYPE.PARAMS:
                    _methodParams(input);
                    return;
                case BDelegateTYPE.PARAMSTWO:
                    _methodTwoParams(input, default(T));
                    return;
                case BDelegateTYPE.RETURN:
                    _methodReturnT();
                    return;
            }
        }

        public void Execute(params T[] inputs)
        {
            switch (_case)
            {
                case BDelegateTYPE.NONE:
                    _methodNone();
                    return;
                case BDelegateTYPE.NORMAL:
                    if (inputs.Length > 0)
                    {
                        _method(inputs[0]);
                    }
                    else
                    {
                        _method(default(T));
                    }
                    return;
                case BDelegateTYPE.REF:
                    if (inputs.Length > 0)
                    {
                        _methodRef(ref inputs[0]);
                    }
                    else
                    {
                        T i = default(T);
                        _methodRef(ref i);
                    }
                    return;
                case BDelegateTYPE.PARAMS:
                    _methodParams(inputs);
                    return;
                case BDelegateTYPE.PARAMSTWO:
                    if (inputs.Length > 1)
                    {
                        _methodTwoParams(inputs[0], inputs[1]);
                    }
                    else if(inputs.Length > 0)
                    {
                        _methodTwoParams(inputs[0], default(T));
                    }
                    else
                    {
                        _methodTwoParams(default(T), default(T));
                    }
                    return;
                case BDelegateTYPE.RETURN:
                    _methodReturnT();
                    return;
            }
        }

        public T Execute()
        {
            switch (_case)
            {
                case BDelegateTYPE.NONE:
                    _methodNone();
                    return default(T);
                case BDelegateTYPE.NORMAL:
                    _method(default(T));
                    break;
                case BDelegateTYPE.REF:
                    T i = default(T);
                    _methodRef(ref i);
                    break;
                case BDelegateTYPE.PARAMS:
                    _methodParams(default(T));
                    break;
                case BDelegateTYPE.PARAMSTWO:
                    _methodTwoParams(default(T), default(T));
                    break;
                case BDelegateTYPE.RETURN:
                    return _methodReturnT();
            }
            return default(T);
        }

        public void Execute(T input0, T input1)
        {
            switch (_case)
            {
                case BDelegateTYPE.NONE:
                    _methodNone();
                    return;
                case BDelegateTYPE.NORMAL:
                    _method(input0);
                    return;
                case BDelegateTYPE.REF:
                    _methodRef(ref input0);
                    return;
                case BDelegateTYPE.PARAMS:
                    _methodParams(input0, input1);
                    return;
                case BDelegateTYPE.PARAMSTWO:
                    _methodTwoParams(input0, input1);
                    return;
                case BDelegateTYPE.RETURN:
                    _methodReturnT();
                    return;
            }
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
                case BDelegateTYPE.NONE:
                    return d._methodNone;
            }
            return d._method;
        }


        public delegate void Method(T input);
        public delegate void MethodRef(ref T input);
        public delegate void MethodTwoParams(T input0, T input1);
        public delegate void MethodParams(params T[] input);
        public delegate void MethodNone();
        public delegate T ReturnT();
        

        private readonly BDelegateTYPE _case;
        private readonly Method _method;
        private readonly MethodRef _methodRef;
        private readonly MethodTwoParams _methodTwoParams;
        private readonly MethodParams _methodParams;
        private readonly ReturnT _methodReturnT;
        private readonly MethodNone _methodNone;
    }
}
