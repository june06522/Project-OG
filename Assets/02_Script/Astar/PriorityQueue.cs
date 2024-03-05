using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Astar
{
    public class PriorityQueue
    {
        private List<Node> _heap;

        public int Count => _heap.Count;
        public Node Root => _heap[0];

        public PriorityQueue(int roomNodeSize)
        {
            _heap = new List<Node>(roomNodeSize);
        }

        public void Push(Node data)
        {
            _heap.Add(data);
            int idx = _heap.Count - 1;
            while (idx > 0)
            {
                int parent = (idx - 1) / 2;
                if (_heap[idx].CompareTo(_heap[parent]) > 0)
                {
                    ( _heap[idx], _heap[parent] ) = ( _heap[parent], _heap[idx] );
                    
                    idx = parent;
                }
                else
                {
                    break;
                }
            }
        }

        public Node Pop()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException();

            Node ret = _heap[0];

            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex];
            _heap.RemoveAt(lastIndex); 
            lastIndex--;

            int now = 0;

            while (true)
            {
                //child
                int left = 2 * now + 1; 
                int right = 2 * now + 2;

                int next = now;

                if (left <= lastIndex && _heap[next].CompareTo(_heap[left]) < 0)
                    next = left;

                if (right <= lastIndex && _heap[next].CompareTo(_heap[right]) < 0)
                    next = right;
 
                if (next == now)
                {
                    break;
                }

                (_heap[now], _heap[next]) = (_heap[next], _heap[now]);

                now = next;
            }

            return ret;
        }

        public Node Contains(Node other)
        {
            return _heap.Find((node) => node == other);
        }

        public void Clear()
        {
            _heap.Clear();
        }
    }

}

