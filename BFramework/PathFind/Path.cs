using System;
using BFramework.ExpandedMath;
using BFramework.World;
using System.Collections.Generic;
using System.Linq;

namespace BFramework.PathFind
{
    /// <summary>
    /// 用于路径规划的路径类, 进行路径规划时调用其 Find() 方法
    /// </summary>
    public class Path
    {
        /// <summary>
        /// 用于记录工作状态的枚举
        /// </summary>
        public enum STATE
        {
            FAIL = -1,
            PROCESSING = 0,
            SUCCESS = 1,
        }
        
        /// <summary>
        /// 用于记录节点状态的枚举
        /// </summary>
        public enum NODESTATE
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

        /// <summary>
        /// 记录节点与其父节点关系的字典
        /// </summary>
        public Dictionary<Node, Node> Parents;

        /// <summary>
        /// 记录节点状态的字典
        /// </summary>
        public Dictionary<Node, NODESTATE> NodeStates;

        /// <summary>
        /// 当前节点的有效邻节点指针
        /// </summary>
        private List<Node> _availableNeighborsCurrent;
        
        /// <summary>
        /// 估值器, 用于估计每个 Node 的消耗
        /// </summary>
        public Estimator<Properties> Estimator;

        /// <summary>
        /// 地图是否为静态
        /// </summary>
        public bool MapStatic;

        /// <summary>
        /// 可用的邻节点字典，仅当地图为静态时使用
        /// </summary>
        private Dictionary<Node, List<Node>> _availableNeighborsDictionary;

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
        public int Steps;

        /// <summary>
        /// 代理
        /// </summary>
        public Agent Agent;

        /// <summary>
        /// 路径的花费, 是整个路径中所有 Node 的 Cost 之和
        /// </summary>
        public float Cost;

        /// <summary>
        /// 起点
        /// </summary>
        public Node Start;

        /// <summary>
        /// 终点
        /// </summary>
        public Node End;

        /// <summary>
        /// 当前检测到的 Node
        /// </summary>
        public Node Current;

        /// <summary>
        /// 当前节点的支撑节点
        /// </summary>
        public Node CurrentFulcrum;

        /// <summary>
        /// 待检测的 Node 列表
        /// </summary>
        public List<Node> Opened;

        /// <summary>
        /// 检测完毕的 Node 列表
        /// </summary>
        public List<Node> Closed;

        /// <summary>
        /// 路径检索的最终结果
        /// </summary>
        public List<Node> Result
        {
            get; private set;
        }

        /// <summary>
        /// 工作状态
        /// </summary>
        public STATE State;

        /// <summary>
        /// 访问估值器的权重值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public double this[string key]
        {
            get
            {
                return Estimator.WeightItem[key];
            }
            set
            {
                Estimator.WeightItem[key] = value;
            }
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
        public static Node GetFulcrum(Agent agent, Node node, params DIRECTION[] directions)
        {
            if (node != null)
            {
                Node result;
                int tempIndex = -1;
                double tempFriction = double.MaxValue;
                for (int i = directions.Length - 1; i > -1; i--)
                {
                    result = node[directions[i]];
                    if (result != null && agent.BeAbleToStand(result) && result.Friction < tempFriction)
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
            return r == 0 ? CompareByAngle(node, other) : r;
        }

        public Node GetParent(Node child)
        {
            return (child != null && Parents.ContainsKey(child)) ? Parents[child] : null;
        }

        public void SetParent(Node node, Node parent)
        {
            if (Parents.ContainsKey(node))
            {
                Parents[node] = parent;
            }
            else
            {
                Parents.Add(node, parent);
            }
        }

        private NODESTATE GetState(Node node)
        {
            if (!NodeStates.ContainsKey(node))
            {
                NodeStates.Add(node, NODESTATE.NONE);
            }
            return NodeStates[node];
        }

        private void BinaryInsert(List<Node> list, Node item)
        {
            int startIndex = 0;
            int endIndex = list.Count - 1;
            int currentIndex = 0;
            int compareResult;
            for (; startIndex <= endIndex; )
            {
                currentIndex = (startIndex + endIndex) / 2;
                compareResult = CompareCost(list[currentIndex], item);
                if (compareResult == 0)
                {
                    break;
                }
                else if (compareResult < 0)
                {
                    //起点后移
                    startIndex = currentIndex + 1;
                }
                else
                {
                    //终点前移
                    endIndex = currentIndex - 1;
                }

            }
            list.Insert(startIndex <= endIndex ? endIndex : currentIndex, item);
        }

        /// <summary>
        /// 将指定 Node 添加到开启列表中(需要设置其父节点)
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parent"></param>
        public void PushToOpenset(Node node, Node parent)
        {
            if (NodeStates.ContainsKey(node))
            {
                NodeStates[node] = NODESTATE.OPEN;
            }
            else
            {
                NodeStates.Add(node, NODESTATE.OPEN);
            }
            SetParent(node, parent);
            node.GValue = Agent.Compute(node, parent) + (parent == null ? 0 : parent.GValue);
            node.HValue = Agent.Compute(node, End);
            node[Default.Properties.Keys.DynamicWeight] = node.HValue * Steps / Agent.StepsLimit;
            node.SetCost(ref Estimator);

            if (Opened.Count < 1)
            {
                Opened.Add(node);
            }
            else
            {
                BinaryInsert(Opened, node);
            }
        }

        /// <summary>
        /// 将指定 Node 添加到关闭列表中
        /// </summary>
        /// <param name="node"></param>
        public void PushToClosed(Node node)
        {
            if (NodeStates.ContainsKey(node))
            {
                Opened.Remove(node);
                NodeStates[node] = NODESTATE.CLOSED;
            }
            else
            {
                NodeStates.Add(node, NODESTATE.CLOSED);
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
            if (GetState(node) == NODESTATE.CLOSED || !Agent.BeAbleToPass(node))
            {
                return;
            }

            if (Agent.ClimblingAbility != Agent.CLIMBLINGABILITY.EXTREME)
            {
                CurrentFulcrum = GetFulcrum(Agent, node, Agent.ClimblingRequirements);
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

            if (GetState(node) == NODESTATE.OPEN)
            {
                double gValueLocal = Agent.Compute(node, Parents[node]);
                double gValueNew = Agent.Compute(node, Current);
                if (gValueNew < gValueLocal)
                {
                    SetParent(node, Current);
                    node.GValue = Current.GValue + gValueNew;
                }
            }
            else
            {
                PushToOpenset(node, Current);
            }
            return;
        }

        /// <summary>
        /// 失败时执行动作
        /// </summary>
        public void OnFail()
        {
            State = STATE.FAIL;
        }

        /// <summary>
        /// 成功时执行动作
        /// </summary>
        public void OnSuccess()
        {
            State = STATE.SUCCESS;
            int count = Closed.Count;
            if (!Parents.ContainsKey(End))
            {
                SetParent(End, Closed[count - 2]);
            }
            Result = new List<Node>(count)
            {
                End
            };
            Node node = End;
            for (int i = 1; i <= count && Parents[node] != null; i++)
            {
                node = Result[i - 1];
                Result.Add(Parents[node]);
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

            if (Agent.ClimblingAbility == Agent.CLIMBLINGABILITY.WEAK)
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
            if (!containsNode)
                _availableNeighborsDictionary.Add(node, _availableNeighborsCurrent);
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
            State = STATE.PROCESSING;
            return;
        }

        /// <summary>
        /// 检索路径, 直到寻路结束
        /// </summary>
        public STATE Find()
        {
            for (; State == STATE.PROCESSING;)
            {
                FindSync();
            }
            return State;
        }

        /// <summary>
        /// 检索路径, 且在每一步检索之后执行动作, 直到寻路结束
        /// </summary>
        /// <param name="action"></param>
        public STATE Find(BDelegate action)
        {
            for (; State == STATE.PROCESSING;)
            {
                FindSync();
                action.Execute();
            }
            return State;
        }

        public double GetResultLength()
        {
            if (State != STATE.SUCCESS)
            {
                return 0;
            }
            double length = 0;
            Node current = Result[0], parent = Result[1];
            bool xEqual, yEqual, zEqual;
            for (; parent != null;)
            {
                xEqual = current.X == parent.X;
                yEqual = current.Y == parent.Y;
                zEqual = current.Z == parent.Z;
                int b = (xEqual ? 1 : 0) + (yEqual ? 2 : 0) + (zEqual ? 4 : 0);
                length += (b == 3 || b == 5 || b == 6) ? 1 : Heuristic.sqrt2;
                current = parent;
                parent = GetParent(current);
            }
            return length;
        }

        /// <summary>
        /// 重置/初始化路径
        /// </summary>
        public void Reset()
        {
            State = STATE.PROCESSING;
            Steps = 0;
            NodeStates = new Dictionary<Node, NODESTATE>();
            Parents = new Dictionary<Node, Node>();
            Opened = new List<Node>();
            Closed = new List<Node>();
            Result = new List<Node>();
            if (!MapStatic)
            {
                _availableNeighborsDictionary = new Dictionary<Node, List<Node>>();
            }
            PushToOpenset(Start, null);
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

        /// <summary>
        /// 将节点坐标转化为整数三维向量
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static VectorInt NodeAsVector(Node node)
        {
            return new VectorInt(node.X, node.Y, node.Z);
        }

        /// <summary>
        /// 比较两个节点之间的角度
        /// </summary>
        /// <param name="node"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        private int CompareByAngle(Node node, Node other)
        {
            VectorInt endPosition = NodeAsVector(End);
            VectorInt startPosition = NodeAsVector(Start);
            VectorInt startToEnd = endPosition - startPosition;
            float startToEndLength = startToEnd.Magnitude;

            VectorInt startToNode = NodeAsVector(node) - startPosition;
            VectorInt startToOther = NodeAsVector(other) - startPosition;
            double cosThita0 = (startToNode * startToEnd) / (startToNode.Magnitude * startToEndLength);
            double cosThita1 = (startToOther * startToEnd) / (startToOther.Magnitude * startToEndLength);
            return -cosThita0.CompareTo(cosThita1);
        }
    }
}
