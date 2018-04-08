using System;
using System.Collections.Generic;

namespace BFramework.PathFind
{

    /// <summary>
    /// 容器类, 用于包装 Block 类
    /// </summary>
    public class Container
    {
        public Container(Block content, Block father, int gValue, int hValue)
        {
            Content = content;
            Father = father;
            GValue = gValue;
            HValue = hValue;
            Cost = GValue + HValue;
        }
        public Block Content { get; set; }
        public Block Father { get; set; }
        public int GValue { get; set; }
        public int HValue { get; set; }
        public int Cost { get; set; }

    }

    /// <summary>
    /// 路径类
    /// </summary>
    public class Path
    {
        /// <summary>
        /// 初始化 Path, 需要给定权重阈值(通行能力), 启发算法, 起点 Block, 终点 Block
        /// </summary>
        /// <param name="weightThreshold"></param>
        /// <param name="heuristic"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Path(int weightThreshold, Block start, Block end, Heuristic.TYPE heuristicType = Heuristic.TYPE.EUCLIDEAN)
        {
            WeightThreshold = weightThreshold;
            Heuristic = new Heuristic(heuristicType);
            Start = start;
            End = end;
            Open = new List<Container>
            {
                new Container(Start, null, 0, Heuristic.Calculate(Start, End))
            };
            Close = new List<Container>();
            Result = new List<Container>();
        }

        /// <summary>
        /// 权重阈值, 记录对 Block 的通行能力, 用于判断 Block 是否可以通过
        /// </summary>
        public int WeightThreshold { get; set; }

        /// <summary>
        /// 路径的权重, 是整个路径中所有 Block 的 Weight 之和
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 路径的花费, 是整个路径中所有 Container 的 Cost 之和
        /// </summary>
        public float Cost { get; set; }

        /// <summary>
        /// 启发算法, 要求输入为两个 Block, 返回两个 Block 之间的"距离"
        /// </summary>
        public Heuristic Heuristic { get; set; }

        /// <summary>
        /// 起点
        /// </summary>
        public Block Start { get; set; }

        /// <summary>
        /// 终点
        /// </summary>
        public Block End { get; set; }

        /// <summary>
        /// 待检测的 Block 列表, 需要用容器 Container 包装 Block
        /// </summary>
        public List<Container> Open { get; set; }

        /// <summary>
        /// 检测完毕的 Block 列表, 需要用容器 Container 包装 Block
        /// </summary>
        public List<Container> Close { get; set; }

        /// <summary>
        /// 寻路结果
        /// </summary>
        public List<Container> Result { get; set; }

        /// <summary>
        /// 检查 Block 能否通过
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public bool AvailableCheck(Block block)
        {
            return  WeightThreshold > block.Weight;
        }

        /// <summary>
        /// 检索路径
        /// </summary>
        /// <returns></returns>
        public bool FindByStep()
        {

            return true;
        }
    }
}
