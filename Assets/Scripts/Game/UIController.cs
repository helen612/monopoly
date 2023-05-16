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
        UpodateCash();
        
        Player.localPlayer.setRoute(route);
    }

    public void UpodateCash()
    {
        cashInfo.text = "Ваш счет: " + Player.localPlayer.cash + " М";
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
        GameObject.Find("Dice1").GetComponent<DiceRoller>().RollDice(1);
        GameObject.Find("Dice2").GetComponent<DiceRoller>().RollDice(2);
        Player.localPlayer.cash -= 100;
        UpodateCash();
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
