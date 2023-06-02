using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.AdditiveLevels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerSingle : MonoBehaviour
{
    public static UIControllerSingle instance;
    // Start is called before the first frame update
    public Camera mainCam;
    public Camera cam2;
    public Camera cam3;
    public Camera cam4;
    private int selectedCam;
    
    private int firstRoll;
    private int secondRoll;
    public Button bDragRoll;

    public GameObject ShopUI;
    public Button Shop;
    public Button bNextPlayer;
    public TMP_Text cashInfo;
    public Route route;
    //public GameObject pm;
    public TMP_Text listPlayersUI;
    
    void Start()
    {
        instance = this;
        
        mainCam.enabled = true;
        selectedCam = 1;
    }
    
    public void updateCash(int money, Color owner)
    {
        cashInfo.color = owner;
        cashInfo.text = "Ваш счет: " + money + " М";
    }
    
/*
    public void toLose()
    {
        var moves = GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().moves;
        
        for (int i = 0; i < moves.Count; i++)
        {
            if (moves[i].field.owner == Player.localPlayer.playerColor)
            {
                GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().moves[i].field.owner =
                    Color.white;
                GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().moves[i].field.level =
                    0;
                foreach (var house in moves[i].houses)
                {
                    Destroy(house);
                }
            }
        }

        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().moves[Player.localPlayer.routePosition]
            .countPlayer--;
        Player.localPlayer.toLose();
        
    }
*/
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
        bDragRoll.interactable = false;
    }

    
    
    public void ShowShopUI()
    {
        ShopUI.SetActive(true);
        ShopUI.GetComponent<ShopUIContoller>().JoinShop();
    }

    public void HideShopUI()
    {
        ShopUI.SetActive(false);
    }

    
    // Update is called once per frame
    void Update()
    {
        /*
        if (!Player.localPlayer.MyMove)
        {
            //updateUI();
        }
        */
    }
}
