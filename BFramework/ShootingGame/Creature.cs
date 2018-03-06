
namespace BFramework.ShootingGame
{
    class Creature
    {
        public class Attributes
        {
            public class BodyPart
            {
                private string _name;
                private float _health;
                private bool _disabled;
            }
            public enum POSTURE
            {
                TRANSITION = 0,
                UPRIGHT = 1,
                SQUAT = 2,
                CRAWL = 3,
            }

            private BodyPart[] _bodyParts;
            private float _health;
            private bool _alive;
            private float _energy;
            private float _defense;
            private float _speed;
            private POSTURE _posture;
            private bool _grounded;

        }

        public class Command
        {
            public bool Fire;
            public bool Jump;
            public bool Run;
            public int ChangePostureTo;
        }

        private int _id;
        private string _name;
        private Attributes _attributes;
        private Sensor _sensor;
        private Brain _brain;
        private Command _command;
        private Actuator _actuator;

        public void Refresh()
        {
            _sensor.Work(ref _attributes);
            _brain.Work(ref _attributes, ref _command);
            _actuator.Work(ref _command);
        }
    }
}
