using System;
using System.Collections.Generic;

namespace BFramework.PathFind
{
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
        public Path(int weightThreshold, Block start, Block end, Heuristic.TYPE heuristicType = Heuristic.TYPE.EUCLIDEAN, int maxSteps = 100)
        {
            MaxSteps = maxSteps;
            Steps = 0;
            WeightThreshold = weightThreshold;
            Heuristic = new Heuristic(heuristicType);
            Start = start;
            End = end;
            Open = new List<Block>();
            Close = new List<Block>();
            SetAsOpen(Start, null);
        }

        public int MaxSteps { get; set; }

        public int Steps { get; set; }

        /// <summary>
        /// 权重阈值, 记录对 Block 的通行能力, 用于判断 Block 是否可以通过
        /// </summary>
        public int WeightThreshold { get; set; }

        /// <summary>
        /// 路径的权重, 是整个路径中所有 Block 的 Weight 之和
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 路径的花费, 是整个路径中所有 Block 的 Cost 之和
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
        /// 用于存放当前检测到的 Block
        /// </summary>
        public Block Current { get; set; }

        /// <summary>
        /// 待检测的 Block 列表, 需要用容器 Container 包装 Block
        /// </summary>
        public List<Block> Open { get; set; }

        /// <summary>
        /// 检测完毕的 Block 列表, 需要用容器 Container 包装 Block
        /// </summary>
        public List<Block> Close { get; set; }

        /// <summary>
        /// 用于记录寻路状态的枚举类
        /// </summary>
        public enum STATUS
        {
            FAIL = -1,
            PROCESSING = 0,
            SUCCESS = 1,
        }

        /// <summary>
        /// 寻路结果
        /// </summary>
        public STATUS Status { get; set; }

        
        private int CompareByCost(Block blockA, Block blockB)
        {
            return blockA.tag.Cost.CompareTo(blockB.tag.Cost);
        }
        public void SetAsOpen(Block block, Block father)
        {
            Open.Add(block);
            block.tag.Set(father, 
                Heuristic.Calculate(block, father), 
                Heuristic.Calculate(block, End));
            Open.Sort(CompareByCost);
        }
        public void SetAsClosed(Block block)
        {
            Close.Add(block);
            block.tag.Closed = true;
        }

        /// <summary>
        /// 检索路径
        /// </summary>
        /// <returns></returns>
        public STATUS FindByStep()
        {
            if (Open.Count < 1)
            {
                Steps++;
                return STATUS.FAIL;
            }
            Current = Open[0];
            if (Current == End)
            {
                Steps++;
                return STATUS.SUCCESS;
            }
            SetAsClosed(Current);
            foreach (Block block in Current.Neighbor)
            {
                if (block == null || block.tag.Closed || block.Weight > WeightThreshold)
                {
                    continue;
                }
                if (Open.Contains(block))
                {
                    int gValueNew = Heuristic.Calculate(Current, block);
                    if (gValueNew < block.tag.GValue)
                    {
                        block.tag.Set(Current, gValueNew, block.tag.HValue);
                    }
                }
                else
                {
                    SetAsOpen(block, Current);
                }
            }
            Steps++;
            return STATUS.PROCESSING;
        }
    }
}
