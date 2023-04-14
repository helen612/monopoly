using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using Mirror;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text;
using Telepathy;
using Unity.VisualScripting;

[System.Serializable]
public class Match : NetworkBehaviour
{
    public string ID;
    public  List<GameObject> players = new List<GameObject>();


    public Match(string ID, GameObject player)
    {
        this.ID = ID;
        players.Add(player);
    }
}

public class MainMenu : NetworkBehaviour
{
    public static MainMenu instance;
    public static bool HostMode; 
    public readonly SyncList<Match> matches = new SyncList<Match>();
    public readonly SyncList<string> matchIDs = new SyncList<string>();
    //public InputField JoinInput;
    public Button HostButton;
    public Button JoinButton;
    public TMP_InputField RoomID;
    
    
    
    public Canvas LobbyCanvas;

    public Transform UIPlayerParent;
    public GameObject UIPlayerPrefab;
    public TMP_Text IDText;
    public Button StartGameButton;
    private TurnManager TurnManager;
    public GameObject turnManager;
    public bool inGame;
    
    private void Start()
    {
        instance = this;
        Debug.Log($"Запущена сцена lobby");
        
        TurnManager = turnManager.GetComponent<TurnManager>();
    }

    private void Update()
    {

    }


    
    public void Host()
    {
        Debug.Log($"Запускаю Создание комнаты");
        HostButton.interactable = false;
        JoinButton.interactable = false;
        RoomID.interactable = false;
        Player.localPlayer.HostGame();
    }
    public void HostSuccess(bool success, string matchID)
    {
        if (success)
        {
            LobbyCanvas.enabled = true;
            //SpawnPlayerUIPrefab(Player.localPlayer);
            TurnManager.players.Add(Player.localPlayer);
            UpdatePlayers();
            IDText.text = matchID;
        }
        else
        {
            //SceneManager.LoadScene("MainMenu");
            HostButton.interactable = true;
            JoinButton.interactable = true;
            RoomID.interactable = true;
        }
    }

    public void UpdatePlayers()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PlayerUI");
        if (objects.Length != 0)
        {
            foreach (var plUI in objects)
            {
                Destroy(plUI);
            }
        }
        foreach (var player in TurnManager.players)
        {
            SpawnPlayerUIPrefab(player);
        }



    }
    
    public void Join()
    {
        Debug.Log($"Запускаю подключение к комнате");
        HostButton.interactable = false;
        JoinButton.interactable = false;
        RoomID.interactable = false;
        Player.localPlayer.JoinGame(RoomID.text.ToUpper());
    }
    public void JoinSuccess(bool success, string matchID)
    {
        if (success)
        {
            LobbyCanvas.enabled = true;
            //SpawnAll();
            //SpawnPlayerUIPrefab(Player.localPlayer);
            TurnManager.players.Add(Player.localPlayer);
            UpdatePlayers();
            IDText.text = matchID;
            StartGameButton.interactable = false;
        }
        else
        {
            //SceneManager.LoadScene("MainMenu");
            RoomID.interactable = true;
            HostButton.interactable = true;
            JoinButton.interactable = true;
        }
    }
    public bool HostGame(string matchID, GameObject player)
    {
        if (!matchIDs.Contains(matchID))
        {
            matchIDs.Add(matchID);
            matches.Add(new Match(matchID, player));
            return true;
        }
        else return false;
    }
    public bool JoinGame(string matchID, GameObject player)
    {
        if (matchIDs.Contains(matchID))
        {
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].ID == matchID)
                {
                    matches[i].players.Add(player);
                    break;
                }
            }
            return true;
        }
        else return false;
    }
    public static string GetRandomID()
    {
        string ID = string.Empty;
        for(int i = 0; i < 5; i++)
        {
            int rand = UnityEngine.Random.Range(0, 36);
            if (rand < 26) ID += (char)+(rand + 65);
            else ID += (rand - 26).ToString();
        }
        return ID;
    }
    public void SpawnPlayerUIPrefab(Player player)
    {
        GameObject newUIPlayer = Instantiate(UIPlayerPrefab, UIPlayerParent);
        newUIPlayer.GetComponent<PlayerUI>().SetPlayer(player);
    }
    
    public void StartGame()
    {
        Player.localPlayer.BeginGame();
    }
    public void BeginGame(string matchID)
    {
        for (int i = 0; i < matches.Count; i++)
        {
            if (matches[i].ID == matchID)
            {
                foreach(var player in matches[i].players)
                {
                    Player player1 = player.GetComponent<Player>();
                    player1.StartGame();
                }
                break;
            }
        }
    }
}



public static class MatchEctension
{
    public static Guid ToGuid(this string id)
    {
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] inputBytes = Encoding.Default.GetBytes(id);
        byte[] hasBytes = provider.ComputeHash(inputBytes);
        return new Guid(hasBytes);
    }
}

