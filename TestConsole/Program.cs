using System;
using System.Collections.Generic;
using System.Linq;
using BFramework;
using BFramework.ShootingGame;
using BFramework.ExpandedMath;
using BFramework.PathFind;
using System.Collections;

namespace TestConsole
{
    class Program
    {
        /// <summary>
        /// 用于测试的类
        /// </summary>
        class TestClass : IEnumerable<int>
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

            public IEnumerator<int> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        class Attribute : IEstimable<int>
        {
            public int Width { get; set; }

            public Attribute(Attribute origin)
            {
                Width = origin.Width;
            }

            public Attribute(int width)
            {
                Width = width;
            }

            public Attribute() : this(5) { }

            public void Add(IEstimable<int> addition)
            {
                Add((Attribute)addition);
            }

            public void Add(Attribute a)
            {
                Width += a.Width;
            }

            public void Add(int addition)
            {
                Width += addition;
            }

            public void Multiply(IEstimable<int> multiplier)
            {
                Multiply((Attribute)multiplier);
            }

            public void Multiply(Attribute multiplier)
            {
                Width *= multiplier.Width;
            }

            public void Multiply(int multiplier)
            {
                Width *= multiplier;
            }

            public Attribute Clone()
            {
                return new Attribute();
            }

            public int Sum()
            {
                return Width;
            }

            IEstimable<int> IEstimable<int>.Clone()
            {
                return new Attribute(this);
            }

            public override string ToString()
            {
                return string.Format("Attribute(Width: {0})", Width);
            }

            public int GetCount()
            {
                return 1;
            }
        }

        static class Test
        {
            public static void Random()
            {
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
            }

            public static void ExpandedMath()
            {
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

                Segments segments = new Segments(45, 15, 841, 200, 478, 104, 684);
                Console.WriteLine(segments);
                Console.WriteLine(segments.Count);
                Console.WriteLine(segments.Max);
                Console.WriteLine(segments[15]);
                Console.WriteLine(segments[53]);
                Console.WriteLine(segments[76]);
                Console.WriteLine(segments[89]);
                Console.WriteLine(segments[108]);

                //限定条件数值
                Console.WriteLine("#############\nConditional Number Test");

                BDelegate<int, bool>[] conditions = new BDelegate<int, bool>[1];

                Conditional<int> conditional = new Conditional<int>(5, new BDelegate<int, bool>(delegate (int value)
                {
                    return value > 10;
                }));

                Console.WriteLine(conditional.Value);
                conditional.Value = 15;
                Console.WriteLine(conditional.Value);
                conditional.Value = 6;
                Console.WriteLine(conditional.Value);

                Console.WriteLine("Conditional Number Test Over\n");
            }

            public static void ObjectPool()
            {
                //创建对象池
                Console.WriteLine("#############\nObjectPool Test");
                ObjectPool objectPool = new ObjectPool();

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
            }

            public static void StateMachine()
            {
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
            }

            public static void ShootingGame()
            {
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
            }

            public static void PathFind()
            {
                int LengthX = 11;
                int LengthY = 15;
                int LengthZ = 9;
                Map map = new Map("TestMap", LengthX, LengthY, LengthZ, true);

                Console.WriteLine(map);
                Console.Write("#####");
                for (int i = 0; i < map.LengthX; i++)
                {
                    Console.Write(string.Format("|  X{0:D2}  ", i));
                }
                Console.Write("|\n");
                for (int i = 0; i < map.LengthY; i++)
                {
                    Console.Write(string.Format(" Y{0:D2} ", i));
                    for (int j = 0; j < map.LengthX; j++)
                    {
                        Console.Write(string.Format("|  {0:D3}  ", map.Blocks[j, i, 0].Weight));
                    }
                    Console.Write("| \n");
                }

                Path path = new Path(map[0, 0, 0], map[LengthX - 1, LengthY - 1, LengthZ - 1], 908, new Node.Attribute(), Heuristic.TYPE.MANHATTAN, 100);
                path.Find();
                foreach (Node node in path.Close)
                {
                    Console.WriteLine(node);
                }
                
                Console.WriteLine(path.Status);
            }

            public static void Estimator()
            {
                Attribute testClassB = new Attribute(); Attribute testClassB2 = new Attribute();
                testClassB.Add(testClassB2);
                Console.WriteLine("A = {0}, B = {1}", testClassB.Width, testClassB2.Width);
                Estimator<Attribute> estimator = new Estimator<Attribute>(testClassB);
                Console.WriteLine("A = {0}, B = {1}", testClassB.Width, testClassB2.Width);
                Console.WriteLine(estimator.Calculate(testClassB2));
            }
        }

        /// <summary>
        /// 命令行程序主体
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Test.Estimator();
            //Test.PathFind();
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }
        
    }
}
