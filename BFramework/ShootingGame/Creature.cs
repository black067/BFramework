using BFramework.ExpandedNumber;

namespace BFramework.ShootingGame
{
    public class Creature
    {
        public class Attribute
        {
            public Attribute()
            {
                Body = new Body(
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

            private Body _body;
            private bool _alive;
            private Limited _energy;
            private float _speed;
            private POSTURE _posture;
            private bool _grounded;

            public Body Body { get => _body; set => _body = value; }
        }

        public struct Command
        {
            public bool Fire;
            public bool Jump;
            public bool Run;
            public int ChangePostureTo;
        }

        public Creature(string name)
        {
            Id = GetHashCode();
            Name = name;
            attributes = new Attribute();
            sensor = new Sensor();
            brain = new Brain();
            command = new Command();
            actuator = new Actuator();
        }

        public Creature() : this("Creature") { }

        private int _id;
        private string _name;
        public Attribute attributes;
        public Sensor sensor;
        public Brain brain;
        public Command command;
        public Actuator actuator;

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }

        public void Refresh()
        {
            sensor.Work(ref attributes);
            brain.Work(ref attributes, ref command);
            actuator.Work(ref command);
        }
    }
}
