using System;

namespace BFramework.ExpandedMath
{
    public class BinaryTree<T> where T : IComparable
    {
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
        }

        private Node _root = null;

        public BinaryTree()
        {

        }

        public BinaryTree(T[] items)
        {
            for(int i = 0, length = items.Length; i < length; i++)
            {
                Add(items[i]);
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
                while (true)
                {
                    //放在temp的左边
                    if (item.CompareTo(temp.Data) <= 0)
                    {
                        if (temp.Left == null)
                        {
                            //父子相认
                            temp.Left = newNode;
                            newNode.Parent = temp;
                            break;
                        }
                        else
                        {
                            temp = temp.Left;
                        }
                    }
                    //放在temp的右边
                    else
                    {
                        if (temp.Right == null)
                        {
                            //父子相认
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
        }

        public bool Contains(T item)
        {
            return Contains(item, _root);
        }

        private bool Contains(T item, Node node)
        {
            if (node == null)
                return false;
            if (node.Data.CompareTo(item) == 0)
            {
                return true;
            }
            else if (item.CompareTo(node.Data) < 0)
            {
                return Contains(item, node.Left);
            }
            else
            {
                return Contains(item, node.Right);
            }
        }

        public void Delete(T item)
        {
            Node temp = _root;
            while (true)
            {
                if (temp == null) return;
                else if (temp.Data.CompareTo(item) == 0) Delete(temp);
                else if (item.CompareTo(temp.Data) > 0) temp = temp.Right;
                else temp = temp.Left;
            }
        }

        private void Delete(Node node)
        {
            if (node.Left== null && node.Right == null)
            {
                if (node.Parent == null)
                {
                    _root = null;
                }
                else if (node.Parent.Left == node)
                {
                    node.Parent.Left = null;
                }
                else if (node.Parent.Right == node)
                {
                    node.Parent.Right = null;
                }
                return;
            }
            else if (node.Left == null && node.Right != null)
            {
                node.Data = node.Right.Data;
                node.Right = null;
                return;
            }
            else if (node.Right == null && node.Left != null)
            {
                node.Data = node.Left.Data;
                node.Left= null;
                return;
            }
            else
            {
                Node temp = node.Right;
                while (true)
                {
                    if (temp.Left != null)
                    {
                        temp = temp.Left;
                    }
                    else
                    {
                        break;
                    }
                }
                node.Data = temp.Data;
                Delete(temp);
            }
        }
    }
}
