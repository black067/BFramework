using System;
using BFramework.Tools;

namespace BFramework.DataStructure
{
    public class BinaryTree<T> where T : class, IComparable<T>
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
                    if (compareResult >= 0)
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

        public void PreOrder()
        {

        }

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
            Console.WriteLine(GetState(node));
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
