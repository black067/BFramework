
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

        /// <summary>
        /// 使用两个参数的方法构造一个 BDelegate 实例
        /// </summary>
        /// <param name="methodTwoParams"></param>
        public BDelegate(MethodTwoParams methodTwoParams)
        {
            _methodTwoParams = methodTwoParams;
            _case = BDelegateTYPE.PARAMSTWO;
        }

        /// <summary>
        /// 使用三个参数的方法构造一个 BDelegate 
        /// </summary>
        /// <param name="methodThreeParams"></param>
        public BDelegate(MethodThreeParams methodThreeParams)
        {
            _methodThreeParams = methodThreeParams;
            _case = BDelegateTYPE.PARAMSTHREE;
        }

        /// <summary>
        /// 执行无参数的 BDelegate, 并得到一个指定类型的输出值
        /// </summary>
        /// <returns></returns>
        public TOut Execute()
        {
            if (_case != BDelegateTYPE.NONE) return default(TOut);
            return _methodNone();
        }

        /// <summary>
        /// 执行一个输入参数的 BDelegate, 并得到一个指定类型的输出值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 执行一个 ref 参数的 BDelegate, 并得到一个指定类型的输出值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public TOut Execute(ref TIn input)
        {
            if (_case != BDelegateTYPE.REF) return default(TOut);
            return _methodRef(ref input);
        }

        /// <summary>
        /// 执行多于三个输入参数 BDelegate, 并得到一个指定类型的输出值
        /// </summary>
        /// <param name="input0"></param>
        /// <param name="input1"></param>
        /// <param name="input2"></param>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public TOut Execute(TIn input0, TIn  input1, TIn  input2, params TIn[] inputs)
        {
            switch (_case)
            {
                case BDelegateTYPE.NORMAL:
                    return _method(input0);
                case BDelegateTYPE.REF:
                    return _methodRef(ref input0);
                case BDelegateTYPE.PARAMS:
                    TIn[] inputs_ = new TIn[inputs.Length + 3];
                    inputs_[0] = input0; inputs_[1] = inputs[1]; inputs_[2] = inputs[2];
                    for (int i = 3, l = inputs_.Length; i < l; i++)
                    {
                        inputs_[i] = inputs[i - 3];
                    }
                    return _methodParams(inputs_);
                case BDelegateTYPE.NONE:
                    return _methodNone();
                case BDelegateTYPE.PARAMSTWO:
                    return _methodTwoParams(input0, input1);
                case BDelegateTYPE.PARAMSTHREE:
                    return _methodThreeParams(input0, input1, input2);
            }
            return default(TOut);
        }

        /// <summary>
        /// 执行两个参数的 BDelegate, 并得到一个指定类型的输出值
        /// </summary>
        /// <param name="input0"></param>
        /// <param name="input1"></param>
        /// <returns></returns>
        public TOut Execute(TIn input0, TIn input1)
        {
            if (_case != BDelegateTYPE.PARAMSTWO)
            {
                return default(TOut);
            }
            return _methodTwoParams(input0, input1);
        }

        /// <summary>
        /// 执行三个参数的 BDelegate, 并得到一个指定类型的输出值
        /// </summary>
        /// <param name="input0"></param>
        /// <param name="input1"></param>
        /// <param name="input2"></param>
        /// <returns></returns>
        public TOut Execute(TIn input0, TIn input1, TIn input2)
        {
            if (_case != BDelegateTYPE.PARAMSTHREE)
            {
                return default(TOut);
            }
            return _methodThreeParams(input0, input1, input2);
        }

        /// <summary>
        /// 定义与系统 Delegate 的转换
        /// </summary>
        /// <param name="d"></param>
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

        /// <summary>
        /// 一个 TIn 类型输入, 返回值为 TOut 类型的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public delegate TOut Method(TIn input);

        /// <summary>
        ///  一个 ref TIn 类型输入, 返回值为 TOut 类型的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public delegate TOut MethodRef(ref TIn input);

        /// <summary>
        ///  两个 TIn 类型输入, 返回值为 TOut 类型的方法
        /// </summary>
        /// <param name="input0"></param>
        /// <param name="input1"></param>
        /// <returns></returns>
        public delegate TOut MethodTwoParams(TIn input0, TIn input1);

        /// <summary>
        /// 三个 TIn 类型输入, 返回值为 TOut 类型的方法
        /// </summary>
        /// <param name="input0"></param>
        /// <param name="Input1"></param>
        /// <param name="Input2"></param>
        /// <returns></returns>
        public delegate TOut MethodThreeParams(TIn input0, TIn Input1, TIn Input2);

        /// <summary>
        /// 多个 TIn 类型输入, 返回值为 TOut 类型的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public delegate TOut MethodParams(params TIn[] input);

        /// <summary>
        ///  无参数输入, 返回值为 TOut 类型的方法
        /// </summary>
        /// <returns></returns>
        public delegate TOut MethodNone();
        
        private readonly BDelegateTYPE _case;
        private readonly Method _method;
        private readonly MethodRef _methodRef;
        private readonly MethodTwoParams _methodTwoParams;
        private readonly MethodThreeParams _methodThreeParams;
        private readonly MethodParams _methodParams;
        private readonly MethodNone _methodNone;
    }

    /// <summary>
    /// 不指定输入\输出类型的委托
    /// </summary>
    public class BDelegate
    {

        /// <summary>
        /// 构造一个空委托
        /// </summary>
        public BDelegate()
        {
            _method = delegate () { };
        }

        /// <summary>
        /// 给定无参数方法构造委托
        /// </summary>
        /// <param name="method"></param>
        public BDelegate(Method method)
        {
            _method = method ?? delegate () { };
        }

        /// <summary>
        /// 给定多参数方法构造委托
        /// </summary>
        /// <param name="method"></param>
        public BDelegate(MethodParams method)
        {
            _methodParams = method ?? delegate (object[] args) { };
        }

        /// <summary>
        /// 执行委托
        /// </summary>
        public void Execute()
        {
            _method();
        }

        /// <summary>
        /// 给定多个参数执行委托
        /// </summary>
        /// <param name="args"></param>
        public void Execute(params object[] args)
        {
            _methodParams(args);
        }

        /// <summary>
        /// 定义与系统 Delegate 的转换
        /// </summary>
        /// <param name="d"></param>
        public static explicit operator System.Delegate(BDelegate d)
        {
            if (d._method == null) { return d._methodParams; }
            else { return d._method; }
        }

        /// <summary>
        /// 为委托添加无参数动作
        /// </summary>
        /// <param name="method"></param>
        public void Add(Method method)
        {
            if (method == null) return;
            _method += method;
        }

        /// <summary>
        /// 为委托添加不定长参数动作
        /// </summary>
        /// <param name="methodParams"></param>
        public void Add(MethodParams methodParams)
        {
            if (methodParams == null) return;
            _methodParams += methodParams;
        }

        /// <summary>
        /// 为委托移除无参数动作
        /// </summary>
        /// <param name="method"></param>
        public void Remove(Method method)
        {
            if (method == null) return;
            _method -= method;
        }

        /// <summary>
        /// 为委托移除不定长参数动作
        /// </summary>
        /// <param name="methodParams"></param>
        public void Remove(MethodParams methodParams)
        {
            if (methodParams == null) return;
            _methodParams -= methodParams;
        }

        /// <summary>
        /// 无参数委托
        /// </summary>
        public delegate void Method();

        /// <summary>
        /// 不定长参数委托
        /// </summary>
        /// <param name="args"></param>
        public delegate void MethodParams(params object[] args);

        private Method _method;
        private MethodParams _methodParams;
    }
    
    /// <summary>
    /// 带有一个泛型参数的委托, 泛型参数可能为输出结果类型或者输入参数类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BDelegate<T>
    {

        /// <summary>
        /// 给定一个输入参数(类型为 T )的动作, 构造委托
        /// </summary>
        /// <param name="method"></param>
        public BDelegate(Method method)
        {
            _method = method;
            _case = BDelegateTYPE.NORMAL;
        }

        /// <summary>
        /// 给定一个输入参数标记为 ref (类型为 T )的动作, 构造委托
        /// </summary>
        /// <param name="method"></param>
        public BDelegate(MethodRef method)
        {
            _methodRef = method;
            _case = BDelegateTYPE.REF;
        }

        /// <summary>
        /// 给定不定长输入参数(类型为 T )的动作, 构造委托
        /// </summary>
        /// <param name="method"></param>
        public BDelegate(MethodParams method)
        {
            _methodParams = method;
            _case = BDelegateTYPE.PARAMS;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public BDelegate(MethodTwoParams method)
        {
            _methodTwoParams = method;
            _case = BDelegateTYPE.PARAMSTWO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public BDelegate(ReturnT method)
        {
            _methodReturnT = method;
            _case = BDelegateTYPE.RETURN;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public BDelegate(MethodNone method)
        {
            _methodNone = method;
            _case = BDelegateTYPE.NONE;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input0"></param>
        /// <param name="input1"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public delegate void Method(T input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public delegate void MethodRef(ref T input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input0"></param>
        /// <param name="input1"></param>
        public delegate void MethodTwoParams(T input0, T input1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public delegate void MethodParams(params T[] input);

        /// <summary>
        /// 
        /// </summary>
        public delegate void MethodNone();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
