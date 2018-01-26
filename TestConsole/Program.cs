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
            int[] arrayInt = new int[5]{ 0,1,2,3,4};
            objectPool.CreateNewQueue("A", arrayInt, 5);
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

            List<Type> typeList = objectPool.Types;
            foreach(Type i in typeList)
            {
                Console.WriteLine(i.ToString());
            }

            Console.WriteLine(objectPool.GetCount("A"));

            Console.Read();
        }

        public static ObjectPool objectPool;
    }
}
