using BFramework.ExpandedMath;
using System.Collections.Generic;

namespace BFramework.PathFind
{
    /// <summary>
    /// 路径类
    /// </summary>
    public class Path
    {
        Estimator<Node.Attribute> _estimator;
        /// <summary>
        /// 初始化 Path, 需要给定起点, 终点, 通行力阈值, 价值估计权重表, 启发算法类型, 最大计算步数
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="walkabilityThreshold"></param>
        /// <param name="weightDictionary"></param>
        /// <param name="heuristicType"></param>
        /// <param name="maxStep"></param>
        public Path(
            Map map,
            Node start, 
            Node end, 
            int walkabilityThreshold, 
            Node.Attribute weightDictionary, 
            Heuristic.TYPE heuristicType = Heuristic.TYPE.EUCLIDEAN, 
            int maxStep = 100)
        {
            environment = map;
            Status = STATUS.PROCESSING;
            Steps = 0;
            MaxStep = maxStep;
            WalkabilityThreshold = walkabilityThreshold;
            Estimator = new Estimator<Node.Attribute>(weightDictionary);
            Heuristic = new Heuristic(heuristicType);
            Start = start;
            End = end;
            Opened = new List<Node>();
            Closed = new List<Node>();
            Result = new List<Node>();
            PushToOpened(Start, null);
        }

        public Map environment;

        /// <summary>
        /// 记录步数最大值
        /// </summary>
        public int MaxStep { get; set; }

        /// <summary>
        /// 记录当前步数
        /// </summary>
        public int Steps { get; set; }

        /// <summary>
        /// 通行力阈值, 记录对 Node 的通行能力, 用于判断 Node 是否可以通过
        /// </summary>
        public int WalkabilityThreshold { get; set; }

        /// <summary>
        /// 估值器, 用于估计每个 Node 的消耗
        /// </summary>
        public Estimator<Node.Attribute> Estimator { get { return _estimator; } private set { _estimator = value; } }

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
        /// 当前检测到的 Node
        /// </summary>
        public Node Current { get; set; }

        /// <summary>
        /// 待检测的 Node 列表
        /// </summary>
        public List<Node> Opened { get; set; }

        /// <summary>
        /// 检测完毕的 Node 列表
        /// </summary>
        public List<Node> Closed { get; set; }
        
        /// <summary>
        /// 路径检索的最终结果
        /// </summary>
        public List<Node> Result { get; private set; }

        /// <summary>
        /// 用于记录工作状态的枚举类
        /// </summary>
        public enum STATUS
        {
            FAIL = -1,
            PROCESSING = 0,
            SUCCESS = 1,
        }

        /// <summary>
        /// 工作状态
        /// </summary>
        public STATUS Status { get; set; }
        
        /// <summary>
        /// 比较两个 Node 的开销
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns></returns>
        private int CompareByCost(Node node1, Node node2)
        {
            return node1.property.Cost.CompareTo(node2.property.Cost);
        }

        /// <summary>
        /// 将指定 Node 添加到开启列表中(需要设置其父节点)
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parent"></param>
        public void PushToOpened(Node node, Node parent)
        {
            Opened.Add(node);
            node.property.Opened = true;
            node.Parent = parent;
            node.property.GValue = Heuristic.Calculate(node, parent);
            node.property.HValue = Heuristic.Calculate(node, End);
            node.SetCost(ref _estimator);
            Opened.Sort(CompareByCost);
        }

        /// <summary>
        /// 将指定 Node 添加到关闭列表中
        /// </summary>
        /// <param name="node"></param>
        public void PushToClosed(Node node)
        {
            Closed.Add(node);
            if (node.property.Opened)
            {
                Opened.Remove(node);
            }
            node.property.Closed = true;
            node.property.Opened = false;
        }

        /// <summary>
        /// 失败时执行动作
        /// </summary>
        public void OnFail()
        {
            Status = STATUS.FAIL;
        }

        /// <summary>
        /// 成功时执行动作
        /// </summary>
        public void OnSuccess()
        {
            Status = STATUS.SUCCESS;
            int length = Closed.Count;
            End.Parent = End.Parent ?? Closed[length - 2];
            Result = new List<Node>(Closed.Count)
            {
                End
            };
            for (int i = 1; i <= length && Result[i - 1].Parent != null; i++)
            {
                Result.Add(Result[i - 1].Parent);
            }
        }

        /// <summary>
        /// 按步检索路径
        /// </summary>
        /// <returns></returns>
        public void FindByStep()
        {
            if (Opened.Count < 1 || Steps == MaxStep)
            {
                OnFail();
                return;
            }
            Current = Opened[0];
            PushToClosed(Current);
            if (Current == End)
            {
                Steps++;
                OnSuccess();
                return;
            }
            foreach (Node node in environment.GetNeighbor(Current))
            {
                if (node == null || node.property.Closed || node.Difficulty > WalkabilityThreshold)
                {
                    continue;
                }
                if (node.property.Opened)
                {
                    int gValueNew = Heuristic.Calculate(Current, node);
                    if (gValueNew < node.property.GValue)
                    {
                        node.Parent = Current;
                        node.property.GValue = gValueNew;
                        node.SetCost(ref _estimator);
                    }
                }
                else
                {
                    PushToOpened(node, Current);
                }
            }
            Steps++;
            Status = STATUS.PROCESSING;
            return;
        }

        /// <summary>
        /// 检索路径, 直到寻路结束
        /// </summary>
        public void Find()
        {
            for (; Status == STATUS.PROCESSING;)
            {
                FindByStep();
            }
        }

        /// <summary>
        /// 重置路径
        /// </summary>
        public void Reset()
        {
            Status = STATUS.PROCESSING;
            Steps = 0;
            Opened = new List<Node>();
            Closed = new List<Node>();
            PushToOpened(Start, null);
        }
    }
}
