using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIContoller : MonoBehaviour
{
    #region Current Position

    public GameObject OwnPrefab;
    public Transform OwnPrefabParent;
    
    [Header("Current Position")] 
    public Button Buy;

    [Header("Street")] 
    public Image owner;
    public GameObject Street;
    public TMP_Text Name;
    public Image color;
    public TMP_Text Rent;
    public TMP_Text GroupRent;
    public TMP_Text Home1;
    public TMP_Text Home2;
    public TMP_Text Home3;
    public TMP_Text Home4;
    public TMP_Text Hotel;
    public TMP_Text BuildHome;
    public TMP_Text BuildHotel;

    [Header("Train Road")] 
    public GameObject TR;
    public TMP_Text NameTR;
    
    [Header("Water")] 
    public GameObject water;
    
    [Header("Energy")] 
    public GameObject Energy;

    
    
    #endregion
    

    #region Choosen Position

    [Header("Choosen Position")]
    [Header("Street")] 
    public GameObject CStreet;
    public TMP_Text CName;
    public Image Ccolor;
    public TMP_Text CRent;
    public TMP_Text CGroupRent;
    public TMP_Text CHome1;
    public TMP_Text CHome2;
    public TMP_Text CHome3;
    public TMP_Text CHome4;
    public TMP_Text CHotel;
    public TMP_Text CBuildHome;
    public TMP_Text CBuildHotel;

    [Header("Train Road")] 
    public GameObject CTR;
    public TMP_Text CNameTR;
    
    [Header("Water")] 
    public GameObject Cwater;
    
    [Header("Energy")] 
    public GameObject CEnergy;

    #endregion
    

    public void JoinShop()
    {
        var curPos = GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>()
            .moves[Player.localPlayer.routePosition];
        switch (curPos.field.tag)
        {
            case "Street":
            {
                owner.color = curPos.field.owner;
                TR.SetActive(false);
                water.SetActive(false);
                Energy.SetActive(false);
                Buy.GetComponentInChildren<TMP_Text>().text = "Купить | М" + curPos.field.cost;
                if(curPos.field.owner == Color.white)
                    Buy.interactable = true;
                else
                    Buy.interactable = false;
                
                
                Street.SetActive(true);
                Name.text = curPos.field.fullName;
                color.color = curPos.field.Color;
                Rent.text = curPos.field.rent.ToString();
                GroupRent.text = curPos.field.rentGroup.ToString();
                Home1.text = curPos.field.home1.ToString();
                Home2.text = curPos.field.home2.ToString();
                Home3.text = curPos.field.home3.ToString();
                Home4.text = curPos.field.home4.ToString();
                Hotel.text = curPos.field.hotel.ToString();
                BuildHome.text = curPos.field.costBuild.ToString();
                BuildHotel.text = curPos.field.costBuild.ToString();
                break;
            }
            case "TR":
            {
                //owner.color = curPos.field.owner;
                water.SetActive(false);
                Energy.SetActive(false);
                Street.SetActive(false);
                TR.SetActive(true);
                
                Buy.GetComponentInChildren<TMP_Text>().text = "Купить | М" + curPos.field.cost;
                if(curPos.field.owner == Color.white)
                    Buy.interactable = true;
                else
                    Buy.interactable = false;

                
                NameTR.text = curPos.field.fullName;
                break;
            }
            case "Com":
            {
                owner.color = curPos.field.owner;
                Street.SetActive(false);
                TR.SetActive(false);
                water.SetActive(false);
                Energy.SetActive(false);

                Buy.GetComponentInChildren<TMP_Text>().text = "Купить | М" + curPos.field.cost;
                if(curPos.field.owner == Color.white)
                    Buy.interactable = true;
                else
                    Buy.interactable = false;


                if (curPos.name == "Com1")
                    Energy.SetActive(true);
                else
                    water.SetActive(true);
                break;
            }
            default:
            {
                owner.color = Color.white;
                Street.SetActive(false);
                TR.SetActive(false);
                water.SetActive(false);
                Energy.SetActive(false);
                Buy.GetComponentInChildren<TMP_Text>().text = "Купить";
                Buy.interactable = false;
                break;
            }
        }
    }

    public void BuyKS()
    {
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>()
            .moves[Player.localPlayer.routePosition].field.owner = Player.localPlayer.playerColor;
        Player.localPlayer.cash -= GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>()
            .moves[Player.localPlayer.routePosition].field.cost;
        UIController.instance.updateCash();
        GameObject obj = Instantiate(OwnPrefab, OwnPrefabParent);
        obj.GetComponent<OwnButtonUI>().SetOwnName(GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>()
            .moves[Player.localPlayer.routePosition].field.fullName, GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>()
            .moves[Player.localPlayer.routePosition].field.Color);
        JoinShop();
    }

    public void SetChoosenKS(Field field)
    {
        switch (field.field.tag)
        {
            case "Street":
                //owner.color = field.field.owner;
                CTR.SetActive(false);
                Cwater.SetActive(false);
                CEnergy.SetActive(false);
                CStreet.SetActive(true);
                
                CName.text = field.field.fullName;
                Ccolor.color = field.field.Color;
                CRent.text = field.field.rent.ToString();
                CGroupRent.text = field.field.rentGroup.ToString();
                CHome1.text = field.field.home1.ToString();
                CHome2.text = field.field.home2.ToString();
                CHome3.text = field.field.home3.ToString();
                CHome4.text = field.field.home4.ToString();
                CHotel.text = field.field.hotel.ToString();
                CBuildHome.text = field.field.costBuild.ToString();
                CBuildHotel.text = field.field.costBuild.ToString();
                break;
            case "TR":
                //owner.color = field.field.owner;
                Cwater.SetActive(false);
                CEnergy.SetActive(false);
                CStreet.SetActive(false);
                CTR.SetActive(true);
                
                CNameTR.text = field.field.fullName;
                break;
            case "Com":
                //owner.color = field.field.owner;
                CStreet.SetActive(false);
                CTR.SetActive(false);
                Cwater.SetActive(false);
                CEnergy.SetActive(false);

                if (field.name == "Com1")
                    CEnergy.SetActive(true);
                else
                    Cwater.SetActive(true);
                break;
            default:
                CStreet.SetActive(false);
                CTR.SetActive(false);
                Cwater.SetActive(false);
                CEnergy.SetActive(false);
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        
    }
}
