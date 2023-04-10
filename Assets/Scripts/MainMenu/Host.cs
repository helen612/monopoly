using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Host : MonoBehaviour
{
    public TMP_InputField HostAddress;
    public static string room;
    
    public void StartSceneLobbyHost()
    {
        //inp = GameObject.Find("")
        MainMenu.HostMode = true;
        NetworkManager.singleton.StartHost();
        Player.localPlayer.HostGame();
    }
    public void StartSceneLobbyClient()
    {
        MainMenu.HostMode = false;
        room = HostAddress.text;
        //Player.localPlayer.JoinGame(HostAddress.text.ToUpper());

    }

}
