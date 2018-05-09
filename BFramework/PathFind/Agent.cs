using BFramework.World;

namespace BFramework.PathFind
{
    public class Agent
    {
        public enum CLIMBLINGABILITY
        {
            WEAK = 0,
            NORMAL = 1,
            STRONG = 2,
            EXTREME = 3
        }

        private readonly static DIRECTION[][] _directions = new DIRECTION[4][] {
            new DIRECTION[1] {
                DIRECTION.BOTTOM
            },
            new DIRECTION[5] {
                DIRECTION.BOTTOM,
                DIRECTION.LEFT,
                DIRECTION.RIGHT,
                DIRECTION.BACK,
                DIRECTION.FORWARD
            },
            new DIRECTION[6] {
                DIRECTION.BOTTOM,
                DIRECTION.TOP,
                DIRECTION.LEFT,
                DIRECTION.RIGHT,
                DIRECTION.BACK,
                DIRECTION.FORWARD
            },
            new DIRECTION[1]
            {
                DIRECTION.CENTER
            }
        };

        public string Name { get; set; }

        public CLIMBLINGABILITY ClimblingAbility { get; set; }

        public DIRECTION[] ClimblingRequirements
        {
            get
            {
                return _directions[(int)ClimblingAbility];
            }
        }

        public int WalkCapacity { get; set; }

    }
}
