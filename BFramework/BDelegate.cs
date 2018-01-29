using System;

namespace BFramework
{
    public class BDelegate<TInput, TOutput>
    {
        public BDelegate(Method method)
        {
            _method = method;
            _case = TYPE.NORMAL;
        }

        public BDelegate(MethodRef methodRef)
        {
            _methodRef = methodRef;
            _case = TYPE.REF;
        }

        public BDelegate(MethodParams methodParams)
        {
            _methodParams = methodParams;
            _case = TYPE.PARAMS;
        }

        public TOutput this[TInput input]
        {
            get
            {
                switch (_case)
                {
                    case TYPE.NORMAL:
                        return _method(input);
                    case TYPE.REF:
                        return _methodRef(ref input);
                    case TYPE.PARAMS:
                        return _methodParams(input);
                }
                return default(TOutput);
            }
        }

        public delegate TOutput Method(TInput input);
        public delegate TOutput MethodRef(ref TInput input);
        public delegate TOutput MethodParams(params TInput[] input);

        private enum TYPE
        {
            NORMAL = 0,
            REF = 1,
            PARAMS = 2,
        }
        
        private TYPE _case;
        private Method _method;
        private MethodRef _methodRef;
        private MethodParams _methodParams;
    }
}
