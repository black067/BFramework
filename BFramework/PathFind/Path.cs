using BFramework.ExpandedMath;
using System.Collections.Generic;

namespace BFramework.PathFind
{
    /// <summary>
    /// 路径类
    /// </summary>
    public class Path
    {
        /// <summary>
        /// 初始化 Path, 需要给定权重阈值(通行能力), 启发算法, 起点 Node, 终点 Node
        /// </summary>
        /// <param name="costThreshold"></param>
        /// <param name="heuristic"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Path(Node start, Node end, int costThreshold, Node.Attribute weightDictionary, Heuristic.TYPE heuristicType = Heuristic.TYPE.EUCLIDEAN, int maxStep = 100)
        {
            Steps = 0;
            MaxStep = maxStep;
            CostThreshold = costThreshold;
            Estimator = new Estimator<Node.Attribute>(weightDictionary);
            Heuristic = new Heuristic(heuristicType);
            Start = start;
            End = end;
            Open = new List<Node>();
            Close = new List<Node>();
            SetAsOpen(Start, null);
        }
        
        /// <summary>
        /// 记录步数最大值
        /// </summary>
        public int MaxStep { get; set; }

        /// <summary>
        /// 记录当前步数
        /// </summary>
        public int Steps { get; set; }

        /// <summary>
        /// 权重阈值, 记录对 Node 的通行能力, 用于判断 Node 是否可以通过
        /// </summary>
        public int CostThreshold { get; set; }

        /// <summary>
        /// 估值器, 用于估计每个 Node 的消耗
        /// </summary>
        public Estimator<Node.Attribute> Estimator { get; set; }

        /// <summary>
        /// 路径的花费, 是整个路径中所有 Node 的 Cost 之和
        /// </summary>
        public float Cost { get; set; }

        /// <summary>
        /// 启发算法, 要求输入为两个 Node, 返回两个 Node 之间的"距离"
        /// </summary>
        public Heuristic Heuristic { get; set; }

        /// <summary>
        /// 起点
        /// </summary>
        public Node Start { get; set; }

        /// <summary>
        /// 终点
        /// </summary>
        public Node End { get; set; }

        /// <summary>
        /// 用于存放当前检测到的 Node
        /// </summary>
        public Node Current { get; set; }

        /// <summary>
        /// 待检测的 Node 列表
        /// </summary>
        public List<Node> Open { get; set; }

        /// <summary>
        /// 检测完毕的 Node 列表
        /// </summary>
        public List<Node> Close { get; set; }

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

        
        private int CompareByCost(Node blockA, Node blockB)
        {
            return blockA.tag.Cost.CompareTo(blockB.tag.Cost);
        }
        public void SetAsOpen(Node node, Node father)
        {
            Open.Add(node);
            int g = Heuristic.Calculate(node, father);
            int h = Heuristic.Calculate(node, End);
            node.tag.Set(father, g, h, g + h);
            Open.Sort(CompareByCost);
        }
        public void SetAsClosed(Node node)
        {
            Close.Add(node);
            if (Open.Contains(node))
            {
                Open.Remove(node);
            }
            node.tag.Closed = true;
        }

        /// <summary>
        /// 检索路径
        /// </summary>
        /// <returns></returns>
        public void FindByStep()
        {
            if (Open.Count < 1 || Steps == MaxStep)
            {
                Status = STATUS.FAIL;
                return;
            }
            Current = Open[0];
            SetAsClosed(Current);
            if (Current == End)
            {
                Steps++;
                Status = STATUS.SUCCESS;
                return;
            }
            foreach (Node node in Current.Neighbor)
            {
                if (node == null || node.tag.Closed || node.Weight > CostThreshold)
                {
                    continue;
                }
                if (node == End)
                {
                    SetAsClosed(node);
                    Steps++;
                    Status = STATUS.SUCCESS;
                    return;
                }
                if (Open.Contains(node))
                {
                    int gValueNew = Heuristic.Calculate(Current, node);
                    if (gValueNew < node.tag.GValue)
                    {
                        node.tag.Set(Current, gValueNew, node.tag.HValue, gValueNew + node.tag.HValue);
                    }
                }
                else
                {
                    SetAsOpen(node, Current);
                }
            }
            Steps++;
            Status = STATUS.PROCESSING;
            return;
        }

        public void Find()
        {
            for (; Status == STATUS.PROCESSING;)
            {
                FindByStep();
            }
        }
    }
}
