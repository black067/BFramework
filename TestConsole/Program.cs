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
            String _name = "default";
            int _age = 18;

            public int Age { get => _age; set => _age = value; }
            public string Name { get => _name; set => _name = value; }
        }
        static void Main(string[] args)
        {
            objectPool = new ObjectPool();
            objectPool.Add("A", 45);
            objectPool.Add("A", 1235);
            objectPool.Add("A", 565);
            objectPool.Add("A", 432);
            objectPool.Add("B", new TestClass("小明", 14));
            objectPool.Add("B", new TestClass("小红", 23));
            objectPool.Add("B", new TestClass("小王", 19));
            objectPool.Add("B", new TestClass("老王", 42));
            objectPool.Add(842, new TestClass("老王", 81));
            var A = "A";
            for (int i = objectPool.GetCount(A) - 1; i > -1; i--)
            {
                Object item = objectPool.GetItem("A");
                Console.WriteLine(item);
            }
            for (int i = objectPool.GetCount("B") - 1; i > -1; i--)
            {
                TestClass item = (TestClass)objectPool.GetItem("B");
                Console.WriteLine(item.Name + " , Age: " + item.Age);
            }
            Console.WriteLine(objectPool.GetItem(842));


            Console.Read();
        }

        static ObjectPool objectPool;
    }
}
