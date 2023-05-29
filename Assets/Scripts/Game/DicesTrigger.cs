using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DicesTrigger : MonoBehaviour
{
    private int dices;
    private int prev;

    private void Start()
    {
        dices = 0;
        prev = 0;
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody.velocity.magnitude == 0 
            && other.attachedRigidbody.angularVelocity.magnitude == 0)
        {
            switch (other.transform.parent.gameObject.name)
            {
                case "Dice1":
                {
                    other.transform.parent.gameObject.transform.position = new Vector3(-23.5f, 23.24f, 104f);
                    Debug.Log("ПЕРШЫЙ");
                    dices++;
                    break;
                }
                case "Dice2":
                {
                    other.transform.parent.gameObject.transform.position = new Vector3(-21.9f, 23.24f, 104f);
                    Debug.Log("Други");
                    dices++;
                    break;
                }
                default:
                {
                    prev = 0;
                    dices = 0;
                    Debug.Log("Один из кубиков упал на ребро");
                    break;
                }
            }

            if (dices == 1)
            {
                int.TryParse(other.name, out prev);
            }
            else if (dices == 2 && prev != 0)
            {
                int thisvalue = 0;
                int.TryParse(other.name, out thisvalue);
                

                if (prev == thisvalue)
                {
                    Player.localPlayer.DoubleCount++;
                    //UIController.instance.bNextPlayer.interactable = false;
                    UIController.instance.bDragRoll.interactable = true;
                    if (Player.localPlayer.DoubleCount == 3)
                    {
                        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().moves[Player.localPlayer.routePosition].onHere("Jail");
                        Player.localPlayer.DoubleCount = 0;
                        //UIController.instance.bNextPlayer.interactable = true;
                        UIController.instance.bDragRoll.interactable = false;
                    }
                    else 
                        Player.localPlayer.startMove(thisvalue + prev);
                }
                else
                {
                    Player.localPlayer.DoubleCount = 0;
                    Player.localPlayer.startMove(thisvalue + prev);
                }
                
                
                
                prev = 0;
                dices = 0;
            }
        }
    }
}
