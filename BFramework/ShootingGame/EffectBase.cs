using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace BFramework.ShootingGame
{

    /// <summary>
    /// 确定可被 Effect 作用的类带有的属性与方法
    /// </summary>
    public interface IAffectable
    {

        /// <summary>
        /// 被指定效果影响
        /// </summary>
        /// <param name="effect"></param>
        void BeAffectedBy(EffectBase effect);

        /// <summary>
        /// 根据效果作用的不同, 对效果执行相应的初始化工作
        /// </summary>
        Dictionary<string, BDelegate<EffectBase>.Method> AffectedActions { get; }

        /// <summary>
        /// 计算效果的作用
        /// </summary>
        Action<EffectBase.CALTYPE, bool> Compute { get; }
    }

    /// <summary>
    /// 效果的基类
    /// </summary>
    [Serializable]
    public abstract class EffectBase : WatchBase, IComparable
    {
        private static Dictionary<string, Action<EffectBase, string>> _compareActions;

        /// <summary>
        /// 从字符串反序列化时, 进行比较的方法字典
        /// </summary>
        public static Dictionary<string, Action<EffectBase, string>> CompareActions
        {
            get
            {
                if (_compareActions == null)
                {
                    _compareActions = new Dictionary<string, Action<EffectBase, string>>()
                    {
                        {"Name", (e, v)=>{ e.Name =  v; } },
                        {"MemberName", (e, v)=>{ e.MemberName =  v; } },
                        {"CalType", (e, v)=>{
                            e.CalType = int.TryParse(v, out int newV) ? 
                                (CALTYPE)newV : 
                                ((CALTYPE)Enum.Parse(typeof(CALTYPE), v, true)); } },
                        {"Cycle", (e, v)=>{ e.Cycle = float.TryParse(v, out float newV) ? newV : 5; } },
                        {"Step", (e, v)=>{e.Step = float.TryParse(v, out float newV) ? newV : 1; } },
                        {"Parameter", (e, v)=>{ e.Parameter = float.TryParse(v, out float newV) ? newV : 1; } },
                        {"NeedReset",
                            (e, v)=>
                            {
                                e.NeedReset = bool.TryParse(v, out bool newV) ? 
                                newV : 
                                (int.TryParse(v, out int newI) ? 
                                    (newI == 0 ? 
                                        false : 
                                        true) : 
                                    true);
                            }
                        },
                        {"ExertsImmediately",
                            (e, v)=>
                            {
                                e.ExertsImmediately = bool.TryParse(v, out bool newV) ?
                                newV :
                                (int.TryParse(v, out int newI) ?
                                    (newI == 0 ?
                                        false :
                                        true) :
                                    true);
                            }
                        },
                        {"IsLoop",
                            (e, v) =>
                            {
                                e.IsLoop = bool.TryParse(v, out bool newV) ?
                                newV :
                                (int.TryParse(v, out int newI) ?
                                    (newI == 0 ?
                                        false :
                                        true) :
                                    true);
                            }
                        },
                    };
                }
                return _compareActions;
            }
        }

        /// <summary>
        /// 从字符串反序列化一个效果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T ReadString<T>(string str) where T : EffectBase, new()
        {
            T e = new T();
            string[] strArr = str.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string tempA, tempB;
            List<string> compareList = CompareActions.Keys.ToList();
            for (int i = 0, length = strArr.Length - 1; i < length; i += 2)
            {
                tempA = strArr[i];
                tempB = strArr[i + 1];
                for (int j = compareList.Count - 1; j > -1; j--)
                {
                    if (string.Equals(tempA, compareList[j], StringComparison.CurrentCultureIgnoreCase))
                    {
                        CompareActions[compareList[j]].Invoke(e, tempB);
                        compareList.RemoveAt(j);
                        break;
                    }
                }
            }
            return e;
        }
        
        private static IFormatter formatter;

        /// <summary>
        /// 格式化工具
        /// </summary>
        protected static IFormatter Formatter
        {
            get
            {
                if (formatter == null)
                {
                    formatter = new BinaryFormatter();
                }
                return formatter;
            }
            set => formatter = value;
        }

        /// <summary>
        /// 序列化一个效果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string Serialize<T>(T item) where T : EffectBase
        {
            MemoryStream stream = new MemoryStream();
            Formatter.Serialize(stream, item);
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Close();
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// 反序列化效果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedMsg"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string serializedMsg) where T : EffectBase
        {
            byte[] buffer = Convert.FromBase64String(serializedMsg);
            MemoryStream stream = new MemoryStream(buffer);
            T item = Formatter.Deserialize(stream) as T;
            stream.Flush();
            stream.Close();
            return item;
        }
        
        /// <summary>
        /// 计算类型
        /// </summary>
        public enum CALTYPE
        {
            /// <summary>
            /// 加法
            /// </summary>
            Add = 0,

            /// <summary>
            /// 乘法
            /// </summary>
            Multiple = 1,

            /// <summary>
            /// 锁定
            /// </summary>
            Lock = 2,
        }

        /// <summary>
        /// 效果名
        /// </summary>
        public string Name;

        /// <summary>
        /// 计算方式
        /// </summary>
        public CALTYPE CalType;

        /// <summary>
        /// 效果作用的成员名
        /// </summary>
        public string MemberName = string.Empty;

        /// <summary>
        /// 作用参数
        /// </summary>
        public float Parameter = 0;

        /// <summary>
        /// 在效果作用完毕后是否需要重置相应的成员
        /// </summary>
        public bool NeedReset = false;

        /// <summary>
        /// 作用次数
        /// </summary>
        public int CallCount = 0;

        /// <summary>
        /// 是否立即产生作用
        /// </summary>
        public bool ExertsImmediately = false;

        /// <summary>
        /// 效果是否还在作用中
        /// </summary>
        [NonSerialized]
        public bool IsAvailable = true;

        /// <summary>
        /// 用于记录作用目标当前值的变量
        /// </summary>
        [NonSerialized]
        protected float _value = 0;

        /// <summary>
        /// 是否已经初始化完毕
        /// </summary>
        public bool IsInitialized { get; protected set; }

        /// <summary>
        /// 计算作用效果方法的委托
        /// </summary>
        /// <param name="value"></param>
        /// <param name="positive"></param>
        public delegate void ComputeMethod(ref float value, bool positive);

        /// <summary>
        /// 计算作用效果
        /// </summary>
        public ComputeMethod Compute;

        /// <summary>
        /// 默认的计算方法, 将根据效果的作用类型, 对效果指定目标的成员(MemberName)产生作用
        /// </summary>
        /// <param name="value"></param>
        /// <param name="positive"></param>
        protected void DefaultCompute(ref float value, bool positive)
        {
            switch (CalType)
            {
                case CALTYPE.Add:
                    value += positive ? Parameter : -Parameter;
                    break;
                case CALTYPE.Multiple:
                    value *= positive ? Parameter : (1 / Parameter);
                    break;
                case CALTYPE.Lock:
                    value = Parameter;
                    break;
            }
        }

        /// <summary>
        /// 初始化效果
        /// </summary>
        /// <param name="target"></param>
        /// <param name="stepAction"></param>
        /// <param name="cycleAction"></param>
        /// <returns></returns>
        public EffectBase Init(object target, BDelegate.Method stepAction = null, BDelegate.Method cycleAction = null)
        {
            if (IsInitialized || IsRun) { return this; }
            IsLoop = false;
            CallCount = 0;
            Compute = Compute ?? DefaultCompute;
            Type objectType = target.GetType();
            FieldInfo fInfo = objectType.GetField(MemberName, BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo pInfo;
            if (fInfo != null && fInfo.FieldType == (typeof(float)))
            {
                _value = (float)fInfo.GetValue(target);
                stepAction = stepAction ?? 
                    (() =>
                    {
                        Compute(ref _value, true);
                        fInfo.SetValue(target, _value);
                        CallCount++;
                    });
                cycleAction = cycleAction ?? 
                    (() =>
                    {
                        for (; NeedReset && CallCount > 0; CallCount--)
                        {
                            Compute(ref _value, false);
                            fInfo.SetValue(target, _value);
                        }
                        CallCount = 0;
                        IsAvailable = false;
                    });
            }
            else
            {
                pInfo = objectType.GetProperty(MemberName, BindingFlags.Instance | BindingFlags.Public);
                if (pInfo == null || pInfo.PropertyType != (typeof(float))) { return this; }
                _value = (float)pInfo.GetValue(target, null);
                stepAction = stepAction ?? 
                    (() =>
                    {
                        Compute(ref _value, true);
                        pInfo.SetValue(target, _value, null);
                        CallCount++;
                    });
                cycleAction = cycleAction ??
                    (() =>
                    {
                        for (; NeedReset && CallCount > 0; CallCount--)
                        {
                            Compute(ref _value, false);
                            pInfo.SetValue(target, _value, null);
                        }
                        CallCount = 0;
                        IsAvailable = false;
                    });
            }
            OnStart.Add(ExertsImmediately ? stepAction : null);
            StepAction.Add(stepAction);
            CycleAction.Add(cycleAction);
            IsInitialized = true;
            return this;
        }

        /// <summary>
        /// 对于可被效果作用的物体进行初始化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public EffectBase InitGeneric<T>(T target) where T : class, IAffectable
        {
            if (IsInitialized || IsRun) { return this; }
            IsLoop = false;
            CallCount = 0;

            void stepAction()
            {
                target.Compute.Invoke(CalType, true);
                CallCount++;
            }
            if (ExertsImmediately)
            {
                OnStart.Add(stepAction);
            }
            StepAction.Add(stepAction);
            CycleAction.Add(() =>
            {
                for (; NeedReset && CallCount > 0; CallCount--)
                {
                    target.Compute.Invoke(CalType, false);
                }
                CallCount = 0;
                IsAvailable = false;
            });
            IsInitialized = true;
            return this;
        }

        /// <summary>
        /// 序列化为字符串(可读)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Name {0},MemberName {1},CalType {2},Parameter {3},Cycle {4},Step {5},NeedReset {6},ExertsImmediately {7}, IsLoop {8}", Name, MemberName, CalType, Parameter, Cycle, Step, NeedReset, ExertsImmediately, IsLoop);
        }

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return Name.CompareTo(obj);
        }

        /// <summary>
        /// 通过 ISerializable 接口序列化实例
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Name", Name);
            info.AddValue("MemberName", MemberName);
            info.AddValue("CalType", CalType);
            info.AddValue("Parameter", Parameter);
            info.AddValue("NeedReset", NeedReset);
            info.AddValue("ExertsImmediately", ExertsImmediately);
        }

        /// <summary>
        /// 通过 ISerializabel 接口反序列化实例
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected EffectBase(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Name = info.GetString("Name");
            MemberName = info.GetString("MemberName");
            CalType = (CALTYPE)info.GetValue("CalType", typeof(CALTYPE));
            Parameter = info.GetSingle("Parameter");
            NeedReset = info.GetBoolean("NeedReset");
            ExertsImmediately = info.GetBoolean("ExertsImmediately");
        }

        /// <summary>
        /// 新建一个效果实例
        /// </summary>
        public EffectBase()
        {
        }
    }
}
