using BFramework.World;

namespace BFramework.PathFind
{
    [System.Serializable]
    public class Agent
    {
        public enum CLIMBLINGABILITY
        {
            WEAK = 0,
            NORMAL = 1,
            STRONG = 2,
            EXCELLENT = 3,
            EXTREME = 4
        }

        public const string Extension = ".agent";

        private readonly static DIRECTION[][] _directions = new DIRECTION[5][] {
            //0, WEAK
            new DIRECTION[1] {
                DIRECTION.BOTTOM
            },
            //1, NORMAL
            new DIRECTION[1] {
                DIRECTION.BOTTOM
            },
            //2, STRONG
            new DIRECTION[5] {
                DIRECTION.BOTTOM,
                DIRECTION.LEFT,
                DIRECTION.RIGHT,
                DIRECTION.BACK,
                DIRECTION.FORWARD
            },
            //3, EXCELLENT
            new DIRECTION[6]
            {
                DIRECTION.BOTTOM,
                DIRECTION.TOP,
                DIRECTION.LEFT,
                DIRECTION.RIGHT,
                DIRECTION.BACK,
                DIRECTION.FORWARD
            },
            //4, EXTREME
            new DIRECTION[1]
            {
                DIRECTION.CENTER
            }
        };

        public Agent(string name, CLIMBLINGABILITY climblingAbility, int walkCapacity, Properties weightTable, Heuristic.TYPE heuristicType, int stepLimit)
        {
            Name = name;
            ClimblingAbility = climblingAbility;
            WalkCapacity = walkCapacity;
            WeightTable = weightTable;
            HeuristicType = heuristicType;
            StepsLimit = stepLimit;
        }

        public string Name
        {
            get; set;
        }

        public CLIMBLINGABILITY ClimblingAbility
        {
            get; set;
        }

        public DIRECTION[] ClimblingRequirements
        {
            get
            {
                return _directions[(int)ClimblingAbility];
            }
        }

        public int WalkCapacity
        {
            get; set;
        }

        public Properties WeightTable
        {
            get; set;
        }

        public Heuristic.TYPE HeuristicType
        {
            get; set;
        }

        public int StepsLimit
        {
            get; set;
        }

        public double Compute(Node node, Node target)
        {
            return Heuristic.Calculate(node, target, HeuristicType);
        }


        public bool BeAbleToPass(Node target)
        {
            return WalkCapacity >= target.Difficulty;
        }
    }
}
