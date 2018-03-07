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

            public string Name { get => _name; set => _name = value; }
            public bool Crucial { get => _crucial; set => _crucial = value; }
            public Limited Health { get => _health; set => _health = value; }
            public bool Disabled { get => _disabled; set => _disabled = value; }
        }
        public Body(params Component[] components)
        {
            int length = components.Length;
            Components = new Dictionary<string, Component>(length);
            for (int i = 0; i <= length - 1; i++)
            {
                Components.Add(components[i].Name, components[i]);
            }
        }

        private float _health;
        public Dictionary<string, Component> Components;

        public float Health { get => _health; set => _health = value; }

    }
}
