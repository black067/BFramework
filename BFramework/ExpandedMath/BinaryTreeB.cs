using System;

namespace BFramework.ExpandedMath
{
    [Serializable]
    public class BinaryTreeB<T> where T : IComparable<T>
    {
        [Serializable]
        private class Node
        {
            public Node(T data)
            {
                Data = data;
            }

            public Node Parent { get; set; }

            public Node Left { get; set; }

            public Node Right { get; set; }

            public T Data { get; set; }
            
            public Node GetMinimumChild()
            {
                if (Left != null)
                {
                    return Left.GetMinimumChild();
                }
                else return this;
            }

            public Node GetMaximumChild()
            {
                if (Right != null)
                {
                    return Right.GetMaximumChild();
                }
                else return this;
            }

            public Node GetSuccessor()
            {
                if(Right != null)
                {
                    return Right.GetMinimumChild();
                }
                Node tempParent = Parent, temp = this;
                for(;tempParent != null && temp == tempParent.Right;)
                {
                    temp = tempParent;
                    tempParent = temp.Parent;
                }
                return tempParent;
            }

            public Node GetForerunner()
            {
                if (Left != null)
                {
                    return Left.GetMaximumChild();
                }
                Node tempParent = Parent, temp = this;
                for (; tempParent != null && temp == tempParent.Left;)
                {
                    temp = tempParent;
                    tempParent = temp.Parent;
                }
                return tempParent;
            }
        }

        private Node _root = null;

        public T Root { get { return _root.Data; } }

        public int Count { get; private set; } = 0;

        public BinaryTreeB()
        {
            Count = 0;
        }

        public BinaryTreeB(T[] items)
        {
            for(int i = 0, length = items.Length; i < length; i++)
            {
                Add(items[i]);
            }
        }
        
        public T Min
        {
            get
            {
                return _root.GetMinimumChild().Data;
            }
        }

        public T Max
        {
            get
            {
                return _root.GetMaximumChild().Data;
            }
        }

        public void Add(T item)
        {
            Node newNode = new Node(item);
            if (_root == null)
            {
                _root = newNode;
            }
            else
            {
                Node temp = _root;
                for (; ; )
                {
                    if (item.CompareTo(temp.Data) <= 0)
                    {
                        if (temp.Left == null)
                        {
                            temp.Left = newNode;
                            newNode.Parent = temp;
                            break;
                        }
                        else
                        {
                            temp = temp.Left;
                        }
                    }
                    else
                    {
                        if (temp.Right == null)
                        {
                            temp.Right = newNode;
                            newNode.Parent = temp;
                            break;
                        }
                        else
                        {
                            temp = temp.Right;
                        }
                    }
                }
            }
            Count++;
        }

        public delegate int Comparison(T a, T b);

        public void Sort(Comparison comparison)
        {

        }

        public bool Contains(T item)
        {
            return Find(item) != null;
        }

        private Node Find(T item)
        {
            Node temp = _root;
            for (; temp != null;)
            {
                int compareResult = temp.Data.CompareTo(item);
                if (compareResult == 0)
                {
                    break;
                }
                else if (compareResult < 0)
                {
                    temp = temp.Right;
                }
                else
                {
                    temp = temp.Left;
                }
            }
            return temp;
        }

        public void PreOrder() { PreOrder(_root); }

        private void PreOrder(Node root)
        {
            if (root == null) return;
            Console.WriteLine(root.Data);
            PreOrder(root.Left);
            PreOrder(root.Right);
        }

        public void Remove(T item)
        {
            Node temp = Find(item);
            if (temp == null) return;
            if (temp.Left == null && temp.Right == null)
            {
                Node parent = temp.Parent;
                if (parent == null) _root = null;
                else if (parent.Left == temp) { parent.Left = null; }
                else { parent.Right = null; }
            }
            else if(temp.Left == null && temp.Right != null)
            {
                Node parent = temp.Parent;
                if (parent == null) temp = temp.Right;
                else
                {
                    if (temp == parent.Left) temp.Right = parent.Left;
                    else temp.Right = parent.Right;
                }
            }
            else if(temp.Right == null && temp.Left != null)
            {
                Node parent = temp.Parent;
                if (parent == null) temp = temp.Left;
                else
                {
                    if (temp == parent.Left) { temp.Left = parent.Left; }
                    else { temp.Left = parent.Right; }
                }
            }
            else
            {
                Node successor = temp.GetSuccessor();
                Node successorParent = successor.Parent;
                // 3.1 要删除点是其父节点的左节点  
                if (temp == temp.Parent.Left)
                {
                    temp.Parent.Left = successor;
                    successor.Left = temp.Left;
                    if (temp == successorParent)    // 判断要删除点是否为后继点的父节点  
                    {
                        temp.Right = null;
                    }
                    else
                    {
                        successor.Right = temp.Right;
                        successorParent.Left = null;
                    }
                }
                // 3.2 要删除点是其父节点的右节点  
                else if (temp == temp.Parent.Right)
                {
                    temp.Parent.Right = successor;
                    successor.Left = temp.Left;
                    if (temp == successorParent)
                    {
                        temp.Right = null;
                    }
                    else
                    {
                        successor.Right = temp.Right;
                        successorParent.Left = null;
                    }
                }
            }
            Count--;
        }
    }
}
