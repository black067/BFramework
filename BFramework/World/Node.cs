﻿using System;
namespace BFramework.World
{
    /// <summary>
    /// 节点类
    /// </summary>
    [System.Serializable]
    public class Node : IComparable<Node>
    {
        /// <summary>
        /// 根据给定的通行难度与坐标建立节点
        /// </summary>
        /// <param name="difficulty"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Node(int x, int y, int z, Properties properties)
        {
            _properties = properties;
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// 节点的属性
        /// </summary>
        private Properties _properties
        {
            get; set;
        }

        /// <summary>
        /// 节点的所有相邻节点
        /// </summary>
        public Node[,,] Neighbors;

        /// <summary>
        /// 节点在 X 轴上的坐标
        /// </summary>
        public int X;

        /// <summary>
        /// 节点在 Y 轴上的坐标
        /// </summary>
        public int Y;

        /// <summary>
        /// 节点在 Z 轴上的坐标
        /// </summary>
        public int Z;

        /// <summary>
        /// 通过给定的方向, 调整坐标的指针
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        private void SwitchDirection(ref DIRECTION direction, ref int x, ref int y, ref int z)
        {
            switch (direction)
            {
                case DIRECTION.LEFT:
                    x -= x > 0 ? 1 : 0;
                    break;
                case DIRECTION.RIGHT:
                    x += x < 2 ? 1 : 0;
                    break;
                case DIRECTION.BOTTOM:
                    y -= y > 0 ? 1 : 0;
                    break;
                case DIRECTION.TOP:
                    y += y < 2 ? 1 : 0;
                    break;
                case DIRECTION.BACK:
                    z -= z > 0 ? 1 : 0;
                    break;
                case DIRECTION.FORWARD:
                    z += z < 2 ? 1 : 0;
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
                SwitchDirection(ref direction1, ref i, ref j, ref k);
                return Neighbors[i, j, k];
            }
        }

        public Node this[DIRECTION direction0, DIRECTION direction1, DIRECTION direction2]
        {
            get
            {
                int i = 1, j = 1, k = 1;
                SwitchDirection(ref direction0, ref i, ref j, ref k);
                SwitchDirection(ref direction1, ref i, ref j, ref k);
                SwitchDirection(ref direction2, ref i, ref j, ref k);
                return Neighbors[i, j, k];
            }
        }

        public Node this[DIRECTION[] directions]
        {
            get
            {
                int x = 1, y = 1, z = 1;
                for (int i = 0, length = directions.Length; i < length; i++)
                {
                    SwitchDirection(ref directions[i], ref x, ref y, ref z);
                }
                return Neighbors[x, y, z];
            }
        }

        public double this[string key]
        {
            get
            {
                return _properties[key];
            }
            set
            {
                _properties[key] = value;
            }
        }
        
        /// <summary>
        /// 节点的类型值
        /// </summary>
        public string Type
        {
            get
            {
                return _properties.NodeType;
            }
            set
            {
                _properties.NodeType = value;
            }
        }

        /// <summary>
        /// 通过该节点的开销
        /// </summary>
        public double Cost
        {
            get
            {
                return _properties.Cost;
            }
            set
            {
                _properties.Cost = value;
            }
        }

        /// <summary>
        /// 该节点的通行难度
        /// </summary>
        public double Difficulty
        {
            get
            {
                return _properties[Default.Properties.Keys.Difficulty];
            }
            set
            {
                _properties[Default.Properties.Keys.Difficulty] = value;
            }
        }

        /// <summary>
        /// 该节点到其父节点的距离估值
        /// </summary>
        public double GValue
        {
            get
            {
                return _properties[Default.Properties.Keys.GValue];
            }
            set
            {
                _properties[Default.Properties.Keys.GValue] = value;
            }
        }

        /// <summary>
        /// 该节点到目标节点的距离估值
        /// </summary>
        public double HValue
        {
            get
            {
                return _properties[Default.Properties.Keys.HValue];
            }
            set
            {
                _properties[Default.Properties.Keys.HValue] = value;
            }
        }

        /// <summary>
        /// 该节点的通行阻力
        /// </summary>
        public double Friction
        {
            get
            {
                return _properties[Default.Properties.Keys.Friction];
            }
            set
            {
                _properties[Default.Properties.Keys.Friction] = value;
            }
        }

        /// <summary>
        /// 设置该节点的通行开销, 需要传入一个对 PathFind.Properties 类的估值器( ExpandedMath.Estimator 类)
        /// </summary>
        /// <param name="estimator"></param>
        public void SetCost(ref ExpandedMath.Estimator<Properties> estimator)
        {
            _properties.Cost = estimator.Calculate(_properties);
        }

        public void SetProerties(Properties properties)
        {
            Type = properties.NodeType;
            foreach (string key in properties.Keys)
            {
                _properties[key] = properties[key];
            }
        }

        public Properties GetProperties()
        {
            return _properties;
        }

        /// <summary>
        /// 节点类输出为字符串的方法重载
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Node({0}, {1}, {2})", X, Y, Z);
        }

        public int CompareTo(Node other)
        {
            return Cost.CompareTo(other.Cost);
        }
    }
}
