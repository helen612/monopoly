using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OwnButtonUISingle : MonoBehaviour
{
    public TMP_Text TMPText;

    private int routePosition;
    
    public void SetOwnName(string Name, Color color, int routePosition)
    {
        TMPText.text = Name;
        TMPText.color = color;
        this.routePosition = routePosition;
    }

    public FieldSingle GetMove()
    {
        return  GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().moves[routePosition];
    }

    public void onClick()
    {
        GameObject.FindWithTag("ShopUI").GetComponent<ShopUiControllerSingle>().SetChoosenKS(GetMove(), routePosition);
    }
}
