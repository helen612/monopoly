using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OwnButtonUI : MonoBehaviour
{
    public TMP_Text TMPText;

    private int routePosition;
    
    public void SetOwnName(string Name, Color color)
    {
        TMPText.text = Name;
        TMPText.color = color;
        routePosition = Player.localPlayer.routePosition;
    }

    public Field GetMove()
    {
        return  GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().moves[routePosition];
    }

    public void onClick()
    {
        GameObject.FindWithTag("ShopUI").GetComponent<ShopUIContoller>().SetChoosenKS(GetMove(), routePosition);
    }
}
