﻿using BFramework.World;

namespace BFramework.PathFind
{
    /// <summary>
    /// 用于路径规划的代理类
    /// </summary>
    [System.Serializable]
    public class Agent
    {
        /// <summary>
        /// 攀附能力水准的枚举
        /// </summary>
        public enum CLIMBLINGABILITY
        {
            WEAK = 0,
            NORMAL = 1,
            STRONG = 2,
            EXCELLENT = 3,
            EXTREME = 4
        }
        
        /// <summary>
        /// 后缀名
        /// </summary>
        public const string Extension = ".agent";

        /// <summary>
        /// 所有攀附水平需要判断的方向数组
        /// </summary>
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

        /// <summary>
        /// 代理的名字
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// 代理的攀附能力
        /// </summary>
        public CLIMBLINGABILITY ClimblingAbility
        {
            get; set;
        }

        /// <summary>
        /// 判断节点能否立足时, 需要判断的方向数组
        /// </summary>
        public DIRECTION[] ClimblingRequirements
        {
            get
            {
                return _directions[(int)ClimblingAbility];
            }
        }

        /// <summary>
        /// 通行阈值
        /// </summary>
        public int WalkCapacity
        {
            get; set;
        }

        /// <summary>
        /// 节点属性权重表
        /// </summary>
        public Properties WeightTable
        {
            get; set;
        }

        /// <summary>
        /// 使用的启发算法类型
        /// </summary>
        public Heuristic.TYPE HeuristicType
        {
            get; set;
        }

        /// <summary>
        /// 每次搜索计算次数上限
        /// </summary>
        public int StepsLimit
        {
            get; set;
        }
        
        /// <summary>
        /// 根据自身使用的启发算法计算距离
        /// </summary>
        /// <param name="node"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public double Compute(Node node, Node target)
        {
            return Heuristic.Calculate(node, target, HeuristicType);
        }


        /// <summary>
        /// 判断节点可否通过
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool BeAbleToPass(Node target)
        {
            return WalkCapacity >= target.Difficulty;
        }
    }
}
