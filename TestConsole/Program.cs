using System;
using System.Collections.Generic;
using BFramework;

namespace TestConsole
{
    class Program
    {
        /// <summary>
        /// 用于测试的类
        /// </summary>
        public class TestClass
        {
            public TestClass(String name, int age)
            {
                _name = name;
                _age = age;
            }
            public TestClass() : this("default", BRandom.Range(0, 100)) { }
            String _name = "default";
            int _age = 18;

            public int Age { get => _age; set => _age = value; }
            public string Name { get => _name; set => _name = value; }
        }

        /// <summary>
        /// 命令行程序主题
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //创建对象池
            Console.WriteLine("#############\nObjectPool Test");
            objectPool = new ObjectPool();

            TestClass[] testClasses = new TestClass[]
            {
                new TestClass("XM", 14),
                new TestClass("LL", 54),
                new TestClass("XY", 24),
                new TestClass("BQ", 27),
                new TestClass("GS", 45),
                new TestClass("WH", 87),
            };

            objectPool.CreateNewQueue("B", testClasses, 5);

            for (int i = objectPool.GetCount("B") - 1; i > -1; i--)
            {
                TestClass item = (TestClass)objectPool.GetItem("B");
                objectPool.Restore(item);
                Console.WriteLine(item.Name + ", Age: " + item.Age);
            }
            
            for (int i = objectPool.GetCount("B") - 1; i > -1; i--)
            {
                TestClass item = (TestClass)objectPool.GetItem("B");
                Console.WriteLine(item.Name + ", Age: " + item.Age);
            }
            Console.WriteLine("ObjectPool Test Over\n");

            Console.WriteLine("#############\nBRandom.Distribution Test");
            int[] arr = BRandom.Distribution(3, 25);
            for (int i = arr.Length - 1; i > -1; i--)
            {
                Console.WriteLine(arr[i]);
            }
            int s = 0;
            for (int i = arr.Length - 1; i > -1; i--)
            {
                s += arr[i];
            }
            Console.WriteLine("Summury = " + s);
            Console.WriteLine("BRandom.Distribution Test Over\n");

            Console.WriteLine("#############\nStateMachine Test");
            StateMachine.State s_1 = new StateMachine.State("State 1");
            StateMachine.State s_2 = new StateMachine.State("State 2");
            StateMachine machine = new StateMachine(s_1, s_2);
            s_1.Action = new BDelegate<string>(delegate (Object[] __args)
            {
                s_1.StateMachine.Params = "I am 123";
                Console.WriteLine(s_1.Name + " Say: " + __args[0]);
                return s_1.StateMachine.Tags[1];
            });
            s_2.Action = new BDelegate<string>(delegate (Object[] __args)
            {
                s_2.StateMachine.Params = "I am 1";
                Console.WriteLine(s_2.Name + " Say: " + __args[0]);
                return s_2.StateMachine.Tags[0];
            });

            for (int i = 0; i < 10; i++)
            {
                machine.Run();
            }
            Console.WriteLine("StateMachine Test Over\n");

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        public static ObjectPool objectPool;
    }
}
