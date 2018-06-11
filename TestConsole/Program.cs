using System;
using System.Collections.Generic;
using System.Linq;
using BFramework;
using BFramework.StateMachines;
using BFramework.ShootingGame;
using BFramework.ExpandedMath;
using BFramework.ExpandedMath.Distributions;
using BFramework.PathFind;
using BFramework.World;
using BFramework.Tools;
using BFramework.DataStructure;
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
        
        static class Test
        {
            public static void Random()
            {
                //随机数测试
                Console.WriteLine("#############\nBRandom.Distribution Test");
                BFramework.ExpandedMath.Random.Init();
                double[] testWeights = new double[] { 40, 100, 500, 20, 300 };
                for (int i = 0; i < 10; i++)
                {
                    Console.Write(BFramework.ExpandedMath.Random.GetIndex(testWeights) + "|");
                }
                Console.WriteLine("\n#############\nBRandom.Distribution Test End");
                Console.WriteLine("#############\nBRandom.Range Test");
                for (int i = 0; i <= 35; i++)
                {
                    int n = BFramework.ExpandedMath.Random.Range(99999, 999999);
                    Console.WriteLine("RandomNumber[{0}] = {1}", i, n);
                }
                Console.WriteLine("\n#############\nBRandom.Range Test End");
                Console.WriteLine("#############\nBRandom.Value Test");
                for (int i = 0; i <= 35; i++)
                {
                    float n = BFramework.ExpandedMath.Random.Value;
                    Console.WriteLine("RandomNumber = {0}", n);
                }
                Console.WriteLine("BRandom Test Over\n");
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

                Segments segments = new Segments(0, 3, 1, 1, 1, 1, 1);
                Console.WriteLine("var : {0}", segments);
                Console.WriteLine("Count = {0}", segments.Count);
                Console.WriteLine("Test{0} = {1}", 0, segments[0]);
                Console.WriteLine("Test{0} = {1}", 4, segments[4]);
                Console.WriteLine("Test{0} = {1}", 2, segments[2]);
                Console.WriteLine("Test{0} = {1}", 5, segments[5]);
                Console.WriteLine("Test{0} = {1}", 10, segments[10]);

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
            /*
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
            */
            public static void StateMachine()
            {
                //状态机测试
                Console.WriteLine("#############\nStateMachine Test");
                State s_1 = new State("State 1");
                State s_2 = new State("State 2");
                State s_3 = new State("State 3");
                s_1.Action = new BDelegate<object, string>(delegate (object input)
                {
                    s_1.StateMachine.Params = "I am State 2";
                    Console.WriteLine(s_1.Name + " Say: " + input);
                    return s_1.StateMachine.Tags[1];
                });
                s_2.Action = new BDelegate<object, string>(delegate (object input)
                {
                    s_2.StateMachine.Params = "I am State 3";
                    Console.WriteLine(s_2.Name + " Say: " + input);
                    return s_2.StateMachine.Tags[2];
                });
                s_3.Action = new BDelegate<object, string>(delegate (object input)
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
                    Console.WriteLine(creature.actuator.PostureMgr.StateMachine.Current);

                }

                creature.Update();
                Console.WriteLine("00 " + creature.actuator.PostureMgr.StateMachine.Current);
                creature.command.ChangePostureTo = 2;
                creature.Update();
                creature.Update();
                Console.WriteLine("02 " + creature.actuator.PostureMgr.StateMachine.Current);
                creature.command.ChangePostureTo = 3;
                creature.Update();
                creature.Update();
                Console.WriteLine("04 " + creature.actuator.PostureMgr.StateMachine.Current);
                creature.command.ChangePostureTo = 1;
                creature.Update();
                creature.Update();
                Console.WriteLine("06 " + creature.actuator.PostureMgr.StateMachine.Current);
                creature.command.ChangePostureTo = 2;
                creature.Update();
                creature.Update();
                Console.WriteLine("08 " + creature.actuator.PostureMgr.StateMachine.Current);
            }

            public static void DoPathfinding()
            {
                int LengthX = 6;
                int LengthY = 2;
                int LengthZ = 6;
                Map map = new Map("TestMap", LengthX, LengthY, LengthZ, true);

                Console.WriteLine(map);
                Console.Write("#####");
                for (int i = 0; i < map.LengthX; i++)
                {
                    Console.Write(string.Format("|  X{0:D2}  ", i));
                }
                Console.Write("|\n");
                for (int i = 0; i < map.LengthZ; i++)
                {
                    Console.Write(string.Format(" Z{0:D2} ", i));
                    for (int j = 0; j < map.LengthX; j++)
                    {
                        Map.SetNode(map[j, 0, i], Default.Properties.Random);
                        Console.Write(string.Format("|  {0:D3}  ", map.Nodes[j, 1, i].Difficulty));
                    }
                    Console.Write("| \n");
                }
                
                Properties weightDic = new Properties();
                weightDic["GVALUE"] = 1;
                weightDic["HVALUE"] = 1;
                Agent agent = new Agent("AGENT0", Agent.CLIMBLINGABILITY.EXCELLENT, 1000, 20, weightDic, Heuristic.TYPE.EUCLIDEAN, 100);
                Path path = new Path(map[0, 1, 0], map[LengthX - 1, 1, LengthZ - 1], agent);
                path.Find();
                Console.WriteLine("=========Result=========");
                foreach (Node node in path.Result)
                {
                    Console.WriteLine("{0}", node, path.GetParent(node));
                }
                Console.WriteLine("{0}, Steps = {1}", path.State, path.Steps);/**/
            }

            public static void ToolsExporter()
            {
                Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
                //Exporter<Properties>.Save("CC/C.property", new Properties());

                Exporter<Properties>.Load("A.property", out Properties p);
                //p = p ?? new Properties();
                Console.WriteLine(p.NodeType);
                /*
                Exporter<Map>.Load("Test.t", out Map map);
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
                        Console.Write(string.Format("|  {0:D3}  ", map.Nodes[j, i, 0].Difficulty));
                    }
                    Console.Write("| \n");
                }*/
            }

            public static void BinaryTreeT1()
            {
                Map map = new Map("Test", 10, 2, 11, true);
                for (int i = 0; i < map.LengthX; i++)
                {
                    for (int j = 0; j < map.LengthY; j++)
                    {
                        for (int k = 0; k < map.LengthZ; k++)
                        {
                            map[i, j, k].Cost = BFramework.ExpandedMath.Random.Range(0, 10000);
                        }
                    }
                }

                BWatch watch = new BWatch();
                List<Node> nodeList = new List<Node>() { map[0, 0, 0] };
                foreach (Node node in map.Nodes)
                {
                    nodeList.RemoveAt(0);
                    nodeList.Add(node);
                    nodeList.Sort(delegate (Node a, Node b) { return a.Cost.CompareTo(b.Cost); });
                }
                Console.WriteLine(watch.Click());
                watch.Refresh();
                BinaryTree<Node> binaryTree = new BinaryTree<Node>(map[0, 0, 10]);
                for(int i = 0; i < 10; i++)
                {
                    binaryTree.Add(map[0, 0, i]);
                }
                
                Console.WriteLine(watch.Click());
            }

            public static void TranslateConfigCSV()
            {
                Configuration configuration = Configuration.ReadCSV("TEST.csv");
                //for (int i = 0; i < configuration.NodeTypes.Length; i++)
                //{
                //    Console.Write("DataID_{0}: Wights = {1},", i, configuration.HeightOffsets.Weights[i]);
                //    for (int j = 0; j < configuration.NodeTypes[i].Length; j++)
                //    {
                //        Console.Write("Type = {0}, Wight = {1:D4}; ",configuration.NodeTypes[i][j], configuration.Weights[i].Weights[j]);
                //    }
                //    Console.Write("\n");
                //}
                Console.WriteLine("done.");

                for (int i = 0; i < 16; i++)
                {
                    Console.WriteLine(configuration.GetNodeTypeByHeight(i));
                }
            }

            public static void BuildDefaultNode()
            {

                List<Properties> properties = Default.Properties.GetPrefabs();

                foreach (Properties p in properties)
                {
                    Exporter<Properties>.Save(p.NodeType + Properties.Extension, p);
                    Console.WriteLine("type: {0}, D: {1}",p.NodeType,p["DIFFICULTY"]);
                }
            }

            public static void Generate()
            {
                Generator generator = new Generator();
                generator.Init("TestGenerator");
                Configuration configuration = generator.Config;
                foreach (Properties p in generator.Prefab.Values)
                {
                    Console.WriteLine(p.NodeType);
                }
                DateTime now = DateTime.Now;
                string format = string.Format("{0}{1}{2}{3}",now.Year, now.Month, now.Day, now.Hour);
                generator.Seed = int.Parse(format);
                Map map = generator.Build("Test", 32, 32, 32);
                Exporter<Map>.Save(map.Name + Map.Extension, map);

                Properties as_table = new Properties();
                as_table[Default.Properties.Keys.GValue] = 1;
                as_table[Default.Properties.Keys.HValue] = 1;
                Agent agent = new Agent("AS_Agent", Agent.CLIMBLINGABILITY.NORMAL, 500, 20, as_table, Heuristic.TYPE.MANHATTAN, 1000);
                string path = System.IO.Directory.GetCurrentDirectory() + "\\Agents\\" + agent.Name + Agent.Extension;
                Exporter<Agent>.Save(path, agent);
            }

            public static void Calculate()
            {
                float deltaTime = 0.5f;
                float delta = 0.35f;
                float deltaB = 0.2f;
                float last = 0;
                float current = 0;
                float deadzone = 0.0001f;

                float time = 1;
                current = delta;
                for (; current - last >= deadzone; time += deltaTime)
                {
                    last = current;
                    delta *= (1-deltaB);
                    current += delta;
                    Console.WriteLine("时间: {0}, 加成: {1}", time, current);
                }
                Console.WriteLine("总计: 时间: {0}, 加成: {1}", time, current);
            }

            public static void Reflection()
            {
                Node node = new Node(4, 5, 0, new Properties());
                BDebug.Log(node);
                Lever leverOfNode = new Lever(node.GetProperties());
                leverOfNode["DIFFICULTY"] = 124;
                BDebug.Log(leverOfNode["Cost"], leverOfNode["DIFFICULTY"]);

            }

            public static void HighOrderEq()
            {
                double x_start = 0, x_end = 70, x_highest = 35;
                double t_highest = 0.25, t_end = 0.5, t_start = 0;
                double z_end = 0, z_highest = 20, z_start = 0;
                double[] a_x = new double[6];
                double[] a_z = new double[5];
                HighOrderEquation X = new HighOrderEquation();
            }

            public static void ListInsert()
            {
                List<int> list = new List<int>();
                int[] testNum = new int[] { 123, 90, 453, 239, 604, 1990, 22, 3294, 532, 875, 755 };
                int count = 0;
                if (list.Count < 1)
                {
                    list.Add(89);
                }
                for (int i = 0, length = testNum.Length; i < length; i++)
                {
                    int j;
                    bool isInserted = false;
                    for (j = list.Count - 1; j > -1; j--)
                    {
                        count++;
                        if (testNum[i] > list[j])
                        {
                            list.Insert(j + 1, testNum[i]);
                            isInserted = true;
                            break;
                        }
                    }
                    if (!isInserted)
                    {
                        list.Insert(0, testNum[i]);
                    }
                    Console.WriteLine("TempCount[{0}] = {1}", i, count);
                }
                foreach (int item in list)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine("Count = {0}",count);

                SortedList<int, Node> nodes = new SortedList<int, Node>();
            }

            public static void HeuristicSort()
            {
                int t1 = 3, t2 = 3, t3 = 3;
                int[] result = Heuristic.Sort3(t1, t2, t3);
                Console.WriteLine("result : {0}, {1}, {2}", result[0], result[1], result[2]);
            }

            public static void BinaryTreeT2()
            {
                BinaryTree<int> tree = new BinaryTree<int>();
                tree.Add(5);
                tree.Add(2);
                tree.Add(7);
                tree.Add(10);
                tree.Add(8);
                tree.Add(6);
                Stack<int> r = new Stack<int>();
                Console.WriteLine("PreOrder");
                tree.PreOrder(tree.Root, ref r);
                for(;r.Count > 0;)
                {
                    Console.WriteLine(r.Pop());
                }
                Console.WriteLine("InOrder");
                tree.InOrder(tree.Root, ref r);
                for(;r.Count > 0;)
                {
                    Console.WriteLine(r.Pop());
                }
                Console.WriteLine("PostOrder");
                tree.PostOrder(tree.Root, ref r);
                for (; r.Count > 0;)
                {
                    Console.WriteLine(r.Pop());
                }
            }
        }

        /// <summary>
        /// 命令行程序主体
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Test.ToolsExporter();
            //Test.Generate();
            //Test.HeuristicSort();
            Test.BinaryTreeT2();
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }
        
    }
}
