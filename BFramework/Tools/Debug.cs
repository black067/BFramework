using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework.Tools
{
    public static class BDebug
    {
        public static int Count { get; set; } = 0;
        public static void Init()
        {
            Count = 0;
        }

        public static void Log()
        {
            Console.WriteLine(Count);
            Count++;
        }

        public static void Log(params object[] args)
        {
            string temp = "";
            for(int i = 0; i< args.Length; i++)
            {
                temp += args[i] + " ";
            }
            Console.WriteLine("[{0}] {1}", DateTime.Now, temp);
        }
        public static void Log(string format, params object[] args)
        {
            string temp = string.Format(format, args);
            Console.WriteLine("[{0}] {1}", DateTime.Now, temp);
        }
    }
}
