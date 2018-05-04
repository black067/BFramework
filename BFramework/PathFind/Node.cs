using System;
using System.Collections.Generic;
using BFramework.ExpandedMath;

namespace BFramework.PathFind
{
    /// <summary>
    /// 节点类
    /// </summary>
    [Serializable]
    public class Node
    {
        /// <summary>
        /// 根据给定的通行难度与坐标建立节点
        /// </summary>
        /// <param name="difficulty"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Node(int difficulty, int x, int y, int z)
        {
            _attribute = new Attribute();
            _attribute["DIFFICULTY"] = difficulty;
            
            X = x;
            Y = y;
            Z = z;
        }
        
        /// <summary>
        /// 节点的属性
        /// </summary>
        private Attribute _attribute { get; set; }
        
        /// <summary>
        /// 节点的所有相邻节点
        /// </summary>
        public Node[,,] Neighbors { get; set; }

        /// <summary>
        /// 节点在 X 轴上的坐标
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// 节点在 Y 轴上的坐标
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// 节点在 Z 轴上的坐标
        /// </summary>
        public int Z { get; set; }

        private void SwitchDirection(ref DIRECTION direction, ref int x, ref int y, ref int z)
        {
            switch (direction)
            {
                case DIRECTION.LEFT:
                    x = 0;
                    break;
                case DIRECTION.RIGHT:
                    x = 2;
                    break;
                case DIRECTION.BOTTOM:
                    y = 0;
                    break;
                case DIRECTION.TOP:
                    y = 2;
                    break;
                case DIRECTION.BACK:
                    z = 0;
                    break;
                case DIRECTION.FORWARD:
                    z = 2;
                    break;
            }
        }

        public Node this[DIRECTION direction]
        {
            get
            {
                int i = 1, j = 1, k = 1;
                SwitchDirection(ref direction, ref i, ref j, ref k);
                return Neighbors[i, j, k];
            }
        }

        public Node this[DIRECTION direction0, DIRECTION direction1]
        {
            get
            {
                int i = 1, j = 1, k = 1;
                SwitchDirection(ref direction0, ref i, ref j, ref k);
                if (direction1 != direction0)
                {
                    if ((int)direction0 == -(int)direction1)
                    {
                        i = 1; j = 1; k = 1;
                    }
                    else
                    {
                        SwitchDirection(ref direction1, ref i, ref j, ref k);
                    }
                }
                return Neighbors[i, j, k];
            }
        }

        public int this[string key]
        {
            get { return _attribute[key]; }
            set { _attribute[key] = value; }
        }

        /// <summary>
        /// 节点是否处于 Opened 列表中
        /// </summary>
        public bool Opened { get { return _attribute.Opened; } set { _attribute.Opened = value; } }

        /// <summary>
        /// 节点是否处于 Closed 列表中
        /// </summary>
        public bool Closed { get { return _attribute.Closed; } set { _attribute.Closed = value; } }
        
        /// <summary>
        /// 节点的父节点
        /// </summary>
        public Node Parent { get { return _attribute.Parent; } set { _attribute.Parent = value; } }

        /// <summary>
        /// 通过该节点的开销
        /// </summary>
        public int Cost { get { return _attribute.Cost; } set { _attribute.Cost = value; } }

        /// <summary>
        /// 该节点的通行难度
        /// </summary>
        public int Difficulty { get { return _attribute["DIFFICULTY"]; } set { _attribute["DIFFICULTY"] = value; } }

        /// <summary>
        /// 该节点到其父节点的距离估值
        /// </summary>
        public int GValue { get { return _attribute["GVALUE"]; } set { _attribute["GVALUE"] = value; } }

        /// <summary>
        /// 该节点到目标节点的距离估值
        /// </summary>
        public int HValue { get { return _attribute["HVALUE"]; } set { _attribute["HVALUE"] = value; } }

        /// <summary>
        /// 该节点的通行阻力
        /// </summary>
        public int Resistance { get { return _attribute["RESISTANCE"]; } set { _attribute["RESISTANCE"] = value; } }

        /// <summary>
        /// 该节点处的温度
        /// </summary>
        public int Temperature { get { return _attribute["TEMPERATURE"]; } set { _attribute["TEMPERATURE"] = value; } }
        
        /// <summary>
        /// 设置该节点的通行开销, 需要传入一个对 PathFind.Property 类的估值器( ExpandedMath.Estimator 类)
        /// </summary>
        /// <param name="estimator"></param>
        public void SetCost(ref Estimator<Attribute> estimator)
        {
            _attribute.Cost = estimator[_attribute];
        }

        /// <summary>
        /// 节点类输出为字符串的方法重载
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Node(X: {1}, Y: {2}, Z: {3}, Difficulty: {0:D3})", Difficulty, X, Y, Z);
        }
    }
}
