using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUiControllerSingle : MonoBehaviour
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
    public Button upgrade;
    public Button downgrade;
    public TMP_Text Status;
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

    public static ShopUiControllerSingle instance;

    int choosenRoutePosition;
    private int currentPosition;
    

    private void Start()
    {
        instance = this;
    }

    public void JoinShop()
    {
        choosenRoutePosition = GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().
            Players[GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().QqPlayer].GetComponent<PlayerSingle>().routePosition;
        FieldSingle field = GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().moves[choosenRoutePosition];

        switch (field.field.tag)
        {
            case "Street":
            {
                owner.color = field.field.owner;
                TR.SetActive(false);
                water.SetActive(false);
                Energy.SetActive(false);
                Buy.GetComponentInChildren<TMP_Text>().text = "Купить | М" + field.field.cost;
                if(field.field.owner == Color.white)
                    Buy.interactable = true;
                else
                    Buy.interactable = false;
                
                
                Street.SetActive(true);
                Name.text = field.field.fullName;
                color.color = field.field.Color;
                Rent.text = field.field.rent.ToString();
                GroupRent.text = field.field.rentGroup.ToString();
                Home1.text = field.field.home1.ToString();
                Home2.text = field.field.home2.ToString();
                Home3.text = field.field.home3.ToString();
                Home4.text = field.field.home4.ToString();
                Hotel.text = field.field.hotel.ToString();
                BuildHome.text = field.field.costBuild.ToString();
                BuildHotel.text = field.field.costBuild.ToString();
                break;
            }
            case "TR":
            {
                //owner.color = curPos.field.owner;
                water.SetActive(false);
                Energy.SetActive(false);
                Street.SetActive(false);
                TR.SetActive(true);
                
                Buy.GetComponentInChildren<TMP_Text>().text = "Купить | М" + field.field.cost;
                if(field.field.owner == Color.white)
                    Buy.interactable = true;
                else
                    Buy.interactable = false;

                
                NameTR.text = field.field.fullName;
                break;
            }
            case "Com":
            {
                owner.color = field.field.owner;
                Street.SetActive(false);
                TR.SetActive(false);
                water.SetActive(false);
                Energy.SetActive(false);

                Buy.GetComponentInChildren<TMP_Text>().text = "Купить | М" + field.field.cost;
                if(field.field.owner == Color.white)
                    Buy.interactable = true;
                else
                    Buy.interactable = false;


                if (field.name == "Com1")
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
        var fieldSingle = GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().changeOwner();
        SetChoosenKS(fieldSingle, choosenRoutePosition);
        JoinShop();
        
    }

    public GameObject spawnButtowOwber()
    {
        GameObject obj = Instantiate(OwnPrefab, OwnPrefabParent);
        return obj;
    }
    
    public void SetChoosenKS(FieldSingle field, int routePosition)
    {
        
        choosenRoutePosition = routePosition;
        switch (field.field.tag)
        {
            case "Street":
                //owner.color = field.field.owner;
                Status.text = "Уровень вашей улицы: " + field.field.level;
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
                
                upgrade.interactable = false; 
                downgrade.interactable = false;
             
                if (field.field.level > 1 && field.field.level < 7)
                {
                    upgrade.interactable = true;  
                }

                if (field.field.level > 2)
                {
                    downgrade.interactable = true;
                }
                    
                break;
            case "TR":
                //owner.color = field.field.owner;
                Status.text = "Уровень вашей станции: " + field.field.level;
                Cwater.SetActive(false);
                CEnergy.SetActive(false);
                CStreet.SetActive(false);
                CTR.SetActive(true);
                upgrade.interactable = false; 
                downgrade.interactable = false;
                CNameTR.text = field.field.fullName;
                break;
            case "Com":
                //owner.color = field.field.owner;
                Status.text = "Уровень вашей ком. предприятия: " + field.field.level;
                CStreet.SetActive(false);
                CTR.SetActive(false);
                Cwater.SetActive(false);
                CEnergy.SetActive(false);
                upgrade.interactable = false; 
                downgrade.interactable = false;
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
    
    public void onClickUpgrade()
    {
        var r = GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().addHouseOnStreet(choosenRoutePosition);
        SetChoosenKS(r, choosenRoutePosition);
    }
    public void onClickDowngrade()
    {
        var r = GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().destroyHouseOnStreet(choosenRoutePosition);
        SetChoosenKS(r, choosenRoutePosition);
    }
    
}
