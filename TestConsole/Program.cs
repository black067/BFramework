using System;
using BFramework;
using BFramework.ShootingGame;
using BFramework.ExpandedMath;

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
            public TestClass() : this("default", BFramework.ExpandedMath.Random.Range(0, 100)) { }
            String _name = "default";
            int _age = 18;

            public int Age
            {
                get
                {
                    return _age;
                }
                set
                {
                    _age = value;
                }
            }
            public string Name
            {
                get
                {
                    return _name;
                }
                set
                {
                    _name = value;
                }
            }
        }

        /// <summary>
        /// 命令行程序主体
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

            //随机数测试
            Console.WriteLine("#############\nBRandom.Distribution Test");
            BFramework.ExpandedMath.Random.Init();
            int[] arr = BFramework.ExpandedMath.Random.Distribution(50, 1000000);
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

            //状态机测试
            Console.WriteLine("#############\nStateMachine Test");
            StateMachine.State s_1 = new StateMachine.State("State 1");
            StateMachine.State s_2 = new StateMachine.State("State 2");
            StateMachine.State s_3 = new StateMachine.State("State 3");
            s_1.Action = new BDelegate<object, string>(delegate (ref Object input)
            {
                s_1.StateMachine.Params = "I am State 2";
                Console.WriteLine(s_1.Name + " Say: " + input);
                return s_1.StateMachine.Tags[1];
            });
            s_2.Action = new BDelegate<object, string>(delegate (ref Object input)
            {
                s_2.StateMachine.Params = "I am State 3";
                Console.WriteLine(s_2.Name + " Say: " + input);
                return s_2.StateMachine.Tags[2];
            });
            s_3.Action = new BDelegate<object, string>(delegate (ref Object input)
            {
                s_3.StateMachine.Params = "I am State 1";
                Console.WriteLine(s_3.Name + " Say: " + input);
                return "State 1";
            });
            StateMachine machine = new StateMachine(s_1, s_2, s_3);
            for (int i = 0; i < 10; i++)
            {
                machine.Run();
            }
            Console.WriteLine("StateMachine Test Over\n");

            //行为树测试
            BFramework.BehaviourTree.BehaviourTree behaviourTree = new BFramework.BehaviourTree.BehaviourTree();

            //
            Console.WriteLine("#############\nShooting Game Test");
            Creature creature = new Creature();
            creature.attributes.body.Components["Head"].health += 2;
            
            while (true)
            {
                
                Console.WriteLine(creature.command.ChangePostureTo);
                ConsoleKeyInfo input = Console.ReadKey();
                if (input.Key == ConsoleKey.Escape)
                {
                    break;
                }
                else if (input.Key == ConsoleKey.C)
                {
                    creature.command.ChangePostureTo = 2;
                }
                else if (input.Key == ConsoleKey.Z)
                {
                    creature.command.ChangePostureTo = 3;
                }
                creature.Update(); creature.Update();
                Console.WriteLine(creature.actuator.PostureMgr.stateMachine.Current);
                
            }

            creature.Update();
            Console.WriteLine("00 " + creature.actuator.PostureMgr.stateMachine.Current);
            creature.command.ChangePostureTo = 2;
            creature.Update();
            creature.Update();
            Console.WriteLine("02 " + creature.actuator.PostureMgr.stateMachine.Current);
            creature.command.ChangePostureTo = 3;
            creature.Update();
            creature.Update();
            Console.WriteLine("04 " + creature.actuator.PostureMgr.stateMachine.Current);
            creature.command.ChangePostureTo = 1;
            creature.Update();
            creature.Update();
            Console.WriteLine("06 " + creature.actuator.PostureMgr.stateMachine.Current);
            creature.command.ChangePostureTo = 2;
            creature.Update();
            creature.Update();
            Console.WriteLine("08 " + creature.actuator.PostureMgr.stateMachine.Current);
            
            //数学拓展测试
            Console.WriteLine("#############\nMath Test");
            Vector vectorA = new Vector(1.51556f, 2, 3);
            Vector vectorB = new Vector(4, 5, 6);
            Console.WriteLine(vectorA + vectorB);
            Console.WriteLine(vectorA * vectorB);
            Console.WriteLine(vectorA / 3);
            Console.WriteLine(vectorA * 4.57f);
            Console.WriteLine(vectorA * 15484);
            Console.WriteLine(vectorA.Cross(vectorB));
            Console.WriteLine(vectorA.Dot(vectorB));

            Segments segments = new Segments(45,15,841,200,478,104,684);
            Console.WriteLine(segments);
            Console.WriteLine(segments.Count);
            Console.WriteLine(segments.Max);
            Console.WriteLine(segments[15]);
            Console.WriteLine(segments[53]);
            Console.WriteLine(segments[76]);
            Console.WriteLine(segments[89]);
            Console.WriteLine(segments[108]);

            
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        public static ObjectPool objectPool;
    }
}
