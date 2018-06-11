using System;
using System.Collections.Generic;
using BFramework.Tools;

namespace BFramework.DataStructure
{
    public class BinaryTree<T> where T : IComparable<T>
    {
        public class Node : IComparable<Node>
        {
            public enum STATE_POSITIVE
            {
                LEAF = 0,
                LEFTONLY = -1,
                RIGHTONLY = 1,
                FILLED = 2,
            }

            public enum STATE_NEGATIVE
            {
                ROOT = 0,
                LEFTCHILD = -1,
                RIGHYCHILD = 1,
            }

            public T Data { get; set; }

            public Node Parent { get; set; }

            public Node Left { get; set; }

            public Node Right { get; set; }

            private bool _hasLeftChild { get { return Left != null; } }
            private bool _hasRightChild { get { return Right != null; } }
            private bool _hasParent { get { return Parent != null; } }

            public STATE_NEGATIVE State_N
            {
                get
                {
                    if (_hasParent)
                    {
                        if(Parent.Left == this) { return STATE_NEGATIVE.LEFTCHILD; }
                        else { return STATE_NEGATIVE.RIGHYCHILD; }
                    }
                    else { return STATE_NEGATIVE.ROOT; }
                }
            }

            public STATE_POSITIVE State_P
            {
                get
                {
                    if (_hasLeftChild)
                    {
                        if (_hasRightChild)
                        {
                            return STATE_POSITIVE.FILLED;
                        }
                        else
                        {
                            return STATE_POSITIVE.LEFTONLY;
                        }
                    }
                    else if (_hasRightChild)
                    {
                        return STATE_POSITIVE.RIGHTONLY;
                    }
                    else
                    {
                        return STATE_POSITIVE.LEAF;
                    }
                }
            }

            public Node(T item)
            {
                Data = item;
            }

            public int CompareTo(Node other)
            {
                return Data.CompareTo(other.Data);
            }

            public Node SetParent(Node parent, bool AsLeftChild)
            {
                Node originalChild;
                if (AsLeftChild)
                {
                    originalChild = parent.Left;
                    parent.Left = this;
                }
                else
                {
                    originalChild = parent.Right;
                    parent.Right = this;
                }
                Parent = parent;
                return originalChild;
            }

            public Node GetMin()
            {
                Node min = this;
                for (; min.Left != null;)
                {
                    min = min.Left;
                }
                return min;
            }

            public Node GetMax()
            {
                Node max = this;
                for (; max != null;)
                {
                    max = max.Right;
                }
                return max;
            }

            public Node GetSuccessor()
            {
                if (Right == null) { return null; }
                Node successor = Right;
                for (; successor.Left != null;)
                {
                    successor = successor.Left;
                }
                return successor;
            }

            public Node GetForerunner()
            {
                if (Left == null) { return null; }
                Node forerunner = Left;
                for(; forerunner.Right != null;)
                {
                    forerunner = forerunner.Right;
                }
                return forerunner;
            }
        }

        public Node Root { get; set; }

        public BinaryTree(T item)
        {
            Add(item);
        }

        public BinaryTree()
        {

        }

        public void Add(T item)
        {
            Node node = new Node(item);
            if (Root == null) { Root = node; }
            else
            {
                Node temp = Root;
                int compareResult;
                for (; ; )
                {
                    compareResult = temp.CompareTo(node);
                    if (compareResult == 0)
                    {
                        return;
                    }
                    else if (compareResult > 0)
                    {
                        if (temp.Left != null) { temp = temp.Left; }
                        else { node.SetParent(temp, true); break; }
                    }
                    else
                    {
                        if (temp.Right != null) { temp = temp.Right; }
                        else { node.SetParent(temp, false); break; }
                    }
                }
            }
        }

        /// <summary>
        /// 先序遍历, 将结果保存到堆栈中
        /// </summary>
        /// <param name="node"></param>
        /// <param name="result"></param>
        public void PreOrder(Node node, ref Stack<T> result)
        {
            if(node == null)
            {
                return;
            }
            result.Push(node.Data);
            PreOrder(node.Left, ref result);
            PreOrder(node.Right, ref result);
        }

        /// <summary>
        /// 中序遍历, 将结果保存到堆栈中
        /// </summary>
        /// <param name="node"></param>
        /// <param name="result"></param>
        public void InOrder(Node node, ref Stack<T> result)
        {
            if (node == null)
            {
                return;
            }
            InOrder(node.Left, ref result);
            result.Push(node.Data);
            InOrder(node.Right, ref result);
        }

        /// <summary>
        /// 后序遍历, 将结果保存到堆栈中
        /// </summary>
        /// <param name="node"></param>
        /// <param name="result"></param>
        public void PostOrder(Node node, ref Stack<T> result)
        {
            if (node == null)
            {
                return;
            }
            PostOrder(node.Left, ref result);
            PostOrder(node.Right, ref result);
            result.Push(node.Data);
        }

        /// <summary>
        /// 查找最小值
        /// </summary>
        /// <returns></returns>
        public Node GetMin()
        {
            Node min = Root;
            if (min == null) return null;
            for (; min.Left != null;)
            {
                min = min.Left;
            }
            return min;
        }

        /// <summary>
        /// 查找最大值
        /// </summary>
        /// <returns></returns>
        public Node GetMax()
        {
            Node max = Root;
            if (max == null) return null;
            for (; max.Right != null;)
            {
                max = max.Right;
            }
            return max;
        }

        /// <summary>
        /// 根据键查找节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Node Find(T key)
        {
            Node current = Root;
            int compareResult;
            for (; current != null;)
            {
                compareResult = current.Data.CompareTo(key);
                if (compareResult == 0)
                {
                    break;
                }
                else if (compareResult > 0)
                {
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }
            return current;
        }

        public static string GetState(Node node)
        {
            return string.Format("My positive state : {0}, negative state : {1}", node.State_P, node.State_N);
        }

        public void Remove(T key)
        {
            Node node = Find(key);
            if (node == null) return;
            Remove(node);
        }

        public void Remove(Node node)
        {
            Node parent = node.Parent;
            switch (node.State_P)
            {
                case Node.STATE_POSITIVE.LEAF:
                    switch (node.State_N)
                    {
                        case Node.STATE_NEGATIVE.ROOT:
                            Root = null;
                            break;
                        case Node.STATE_NEGATIVE.LEFTCHILD:
                            parent.Left = null;
                            break;
                        case Node.STATE_NEGATIVE.RIGHYCHILD:
                            parent.Right = null;
                            break;
                    }
                    break;
                case Node.STATE_POSITIVE.LEFTONLY:
                    switch (node.State_N)
                    {
                        case Node.STATE_NEGATIVE.ROOT:
                            Root = node.Left;
                            Root.Parent = null;
                            break;
                        case Node.STATE_NEGATIVE.LEFTCHILD:
                            parent.Left = node.Left;
                            node.Left.Parent = parent;
                            break;
                        case Node.STATE_NEGATIVE.RIGHYCHILD:
                            parent.Right = node.Left;
                            node.Left.Parent = parent;
                            break;
                    }
                    break;
                case Node.STATE_POSITIVE.RIGHTONLY:
                    switch (node.State_N)
                    {
                        case Node.STATE_NEGATIVE.ROOT:
                            Root = node.Right;
                            Root.Parent = null;
                            break;
                        case Node.STATE_NEGATIVE.LEFTCHILD:
                            parent.Left = node.Right;
                            node.Right.Parent = parent;
                            break;
                        case Node.STATE_NEGATIVE.RIGHYCHILD:
                            parent.Right = node.Right;
                            node.Right.Parent = parent;
                            break;
                    }
                    break;
                case Node.STATE_POSITIVE.FILLED:
                    switch (node.State_N)
                    {
                        case Node.STATE_NEGATIVE.ROOT:
                            Node forerunner = Root.GetForerunner();
                            forerunner.Right = Root.Right;
                            Root.Right.Parent = forerunner;
                            Root = Root.Left;
                            Root.Parent = null;
                            break;
                        case Node.STATE_NEGATIVE.LEFTCHILD:
                            parent.Left = node.Left;
                            Node forerunner_left = node.GetForerunner();
                            forerunner_left.Right = node.Right;
                            break;
                        case Node.STATE_NEGATIVE.RIGHYCHILD:
                            parent.Right = node.Left;
                            Node min = node.GetMin();
                            min.Right = node.Right;
                            break;
                    }
                    break;
            }
        }
    }
}
