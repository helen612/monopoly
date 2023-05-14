using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.AdditiveLevels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera mainCam;
    public Camera cam2;
    public Camera cam3;
    public Camera cam4;
    private int selectedCam;
    
    private int firstRoll;
    private int secondRoll;
    public Button bDragRoll;

    public Button bNextPlayer;
    public TMP_Text cashInfo;
    public Route route;
    void Start()
    {
        
        bNextPlayer.interactable = false;
        bDragRoll.interactable = true;
        cashInfo.color = Player.localPlayer.playerColor;
        mainCam.enabled = true;
        selectedCam = 1;
        //GameObject.Find("Route").
        
        Player.localPlayer.setRoute(route);
    }

    public void NextCam()
    {
        switch (selectedCam)
        {
            case 1:
            {
                mainCam.enabled = false;
                cam2.enabled = true;
                selectedCam = 2;
                break;
            }
            case 2:
            {
                cam2.enabled = false;
                cam3.enabled = true;
                selectedCam = 3;
                break;
            }
            case 3:
            {
                cam3.enabled = false;
                cam4.enabled = true;
                selectedCam = 4;
                break;
            }
            case 4:
            {
                cam4.enabled = false;
                mainCam.enabled = true;
                selectedCam = 1;
                break;
            }
            default:
            {
                cam2.enabled = false;
                cam3.enabled = false;
                cam4.enabled = false;
                mainCam.enabled = true;
                selectedCam = 1;
                break;
            }
        }
    }

    public void DragRoll()
    {
        firstRoll = UnityEngine.Random.Range(1, 6+1);
        secondRoll = UnityEngine.Random.Range(1, 6+1);
        Message.show("",firstRoll + " " + secondRoll);
        Player.localPlayer.startMove(firstRoll+secondRoll);
        if (firstRoll == secondRoll)
        {
            bDragRoll.interactable = true;
            bNextPlayer.interactable = false;
        }
        else
        {
            bDragRoll.interactable = false;
            bNextPlayer.interactable = true;
        }
        
        
    }

    public void nextPlayer()
    {
        bDragRoll.interactable = true;
        bNextPlayer.interactable = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
