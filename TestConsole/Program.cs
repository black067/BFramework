using System;
using System.Collections.Generic;
using BFramework;

namespace TestConsole
{
    class Program
    {
        public class TestClass
        {
            public TestClass(String name,int age)
            {
                _name = name;
                _age = age;
            }
            public TestClass():this("default", 0) { }
            String _name = "default";
            int _age = 18;

            public int Age { get => _age; set => _age = value; }
            public string Name { get => _name; set => _name = value; }
        }
        static void Main(string[] args)
        {
            objectPool = new ObjectPool();
            int[] arrayInt = new int[5]{0,1,2,3,4};
            objectPool.CreateNewQueue("A", arrayInt, 5);
            for (int i = objectPool.GetCount("A") - 1; i > -1; i--)
            {
                var item = objectPool.GetItem("A");
                Console.WriteLine(item);
            }
            TestClass[] testClasses = new TestClass[]
            {
                new TestClass("XM", 14),
                new TestClass("LL", 54),
            };
            objectPool.CreateNewQueue("B", testClasses, 5);
            for (int i = objectPool.GetCount("B") - 1; i > -1; i--)
            {
                TestClass item = (TestClass)objectPool.GetItem("B");
                Console.WriteLine(item.Name + " , Age: " + item.Age);
            }

            List<Type> typeList = objectPool.Types;
            foreach(Type i in typeList)
            {
                Console.WriteLine(i.ToString());
            }

            Console.WriteLine(objectPool.GetCount("A"));
            
            for (int i = objectPool.GetCount("B") - 1; i > -1; i--)
            {
                TestClass item = (TestClass)objectPool.GetItem("B");
                Console.WriteLine(item.Name + " , Age: " + item.Age);
            }

            for (int i = 10; i > -1; i--)
            {
                Console.WriteLine(BRandom.Range(-100, 100));
            }
            
            int[] arr = BRandom.Distribution(50, 1000);
            int s = 0;
            for (int i = arr.Length - 1; i > -1; i--)
            {
                s += arr[i];
            }
            Console.WriteLine("Summury = " + s);
            for (int i = arr.Length - 1; i > -1; i--)
            {
                Console.WriteLine(arr[i]);
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        public static ObjectPool objectPool;
    }
}
