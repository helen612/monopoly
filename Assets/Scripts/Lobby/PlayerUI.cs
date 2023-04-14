using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public TMP_Text TMP_Text;
    private Player Player;


    public void SetPlayer(Player player)
    {
        this.Player = player;
        TMP_Text.text = "Имя";

    }
}
