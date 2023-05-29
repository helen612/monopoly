using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.AdditiveLevels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
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
    public GameObject pm;
    public TMP_Text listPlayersUI;
    
    void Start()
    {
        Player.localPlayer.setPM();
        instance = this;
        updateUI();
        
        cashInfo.color = Player.localPlayer.playerColor;
        mainCam.enabled = true;
        selectedCam = 1;
        
        updateCash();
        Player.localPlayer.currentRoute = route;
    }

    public void updateListPlayersUI(List<Player> players)
    {
        listPlayersUI.text = "";
        foreach (var p in players)
        {
            listPlayersUI.text += $"<color=#{ColorUtility.ToHtmlStringRGB(p.playerColor)}>{p.PlayerDisplayName}</color>\n";
        }
    }

    public void updateUI()
    {
        if (Player.localPlayer.MyMove)
        {
            bNextPlayer.interactable = false;
            if (Player.localPlayer.forSkip == 0)
                bDragRoll.interactable = true;
            else
            {
                bDragRoll.interactable = false;
                bNextPlayer.interactable = true;
            }
               
            Shop.interactable = true;
        }
        else
        {
            bNextPlayer.interactable = false;
            bDragRoll.interactable = false;
            Shop.interactable = false;
        }

    }

    public void updateCash()
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
        bDragRoll.interactable = false;
        updateCash();
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

    public void nextPlayer()
    {
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().NextPlayer();
    }
    // Update is called once per frame
    void Update()
    {
        if (!Player.localPlayer.MyMove)
        {
            updateUI();
        }
    }
}
