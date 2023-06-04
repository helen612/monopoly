using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSingle : MonoBehaviour
{
    public Color playerColor;
    public Vector3 playerCord;
    public int cash;
    
    public int routePosition; 
    private int steps;
    private bool isMoving;

    public bool MyMove;
    public int DoubleCount;
    public int forSkip;
    public int FreeJail;
    public List<GameObject> Owns;


    private void Update()
    {
        
    }

    private void Start()
    {
        cash = 1500;
        routePosition = 0;
        DoubleCount = 0;
        forSkip = 0;
        FreeJail = 0;
        Owns = new List<GameObject>();
    }
    
    public void startMove(int steps)
    {
        this.steps = steps;
        StartCoroutine(Move());
    }
    private IEnumerator Move()
    {
        if (isMoving)
        {
            yield break;
        }

        isMoving = true;
        while (steps > 0)
        {
            
            GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().moves[routePosition].countPlayer--;
            //currentRoute.childNodeList[routePosition].countPlayer--;

            routePosition++;
            routePosition %= GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().currentRoute.childNodeList.Count;

            Vector3 nextPos = GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>()
                                  .currentRoute.childNodeList[routePosition].position +
                              GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().moves[routePosition].getPos();
            //Vector3 nextPos = currentRoute.childNodeList[routePosition].node.transform.position;
            Debug.Log(GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().moves[routePosition].field.fullName);
            GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().moves[routePosition].countPlayer++;
            //Vector3 nextPos = currentRoute.childNodeList[routePosition].position;
            while (MoveToNextNode(nextPos))
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);
            steps--;
            if (routePosition == 0)
                GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().moves[0].onHere("GO");
        }


        isMoving = false;
        if (routePosition != 0)
            GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().moves[routePosition]
                .onHere(GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>()
                    .currentRoute.childNodeList[routePosition].tag);
        if (DoubleCount != 0)
        {
            UIControllerSingle.instance.bNextPlayer.interactable = false;
        }
        else
            UIControllerSingle.instance.bNextPlayer.interactable = true;


    }
    bool MoveToNextNode(Vector3 goal)
    {
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, 8f * Time.deltaTime));
    }
}
