using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;

public class Route : MonoBehaviour
{
    Transform[] childObjects;
    public List<Transform> childNodeList = new List<Transform>();

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
            Vector3 currentPos = childNodeList[i].transform.position;
            if (i > 0)
            {
                Vector3 prevPos = childNodeList[i - 1].transform.position;
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
                childNodeList.Add(child.transform);
            }
        }
    }
}
