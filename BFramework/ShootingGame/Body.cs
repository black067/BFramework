using System.Collections.Generic;
using BFramework.ExpandedNumber;

namespace BFramework.ShootingGame
{
    public class Body
    {
        public class Component
        {
            public Component(string name, bool crucial, Limited health, Limited defense)
            {
                _name = name;
                _crucial = crucial;
                _health = health;
                _defense = defense;
                _disabled = false;
            }
            string _name;
            bool _crucial;
            Limited _health;
            Limited _defense;
            bool _disabled;

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
            public bool Crucial
            {
                get
                {
                    return _crucial;
                }
                set
                {
                    _crucial = value;
                }
            }
            public Limited Health
            {
                get
                {
                    return _health;
                }
                set
                {
                    _health = value;
                }
            }
            public bool Disabled
            {
                get
                {
                    return _disabled;
                }
                set
                {
                    _disabled = value;
                }
            }
        }
        public Body(params Component[] components)
        {
            int length = components.Length;
            Components = new Dictionary<string, Component>(length);
            Tags = new List<string>(length);
            for (int i = 0; i <= length - 1; i++)
            {
                Components.Add(components[i].Name, components[i]);
                Tags.Add(components[i].Name);
            }
        }

        private float _health;
        public Dictionary<string, Component> Components;
        public List<string> Tags;

        public float Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
            }
        }
    }
}
