


using System;
using UnityEngine;

namespace Astar
{
    public enum NodeType
    {
        Close,
        Open,
    }

    public class Node : IComparable<Node>
    {
        public Node parent;
        public Vector3Int Pos;
        public int Weight; // °¡ÁßÄ¡
        public int F;
        public int G;
        public int H;


        //public NodeType Type;
        public int CompareTo(Node other)
        {
            if (other.F == this.F) return 0;
            else
                return other.F < this.F ? -1 : 1;
        }

        public override bool Equals(object obj) => this.Equals(obj as Node);
        public override int GetHashCode() => base.GetHashCode();

        public bool Equals(Node node)
        {
            if (node is null)
                return false;
            return GetHashCode() == node.GetHashCode();
        }

        public static bool operator ==(Node lhs, Node rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                    return true;
                else
                    return false;
            }
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Node lhs, Node rhs) => !(lhs == rhs);


        public void Reset()
        {
            parent = null;
            Pos = default(Vector3Int);
            Weight = default(int);
            F = default(int);
            G = default(int);
            H = default(int);
        }

    }
}

