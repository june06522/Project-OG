using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class BSPNode
{
    public BSPNode leftNode;
    public BSPNode rightNode;
    public BSPNode parNode;
    public RectInt nodeRect; //분리된 공간의 rect정보
    public BSPNode(RectInt rect)
    {
        this.nodeRect = rect;
    }
}

public class BSPAlgorithm : MonoBehaviour
{
    [SerializeField] int maxDip = 10;
    [SerializeField] int Maxdist = 10;

    RoomGenarator room;
    BSPNode root;

    private void Awake()
    {
        room = GetComponent<RoomGenarator>();
        int x, y;
        x = room.Width * room.WidthLength;
        y = room.Height * room.HeightLength;
        root = new BSPNode(new RectInt(-x / 2, -y / 2, x / 2, y / 2));
        Devide(root, 0);
    }

    void Devide(BSPNode tree, int n)
    {
        if (n >= maxDip)
            return;

        int devX = 0,devY = 0;

        if(tree.nodeRect.width > tree.nodeRect.height)
            devX = Random.Range(-Maxdist, Maxdist);
        else
            devY = Random.Range(-Maxdist, Maxdist);

        tree.leftNode = new BSPNode(new RectInt(
            tree.nodeRect.x, tree.nodeRect.y, tree.nodeRect.width, tree.nodeRect.height));
        tree.rightNode = new BSPNode(new RectInt(
             tree.nodeRect.x, tree.nodeRect.y, tree.nodeRect.width, tree.nodeRect.height));
        tree.leftNode.parNode = tree;
        tree.rightNode.parNode = tree;

        Devide(tree.leftNode, n + 1);
        Devide(tree.rightNode, n + 1);
    }
}
