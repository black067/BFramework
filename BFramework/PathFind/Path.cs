using System;
using BFramework.ExpandedMath;
using BFramework.DataStructure;
using BFramework.World;
using System.Collections.Generic;

namespace BFramework.PathFind
{
    /// <summary>
    /// 路径类
    /// </summary>
    public class Path
    {
        private enum STATE
        {
            CLOSED = -1,
            NONE = 0,
            OPEN = 1,
        }

        private readonly static DIRECTION[] _directions = {
            DIRECTION.LEFT,
            DIRECTION.RIGHT,
            DIRECTION.BACK,
            DIRECTION.FORWARD,
            DIRECTION.BOTTOM,
            DIRECTION.TOP
        };
        
        private Dictionary<Node, Node> _nodeParent { get; set; }
        private List<Node> _availableNeighborsCurrent { get; set; }
        private Estimator<Properties> _estimator;
        private Dictionary<Node, List<Node>> _availableNeighborsDictionary { get; set; }
        private Dictionary<Node, STATE> _nodeStates { get; set; }
        
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
            Agent agent,
            bool mapStatic = false)
        {
            Start = start;
            End = end;
            Agent = agent;
            Estimator = new Estimator<Properties>(agent.WeightTable);
            MapStatic = mapStatic;
            Reset();
        }

        /// <summary>
        /// 记录当前步数
        /// </summary>
        public int Steps { get; set; }
        
        /// <summary>
        /// 估值器, 用于估计每个 Node 的消耗
        /// </summary>
        public Estimator<Properties> Estimator { get { return _estimator; } private set { _estimator = value; } }
        
        public Agent Agent { get; set; }

        public bool MapStatic { get; set; }

        /// <summary>
        /// 路径的花费, 是整个路径中所有 Node 的 Cost 之和
        /// </summary>
        public float Cost { get; set; }
        
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
        public List<Node> Opened;

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
        /// 访问估值器的权重值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public double this[string key]
        {
            get { return Estimator.WeightItem[key]; }
            set { Estimator.WeightItem[key] = value; }
        }

        /// <summary>
        /// 根据键值设置估值器相应的权重值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void SetWeight(string key, int weight)
        {
            if (Estimator.WeightItem.ContainsKey(key))
            {
                Estimator.WeightItem[key] = weight;
            }
        }

        /// <summary>
        ///  检查节点是否可以作为支点(需要给出检查的方位)
        /// </summary>
        /// <param name="node"></param>
        /// <param name="directions"></param>
        /// <returns></returns>
        public static Node GetFulcrum(Node node, params DIRECTION[] directions)
        {
            if (node != null)
            {
                Node result;
                int tempIndex = -1;
                double tempFriction = double.MaxValue;
                for (int i = directions.Length - 1; i > -1; i--)
                {
                    result = node[directions[i]];
                    if (result != null && result.Friction > 0 && result.Friction < tempFriction)
                    {
                        tempFriction = result.Friction;
                        tempIndex = i;
                    }
                }
                if (tempIndex >= 0)
                {
                    return node[directions[tempIndex]];
                }
            }
            return null;
        }

        /// <summary>
        /// 比较两个 Node 的开销
        /// </summary>
        /// <param name="node"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        private int CompareCost(Node node, Node other)
        {
            int r = node.CompareTo(other);
            r = r == 0 ? CompareByAngle(node, other) : r;
            return r;
        }

        public Node GetParent(Node child)
        {
            return (child != null && _nodeParent.ContainsKey(child)) ? _nodeParent[child] : null;
        }

        public void SetParent(Node node, Node parent)
        {
            if (_nodeParent.ContainsKey(node))
            {
                _nodeParent[node] = parent;
            }
            else
            {
                _nodeParent.Add(node, parent);
            }
        }

        private STATE GetState(Node node)
        {
            if (!_nodeStates.ContainsKey(node))
            {
                _nodeStates.Add(node, STATE.NONE);
            }
            return _nodeStates[node];
        }

        private bool OpenedIsEmpty(List<Node> list)
        {
            return list.Count < 1;
        }

        /// <summary>
        /// 将指定 Node 添加到开启列表中(需要设置其父节点)
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parent"></param>
        public void PushToOpened(Node node, Node parent)
        {
            if (_nodeStates.ContainsKey(node))
            {
                _nodeStates[node] = STATE.OPEN;
            }
            else
            {
                _nodeStates.Add(node, STATE.OPEN);
            }
            SetParent(node, parent);
            node.GValue = Agent.Compute(node, Start);
            node.HValue = Agent.Compute(node, End);
            node.SetCost(ref _estimator);
            Opened.Add(node);
            Opened.Sort(CompareCost);
        }

        /// <summary>
        /// 将指定 Node 添加到关闭列表中
        /// </summary>
        /// <param name="node"></param>
        public void PushToClosed(Node node)
        {
            if (_nodeStates.ContainsKey(node))
            {
                Opened.Remove(node);
                _nodeStates[node] = STATE.CLOSED;
            }
            else
            {
                _nodeStates.Add(node, STATE.CLOSED);
            }

            Closed.Add(node);
        }

        /// <summary>
        /// 检查节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public void CheckNode(Node node)
        {
            if (GetState(node) == STATE.CLOSED || !Agent.BeAbleToPass(node))
            {
                return;
            }

            if(Agent.ClimblingAbility != Agent.CLIMBLINGABILITY.EXTREME)
            {
                CurrentFulcrum = GetFulcrum(node, Agent.ClimblingRequirements);
                if (CurrentFulcrum == null)
                {
                    return;
                }
                node[Default.Properties.Keys.Resistance] = CurrentFulcrum.Friction;
            }
            else
            {
                node[Default.Properties.Keys.Resistance] = 0;
            }
            node[Default.Properties.Keys.Resistance] += node.Difficulty / 10;
            //if (node.Y - Current.Y != 0) node[Default.Properties.Keys.Resistance] += 5;
            //if (node.X - Current.X != 0) node[Default.Properties.Keys.Resistance] += 5;
            //if (node.Z - Current.Z != 0) node[Default.Properties.Keys.Resistance] += 5;

            if (GetState(node) == STATE.OPEN)
            {
                double gValueLocal = Agent.Compute(node, _nodeParent[node]);
                double gValueNew = Agent.Compute(node, Current);
                if (gValueNew < gValueLocal)
                {
                    SetParent(node, Current);
                }
            }
            else
            {
                PushToOpened(node, Current);
            }
            return;
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
            if (!_nodeParent.ContainsKey(End))
            {
                SetParent(End, Closed[count - 2]);
            }
            Result = new List<Node>(count)
            {
                End
            };
            Node node = End;
            for (int i = 1; i <= count && _nodeParent[node] != null; i++)
            {
                node = Result[i - 1];
                Result.Add(_nodeParent[node]);
            }
        }

        /// <summary>
        /// 检查节点是否可以通行
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool CompareDifficulty(Node node)
        {
            return node != null && Agent.BeAbleToPass(node);
        }

        /// <summary>
        /// 对于指定节点, 获取其可以通行的相邻节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public List<Node> GetAvailableNeighbors(Node node)
        {
            bool containsNode = _availableNeighborsDictionary.ContainsKey(node);
            if (MapStatic && containsNode)
            {
                return _availableNeighborsDictionary[node];
            }
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

            if(Agent.ClimblingAbility == Agent.CLIMBLINGABILITY.WEAK)
            {
                return _availableNeighborsCurrent;
            }

            for (int i = _directions.Length - 1; i > -1; i--)
            {
                directionI = _directions[i];
                for (int j = _directions.Length - 1; j > -1; j--)
                {
                    directionJ = _directions[j];
                    if (directionJ == directionI || (int)directionJ == -(int)directionI)
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
            if(!containsNode) _availableNeighborsDictionary.Add(node, _availableNeighborsCurrent);
            return _availableNeighborsCurrent;
        }

        /// <summary>
        /// 按步检索路径
        /// </summary>
        /// <returns></returns>
        public void FindSync()
        {
            if (Opened.Count < 1 || Steps >= Agent.StepsLimit)
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
            GetAvailableNeighbors(Current);
            foreach (Node node in _availableNeighborsCurrent)
            {
                CheckNode(node);
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
                FindSync();
            }
        }

        /// <summary>
        /// 检索路径, 且在每一步检索之后执行动作, 直到寻路结束
        /// </summary>
        /// <param name="action"></param>
        public void Find(BDelegate action)
        {
            for (; Status == STATUS.PROCESSING;)
            {
                FindSync();
                action.Execute();
            }
        }

        /// <summary>
        /// 重置路径
        /// </summary>
        public void Reset()
        {
            Status = STATUS.PROCESSING;
            Steps = 0;
            _nodeStates = new Dictionary<Node, STATE>();
            _nodeParent = new Dictionary<Node, Node>();
            Opened = new List<Node>();
            Closed = new List<Node>();
            Result = new List<Node>();
            if (!MapStatic)
            {
                _availableNeighborsDictionary = new Dictionary<Node, List<Node>>();
            }
            PushToOpened(Start, null);
        }

        /// <summary>
        /// 设置起点
        /// </summary>
        /// <param name="start"></param>
        public void SetStart(Node start)
        {
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

        private static VectorInt NodeAsVector(Node node)
        {
            return new VectorInt(node.X, node.Y, node.Z);
        }
        
        private int CompareByAngle(Node node, Node other)
        {
            VectorInt endPosition = NodeAsVector(End);
            VectorInt startPosition = NodeAsVector(Start);
            VectorInt startToEnd = endPosition - startPosition;
            float startToEndLength = startToEnd.Magnitude;

            VectorInt thisToEnd = endPosition - NodeAsVector(node);
            VectorInt otherToEnd = endPosition - NodeAsVector(other);
            double cosThita0 = (thisToEnd * startToEnd) / (thisToEnd.Magnitude * startToEndLength);
            double cosThita1 = (otherToEnd * startToEnd) / (otherToEnd.Magnitude * startToEndLength);
            return -cosThita0.CompareTo(cosThita1);
        }
    }
}
