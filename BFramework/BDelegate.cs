using System;
namespace BFramework
{

    /// <summary>
    /// 指定输入类型及输出类型的委托, 可以为委托添加不同的方法
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class BDelegate<TInput, TOutput>
    {

        /// <summary>
        /// 添加 TInput 输入 方法
        /// </summary>
        /// <param name="method"></param>
        public BDelegate(Method method)
        {
            _method = method;
            _case = TYPE.NORMAL;
        }
        
        /// <summary>
        /// 添加 ref TInput 输入 方法
        /// </summary>
        /// <param name="methodRef"></param>
        public BDelegate(MethodRef methodRef)
        {
            _methodRef = methodRef;
            _case = TYPE.REF;
        }

        /// <summary>
        /// 添加 parms TInput[] 输入 的方法
        /// </summary>
        /// <param name="methodParams"></param>
        public BDelegate(MethodParams methodParams)
        {
            _methodParams = methodParams;
            _case = TYPE.PARAMS;
        }

        /// <summary>
        /// 添加无输入的方法
        /// </summary>
        /// <param name="methodVoid"></param>
        public BDelegate(MethodNone methodVoid)
        {
            _methodNone = methodVoid;
            _case = TYPE.NONE;
        }

        public BDelegate(MethodTwoParams methodTwoParams)
        {
            _methodTwoParams = methodTwoParams;
            _case = TYPE.PARAMSTWO;
        }

        public TOutput Execute(TInput input)
        {
            switch (_case)
            {
                case TYPE.NORMAL:
                    return _method(input);
                case TYPE.REF:
                    return _methodRef(ref input);
                case TYPE.PARAMS:
                    return _methodParams(input);
                case TYPE.NONE:
                    return _methodNone();
            }
            return default(TOutput);
        }

        public TOutput Execute(params TInput[] inputs)
        {
            switch (_case)
            {
                case TYPE.NORMAL:
                    return _method(inputs[0]);
                case TYPE.REF:
                    return _methodRef(ref inputs[0]);
                case TYPE.PARAMS:
                    return _methodParams(inputs);
                case TYPE.NONE:
                    return _methodNone();
                case TYPE.PARAMSTWO:
                    return _methodTwoParams(inputs[0], inputs[1]);
            }
            return default(TOutput);
        }

        public delegate TOutput Method(TInput input);
        public delegate TOutput MethodRef(ref TInput input);
        public delegate TOutput MethodTwoParams(TInput input0, TInput input1);
        public delegate TOutput MethodParams(params TInput[] input);
        public delegate TOutput MethodNone();

        private enum TYPE
        {
            NORMAL = 0,
            REF = 1,
            PARAMS = 2,
            NONE = 3,
            PARAMSTWO = 4,
        }
        
        private readonly TYPE _case;
        private readonly Method _method;
        private readonly MethodRef _methodRef;
        private readonly MethodTwoParams _methodTwoParams;
        private readonly MethodParams _methodParams;
        private readonly MethodNone _methodNone;
    }
}
