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
    public bool inMatch;
    public bool MatchFull;
    public  List<GameObject> players = new List<GameObject>();


    public Match(string ID, GameObject player)
    {
        MatchFull = false;
        inMatch = false;
        this.ID = ID;
        players.Add(player);
    }

    public Match()
    {
        
    }
}

public class MainMenu : NetworkBehaviour
{
    public static MainMenu instance;
    public static bool HostMode; 
    public readonly SyncList<Match> matches = new SyncList<Match>();
    public readonly SyncList<string> matchIDs = new SyncList<string>();
    public int MaxPlayers;
    private NetworkManager _networkManager;
    
    [Header("MainMenu")]
    public Button HostButton;
    public Button JoinButton;
    public TMP_InputField RoomID;
    public TMP_InputField NikInput;


    [Header("Name")]
    public Button SetNameButtom;
    public TMP_InputField NameInput;
    public int firstTime = 1;
    [SyncVar] public string DisplayName;
    
    [Header("Lobby")]
    public Canvas LobbyCanvas;
    public Transform UIPlayerParent;
    public GameObject UIPlayerPrefab;
    public TMP_Text IDText;
    public Button StartGameButton;
    public GameObject localPlayerLobbyUI;
    public bool inGame;

    private Disconnect dicConnect;
    
    private void Start()
    {
        instance = this;
        dicConnect = new Disconnect();
        
        Debug.Log($"Запущена сцена lobby");

        _networkManager = GameObject.FindObjectOfType<NetworkManager>();
        firstTime = PlayerPrefs.GetInt("FirstTime", 1);
        
        if(!PlayerPrefs.HasKey("Name")) return;

        string defaultName = PlayerPrefs.GetString("Name");
        NameInput.text = defaultName;
        DisplayName = defaultName;
        SetName(defaultName);
    }

    private void Update()
    {
        if (inGame)
        {
            PlayerPrefs.SetInt("FirstTime", firstTime);
        }
    }

    public void SaveName()
    {
        //HostButton.interactable = false;
        //JoinButton.interactable = false;
        //RoomID.interactable = false;
        //Player.localPlayer.HostGame();
        firstTime = 0;
        DisplayName = NameInput.text;
        PlayerPrefs.SetString("Name", DisplayName);
        Invoke(nameof(Disconnect), 1f);
    }

    void Disconnect()
    {
        /*
        if (_networkManager.mode == NetworkManagerMode.Host)
        {
            _networkManager.StopHost();
        }
        else if (_networkManager.mode == NetworkManagerMode.ClientOnly)
        {
            _networkManager.StopClient();
        }
        */
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
        else if (NetworkServer.active)
        {
            NetworkManager.singleton.StopServer();
        }
    }
    
    public void SetName(string name)
    {
        SetNameButtom.interactable = !string.IsNullOrEmpty(name);
    }
    
    public void Host()
    {
        Debug.Log($"Запускаю Создание комнаты");
        HostButton.interactable = false;
        JoinButton.interactable = false;
        RoomID.interactable = false;
        //Chang
        Player.localPlayer.HostGame();
    }
    public void HostSuccess(bool success, string matchID)
    {
        if (success)
        {
            SetNameButtom.interactable = false;
            NikInput.interactable = false;
            LobbyCanvas.enabled = true;

            if (!localPlayerLobbyUI == null)
            {
                Destroy(localPlayerLobbyUI);
            }
            
            localPlayerLobbyUI = SpawnPlayerUIPrefab(Player.localPlayer);
            IDText.text = matchID;
        }
        else
        {
            Message.show("Создание команты", "Создание комнаты не удалось!");
            //SceneManager.LoadScene("MainMenu");
            HostButton.interactable = true;
            JoinButton.interactable = true;
            RoomID.interactable = true;
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
            SetNameButtom.interactable = false;
            NikInput.interactable = false;
            LobbyCanvas.enabled = true;
            if (!localPlayerLobbyUI == null)
            {
                Destroy(localPlayerLobbyUI);
            }
            localPlayerLobbyUI = SpawnPlayerUIPrefab(Player.localPlayer);
            IDText.text = matchID;
            StartGameButton.interactable = false;
        }
        else
        {
            Message.show("Подключение к комнате", "Подключение к комнате не удалось!\nВозможно введен неверный код комнаты!");
            RoomID.interactable = true;
            HostButton.interactable = true;
            JoinButton.interactable = true;
        }
    }

    public void DisconnectGame()
    {
        if (localPlayerLobbyUI != null)
        {
            Destroy(localPlayerLobbyUI);
        }
        else Invoke(nameof(Disconnect), 0.01f);


        Player.localPlayer.DisconnectGame();
        LobbyCanvas.enabled = false;
        RoomID.interactable = true;
        HostButton.interactable = true;
        JoinButton.interactable = true;
        SetNameButtom.interactable = true;
        NikInput.interactable = true;
        Message.showNotification("Вы вышли из комнаты");
        
    }
    
    public bool HostGame(string matchID, GameObject player)
    {
        if (!matchIDs.Contains(matchID))
        {
            matchIDs.Add(matchID);
            Match match = new Match(matchID, player);
            matches.Add(match);
            player.GetComponent<Player>().CurrentMatch = match;
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
                    if (!matches[i].inMatch && !matches[i].MatchFull)
                    {
                        matches[i].players.Add(player);
                        player.GetComponent<Player>().CurrentMatch = matches[i];
                        matches[i].players[0].GetComponent<Player>().PlayerCountUpdated(matches[i].players.Count);
                        if (matches[i].players.Count == MaxPlayers)
                        {
                            matches[i].MatchFull = true;
                        }

                        break;
                    }
                    else return false;
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
    public GameObject SpawnPlayerUIPrefab(Player player)
    {
        GameObject newUIPlayer = Instantiate(UIPlayerPrefab, UIPlayerParent);
        newUIPlayer.GetComponent<PlayerUI>().SetPlayer(player.PlayerDisplayName);
        return newUIPlayer;
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
                matches[i].inMatch = true;
                foreach(var player in matches[i].players)
                {
                    player.GetComponent<Player>().StartGame();
                }
                break;
            }
        }
    }
    
    public void SetBeginButtonActive(bool active)
    {
        StartGameButton.interactable = active;
    }

    public void PlayerDisconnected(GameObject player, string ID)
    {
        for (int i = 0; i < matches.Count; i++)
        {
            if (matches[i].ID == ID)
            {
                int playerIndex = matches[i].players.IndexOf(player);
                if (matches[i].players.Count > playerIndex) 
                    matches[i].players.RemoveAt(playerIndex);
                if (matches[i].players.Count == 0)
                {
                    matches.RemoveAt(i);
                    matchIDs.Remove(ID);
                }else
                {
                    matches[i].players[0].GetComponent<Player>().PlayerCountUpdated(matches[i].players.Count);

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

