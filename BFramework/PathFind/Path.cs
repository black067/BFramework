using System;
using BFramework.ExpandedMath;
using System.Collections.Generic;

namespace BFramework.PathFind
{
    /// <summary>
    /// 路径类
    /// </summary>
    public class Path
    {
        Estimator<Attribute> _estimator;
        static DIRECTION[] _directions = { DIRECTION.LEFT, DIRECTION.RIGHT, DIRECTION.BACK, DIRECTION.FORWARD, DIRECTION.BOTTOM, DIRECTION.TOP };
        Dictionary<DIRECTION, bool> _directionState;
        List<Node> _availableNeighborsCurrent;


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
            Node start,
            Node end,
            int walkabilityThreshold,
            Attribute weightDictionary,
            Heuristic.TYPE heuristicType = Heuristic.TYPE.EUCLIDEAN,
            int maxStep = 100)
        {
            Status = STATUS.PROCESSING;
            Steps = 0;
            MaxStep = maxStep;
            WalkabilityThreshold = walkabilityThreshold;
            FulcrumRequirement = 4;
            Estimator = new Estimator<Attribute>(weightDictionary);
            Heuristic = new Heuristic(heuristicType);
            Start = start;
            End = end;
            Opened = new List<Node>();
            Closed = new List<Node>();
            Result = new List<Node>();
            PushToOpened(Start, null);

            _directionState = new Dictionary<DIRECTION, bool>();
            for (int i = _directions.Length - 1; i > -1; i--)
            {
                _directionState.Add(_directions[i], false);
            }
            _availableNeighborsCurrent = new List<Node>(26);
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
        /// 通行力阈值, 记录对 Node 的通行能力, 用于判断 Node 是否可以通过
        /// </summary>
        public int WalkabilityThreshold { get; set; }

        /// <summary>
        /// 估值器, 用于估计每个 Node 的消耗
        /// </summary>
        public Estimator<Attribute> Estimator { get { return _estimator; } private set { _estimator = value; } }
        
        /// <summary>
        /// 攀附能力, 数值越大, 对支撑的依赖越少
        /// </summary>
        public int FulcrumRequirement { get; set; }

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
        /// 当前节点的支撑节点
        /// </summary>
        public Node CurrentFulcrum { get; set; }

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
        /// 用于便捷访问估值器的权重值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int this[string key]
        {
            get { return Estimator.WeightItem[key]; }
            set { Estimator.WeightItem[key] = value; }
        }

        public void SetWeight(string key, int weight)
        {
            if (Estimator.WeightItem.Dictionary.ContainsKey(key))
            {
                Estimator.WeightItem[key] = weight;
            }
        }

        /// <summary>
        /// 检查节点是否可以作为支点
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static int CheckFulcrum(params Node[] nodes)
        {
            if(nodes != null)
            {
                for (int i = nodes.Length - 1; i > -1; i--)
                {
                    if (nodes[i] != null && nodes[i].Resistance > 0)
                        return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 检查相邻节点数组中是否存在支撑点
        /// </summary>
        /// <param name="neighbors"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static Node CheckFulcrums(Node node, int level)
        {
            if (level > 3) { return node; }
            else if (level < 1) { return null; }
            Node[] nodesForCheck = null;
            switch (level)
            {
                case 1:
                    nodesForCheck = new Node[] { node[DIRECTION.BOTTOM] };
                    break;
                case 2:
                    nodesForCheck = new Node[] {
                        node[DIRECTION.BOTTOM],
                        node[DIRECTION.BACK] ,
                        node[DIRECTION.FORWARD] ,
                        node[DIRECTION.LEFT] ,
                        node[DIRECTION.RIGHT] };
                    break;
                case 3:
                    nodesForCheck = new Node[] {
                        node[DIRECTION.BOTTOM],
                        node[DIRECTION.TOP],
                        node[DIRECTION.BACK] ,
                        node[DIRECTION.FORWARD] ,
                        node[DIRECTION.LEFT] ,
                        node[DIRECTION.RIGHT] };
                    break;
            }
            int i = CheckFulcrum(nodesForCheck);
            if (i >= 0)
            {
                return nodesForCheck[i];
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// 比较两个 Node 的开销
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns></returns>
        private int CompareByCost(Node node1, Node node2)
        {
            return node1.Cost.CompareTo(node2.Cost);
        }

        /// <summary>
        /// 将指定 Node 添加到开启列表中(需要设置其父节点)
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parent"></param>
        public void PushToOpened(Node node, Node parent)
        {
            Opened.Add(node);
            node.Opened = true;
            node.Parent = parent;
            node.GValue = Heuristic.Calculate(node, Start);
            node.HValue = Heuristic.Calculate(node, End);
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
            if (node.Opened)
            {
                Opened.Remove(node);
            }
            node.Closed = true;
            node.Opened = false;
        }

        /// <summary>
        /// 检查节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool CheckNode(Node node)
        {
            if (node == null || node.Closed || node.Difficulty > WalkabilityThreshold)
            {
                return false;
            }
            CurrentFulcrum = CheckFulcrums(node, FulcrumRequirement);
            if (CurrentFulcrum == null)
            {
                return false;
            }
            if (node.Opened)
            {
                int gValueNew = Heuristic.Calculate(node, Start);
                if (gValueNew < node.GValue)
                {
                    node.Parent = Current;
                    node.GValue = gValueNew;
                    node.SetCost(ref _estimator);
                }
            }
            else
            {
                PushToOpened(node, Current);
            }
            return true;
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
            int count = Closed.Count;
            End.Parent = End.Parent ?? Closed[count - 2];
            Result = new List<Node>(count)
            {
                End
            };
            for (int i = 1; i <= count && Result[i - 1].Parent != null; i++)
            {
                Result.Add(Result[i - 1].Parent);
            }
        }

        /// <summary>
        /// 检查节点是否可以通行
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool CompareDifficulty(Node node)
        {
            return node != null && node.Difficulty < WalkabilityThreshold;
        }

        /// <summary>
        /// 对于指定节点, 获取其可以通行的相邻节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public List<Node> GetAvailableNeighbors(Node node)
        {
            _availableNeighborsCurrent = new List<Node>(26);
            DIRECTION directionI, directionJ;
            for (int i = _directions.Length - 1; i > -1; i--)
            {
                directionI = _directions[i];
                if (CompareDifficulty(node[directionI]))
                {
                    _availableNeighborsCurrent.Add(node[directionI]);
                }
            }
            for (int i = _directions.Length - 1; i > -1; i--)
            {
                directionI = _directions[i];
                for (int j = _directions.Length - 1; j > -1; j--)
                {
                    directionJ = _directions[j];
                    if (directionJ == directionI || (int)directionJ == -(int) directionI)
                    {
                        continue;
                    }
                    if ((CompareDifficulty(node[directionI]) || CompareDifficulty(node[directionJ])) && 
                        CompareDifficulty(node[directionI, directionJ]))
                    {
                        _availableNeighborsCurrent.Add(node[directionI, directionJ]);
                    }
                }
            }
            return _availableNeighborsCurrent;
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

            foreach (Node node in GetAvailableNeighbors(Current))
            {
                if (!CheckNode(node))
                {
                    continue;
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
            foreach (Node node in Closed)
            {
                node.Closed = false;
                node.Opened = false;
            }
            foreach (Node node in Opened)
            {
                node.Closed = false;
                node.Opened = false;
            }
            Opened = new List<Node>();
            Closed = new List<Node>();
            PushToOpened(Start, null);
        }
        
        /// <summary>
        /// 设置起点
        /// </summary>
        /// <param name="start"></param>
        public void SetStart(Node start)
        {
            if (Start != null)
            {
                Start.Closed = false;
                Start.Opened = false;
            }
            Start = start;
            Reset();
        }

        /// <summary>
        /// 设置终点
        /// </summary>
        /// <param name="end"></param>
        public void SetEnd(Node end)
        {
            End = end;
        }
    }
}
