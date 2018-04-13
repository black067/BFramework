using System;
using BFramework.ExpandedMath;
using System.Collections.Generic;

namespace BFramework.PathFind
{
    [Serializable]
    public class Property : Estimable
    {
        public enum KEY
        {
            DIFFICULTY = 0,
            GVALUE = 1,
            HVALUE = 2,
            RESISTANCE = 3,
            TEMPERATURE = 4
        }

        public int this[KEY name]
        {
            get { return this[(int)name]; }
            set { this[(int)name] = value; }
        }

        public bool Closed { get; set; }
        public bool Opened { get; set; }
        public Node Parent { get; set; }
        public int Cost { get; set; }

        public Property()
        {
            Closed = false;
            Opened = false;
            Parent = null;
            Cost = int.MaxValue;

            Dictionary = new Dictionary<string, int>();
            List<string> list = new List<string>();
            foreach (KEY n in Enum.GetValues(typeof(KEY)))
            {
                list.Add(n.ToString());
            }
            KeysArray = list.ToArray();
            foreach (string key in KeysArray)
            {
                Dictionary.Add(key, 0);
            }
        }
    }
}
