using System.Collections.Generic;
using BFramework.ExpandedMath;

namespace BFramework.ShootingGame
{
    public class Body
    {
        public class Component
        {
            public Component(string name, bool crucial, Limited health, Limited defense)
            {
                this.name = name;
                this.crucial = crucial;
                this.health = health;
                this.defense = defense;
                this.disabled = false;
            }
            public string name;
            public bool crucial;
            public Limited health;
            public Limited defense;
            public bool disabled;
        }

        public Body(params Component[] components)
        {
            int length = components.Length;
            Components = new Dictionary<string, Component>(length);
            Tags = new List<string>(length);
            for (int i = 0; i <= length - 1; i++)
            {
                Components.Add(components[i].name, components[i]);
                Tags.Add(components[i].name);
            }
        }

        public Dictionary<string, Component> Components;
        public List<string> Tags;

        public float Health
        {
            get;
            set;
        }
    }
}
