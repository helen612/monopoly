using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    public Transform node;
    public int countPlayer;
    public List<Vector3> spawndots;

    public Node(Transform node)
    {
        this.node = node;
        countPlayer = 0;
        spawndots = new List<Vector3>
        {
            new Vector3(node.transform.position.x + 1, node.transform.position.y, node.transform.position.z + 1),
            new Vector3(node.transform.position.x + 1, node.transform.position.y, node.transform.position.z),
            new Vector3(node.transform.position.x + 1, node.transform.position.y, node.transform.position.z + 1),
            new Vector3(node.transform.position.x , node.transform.position.y, node.transform.position.z + 1),
            new Vector3(node.transform.position.x, node.transform.position.y, node.transform.position.z),
            new Vector3(node.transform.position.x, node.transform.position.y, node.transform.position.z - 1),
            new Vector3(node.transform.position.x - 1, node.transform.position.y, node.transform.position.z - 1),
            new Vector3(node.transform.position.x - 1, node.transform.position.y, node.transform.position.z + 1),

        };
    }

    public Vector3 getPos()
    {
        return spawndots[countPlayer];
    }
    
}

public class Route : MonoBehaviour
{
    Transform[] childObjects;
    public List<Node> childNodeList = new List<Node>();

    private void Start()
    {
        FillNodes();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        
        FillNodes();

        for (int i = 0; i < childNodeList.Count; i++)
        {
            Vector3 currentPos = childNodeList[i].node.transform.position;
            if (i > 0)
            {
                Vector3 prevPos = childNodeList[i - 1].node.transform.position;
                Gizmos.DrawLine(prevPos, currentPos);
            }
        }
    }

    void FillNodes()
    {
        childNodeList.Clear();
        childObjects = GetComponentsInChildren<Transform>();

        foreach (var child in childObjects)
        {
            if (child != this.transform)
            {
                childNodeList.Add(new Node(child.transform));
            }
        }
    }
}
