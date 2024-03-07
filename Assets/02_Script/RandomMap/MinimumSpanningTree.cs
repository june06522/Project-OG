using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimumSpanningTree : MonoBehaviour
{
    public static List<TriEdge> FindLine(List<Triangle> triangles, Vector2 startPos)
    {
        PriorityQueue pq = new PriorityQueue();

        List<TriEdge> list = new List<TriEdge>();
        List<Vector2> checkVisited = new List<Vector2>();

        Dictionary<Vector2, List<Vector2>> dic = new Dictionary<Vector2, List<Vector2>>();

        checkVisited.Add(startPos);

        for(int i = 0; i < triangles.Count; i++)
        {
            if(!dic.ContainsKey(triangles[i].a))
                dic.Add(triangles[i].a, new List<Vector2>());

            if (!dic.ContainsKey(triangles[i].b))
                dic.Add(triangles[i].b, new List<Vector2>());

            if (!dic.ContainsKey(triangles[i].c))
                dic.Add(triangles[i].c, new List<Vector2>());

            dic[triangles[i].a].Add(triangles[i].b);
            dic[triangles[i].a].Add(triangles[i].c);
            dic[triangles[i].b].Add(triangles[i].a);
            dic[triangles[i].b].Add(triangles[i].c);
            dic[triangles[i].c].Add(triangles[i].a);
            dic[triangles[i].c].Add(triangles[i].b);
        }

        for(int i = 0; i < dic[startPos].Count; i++)
        {
            pq.EnQueue(new PQItem(startPos, dic[startPos][i], Dif(startPos, dic[startPos][i])));
        }

        while(pq.pq.Count > 0)
        {
            PQItem temp = pq.DeQueue();
            if (checkVisited.Contains(temp.b))
                continue;

            checkVisited.Add(temp.b);

            list.Add(new TriEdge(temp.a,temp.b));

            for (int i = 0; i < dic[temp.b].Count; i++)
            {
                pq.EnQueue(new PQItem(temp.b, dic[temp.b][i], Dif(temp.b, dic[temp.b][i])));
            }
        }

        return list;
    }
    
    public static float Dif(Vector2 a, Vector2 b) =>
        Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2));
}

public class PriorityQueue
{
    public List<PQItem> pq = new List<PQItem>();

    public void EnQueue(PQItem item)
    {
        for(int i = 0; i < pq.Count;i++)
        {
            if(item.weight < pq[i].weight)
            {
                pq.Insert(i, item);
                return;
            }
        }
        pq.Add(item);
    }

    public PQItem DeQueue()
    {
        PQItem item = pq[0];
        pq.Remove(item);
        return item;
    }
}

public class PQItem
{
    public Vector2 a;
    public Vector2 b;
    public float weight;

    public PQItem(Vector2 a, Vector2 b, float weight)
    {
        this.a      = a;
        this.b      = b;
        this.weight = weight;
    }
}