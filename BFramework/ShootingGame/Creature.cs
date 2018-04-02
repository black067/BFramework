using BFramework.ExpandedMath;
using System.Collections.Generic;

namespace BFramework.ShootingGame
{
    public class Creature
    {
        public class Attribute
        {
            public Attribute()
            {
                body = new Body(
                    new Body.Component("Head", true, new Limited(0, 100, 100), new Limited(0, 100, 0)),
                    new Body.Component("HandR", false, new Limited(0, 100, 100), new Limited(0, 100, 0)),
                    new Body.Component("HandL", false, new Limited(0, 100, 100), new Limited(0, 100, 0))
                    );
            }

            public enum POSTURE
            {
                TRANSITION = 0,
                UPRIGHT = 1,
                SQUAT = 2,
                CRAWL = 3,
            }

            public Body body;
            public bool alive;
            public Vector position;
            public Vector transform;

            public Limited energy;
            public float speed;
            public POSTURE posture;
            public bool grounded;
        }

        public struct Command
        {
            public bool Fire;
            public Vector Move;
            public bool Jump;
            public bool Run;
            private int _changePostureTo;
            public int ChangePostureTo
            {
                get
                {
                    return _changePostureTo;
                }
                set
                {
                    if (value < -1)
                    {
                        _changePostureTo = -1;
                    }
                    else if (value > 3)
                    {
                        _changePostureTo = 3;
                    }
                    else
                    {
                        _changePostureTo = value;
                    }
                }
            }
            public void Refresh()
            {
                Fire = false;
                Move = Vector.Zero;
                Jump = false;
                Run = false;
                ChangePostureTo = -1;
            }
        }

        public Creature()
        {
            attributes = new Attribute();
            sensor = new Sensor();
            brain = new Brain();
            command = new Command();
            actuator = new Actuator();
        }
        
        public string name;
        public int id;
        public Attribute attributes;
        public Sensor sensor;
        public Brain brain;
        public Command command;
        public Actuator actuator;
        
        public void Update()
        {
            sensor.Work(ref attributes);
            brain.Work(ref attributes, ref command);
            actuator.Work(ref command);
            command.Refresh();
        }
    }
}
