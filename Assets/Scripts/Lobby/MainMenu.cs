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
    public List<GameObject> players = new List<GameObject>();
    public PlayerManager PM;
    
    
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

    public GameObject _playerManager;
    public GameObject _House;
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
        GameObject obj = Instantiate(_playerManager);
        NetworkServer.Spawn(obj);
        obj.GetComponent<NetworkMatch>().matchId = matchID.ToGuid();
        PlayerManager newPM = obj.GetComponent<PlayerManager>();
        
        for (int i = 0; i < matches.Count; i++)
        {
            if (matches[i].ID == matchID)
            {
                matches[i].inMatch = true;
                List<Color> ColorPlayers = GetRandomColor(matches[i].players.Count);
                List<Vector3> beginCoord = new List<Vector3>
                {
                    new Vector3(-19.3f, 23.0f, 122.0f),
                    new Vector3(-19.3f, 23.0f, 120.7f),
                    new Vector3(-18.2f, 23.0f, 122.0f),
                    new Vector3(-18.2f, 23.0f, 120.7f),
                    new Vector3(-17.1f, 23.0f, 122.0f),
                    new Vector3(-17.1f, 23.0f, 120.7f),
                    new Vector3(-16.0f, 23.0f, 122.0f),
                    new Vector3(-16.0f, 23.0f, 120.7f)
                };
                foreach (var player in matches[i].players)
                {
                    var _player = player.GetComponent<Player>();
                    _player.CurrentMatch = matches[i];
                    
                    _player.playerColor = ColorPlayers[0];
                    ColorPlayers.RemoveAt(0);
                    _player.playerCord = beginCoord[0];
                    beginCoord.RemoveAt(0);
                    
                    _player.playerManager = newPM;
                    player.GetComponent<Player>().StartGame();

                }

                break;
            }
        }
    }

    public void SetStartV(string matchID)
    {
        for (int i = 0; i < matches.Count; i++)
        {
            if (matches[i].ID == matchID)
            {
                //получение игроков с сервера
                var pwm = new List<Player>();
                foreach (var p in matches[i].players)
                {
                    
                    pwm.Add(p.GetComponent<Player>());
                }
                
                matches[i].players[0].GetComponent<Player>().MyMove = true;
                for (int ip  = 0; ip < matches[i].players.Count; ip++)
                {
                    matches[i].players[0].GetComponent<Player>().PlayersWithMe = pwm;
                    matches[i].players[ip].GetComponent<Player>().TargetStartPM(pwm);
                    //matches[i].players[ip].GetComponent<Player>().updatePm();
                }
                
            }
        }
    }

    public void NextPlayer(string matchID, int oldIndex, List<Field> moves, List<cards> cardsQueue)
    {
        for (int i = 0; i < matches.Count; i++)
        {
            if (matches[i].ID == matchID)
            {
                //получение игроков с сервера
                var pwm = new List<Player>();
                foreach (var p in matches[i].players)
                {
                    pwm.Add(p.GetComponent<Player>());
                }
                
                matches[i].players[oldIndex].GetComponent<Player>().MyMove = false;
                
                oldIndex++;
                if (pwm.Count == oldIndex)
                {
                    oldIndex = 0;
                }
                matches[i].players[oldIndex].GetComponent<Player>().MyMove = true;
                
                for (int ip = 0; ip < matches[i].players.Count; ip++)
                {
                    matches[i].players[ip].GetComponent<Player>().TargetNextPlayer(moves,pwm, cardsQueue);
                    //matches[i].players[ip].GetComponent<Player>().updatePm();
                }

            }
        }

    }

    public void SendMoney(string matchID,Color owner, int money)
    {
        for (int i = 0; i < matches.Count; i++)
        {
            if (matches[i].ID == matchID)
            {
                for (int j = 0; j < matches[i].players.Count; j++)
                {
                    var color = matches[i].players[j].GetComponent<Player>().playerColor;
                    if (owner == color)
                    {
                        matches[i].players[j].GetComponent<Player>().TargetGetMoney(money);
                    }
                }
                

            }
        }
    }

    public void SpawnHouse(string matchID, Vector3 housePos, int position)
    {
        for (int i = 0; i < matches.Count; i++)
        {
            if (matches[i].ID == matchID)
            {
                GameObject obj = Instantiate(_House, housePos,Quaternion.identity);
                NetworkServer.Spawn(obj);
                obj.GetComponent<NetworkMatch>().matchId = matchID.ToGuid();
                for (int ip  = 0; ip < matches[i].players.Count; ip++)
                {
                    matches[i].players[ip].GetComponent<Player>().TargetSpawnHouse(obj, position);
                }
            }
        }
    }
    
    public void SpawnHotel(string matchID, Vector3 housePos, int position)
    {
        for (int i = 0; i < matches.Count; i++)
        {
            if (matches[i].ID == matchID)
            {
                
                GameObject obj = Instantiate(_House, housePos,Quaternion.identity);
                obj.GetComponent<Renderer>().material.color = Color.red;
                NetworkServer.Spawn(obj);
                obj.GetComponent<NetworkMatch>().matchId = matchID.ToGuid();
                for (int ip  = 0; ip < matches[i].players.Count; ip++)
                {
                    matches[i].players[ip].GetComponent<Player>().TargetSpawnHouse(obj, position);
                }
            }
        }
    }

    public void destroyHotel(string matchID, int position, Vector3 HousePos, Vector3 posNode)
    {
        for (int i = 0; i < matches.Count; i++)
        {
            if (matches[i].ID == matchID)
            {
                int r = position / 10;
                List<GameObject> houses = new List<GameObject>();
                switch (r)
                {
                    case 0:
                    {
                        posNode.z += HousePos.z;
                        posNode.x += HousePos.x;
                        var st = posNode.x;
                        for (int j = 0; j < 4; j++)
                        {
                            posNode.x = st;
                            posNode.x += j * HousePos.y;
                            houses.Add(spawnHouse(posNode, matchID));
                        }
                        
                
                        break;
                    }
                    case 1:
                    {
                        posNode.z += HousePos.z;
                        posNode.x += HousePos.x;
                        var st = posNode.z;

                        for (int j = 0; j < 4; j++)
                        {
                            posNode.z = st;
                            posNode.z += j * HousePos.y;
                            houses.Add(spawnHouse(posNode, matchID));
                        }
                        break;
                    }
                    case 2:
                    {
                        posNode.z += HousePos.z;
                        posNode.x += HousePos.x;
                
                        var st = posNode.x;
                        for (int j = 0; j < 4; j++)
                        {
                            posNode.x = st;
                            posNode.x += j * HousePos.y;
                            houses.Add(spawnHouse(posNode, matchID));
                        }
                        break;
                    }
                    case 3:
                    {
                        posNode.z += HousePos.z;
                        posNode.x += HousePos.x;
                
                        var st = posNode.z;

                        for (int j = 0; j < 4; j++)
                        {
                            posNode.z = st;
                            posNode.z += j * HousePos.y;
                            houses.Add(spawnHouse(posNode, matchID));
                        }
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
                for (int ip  = 0; ip < matches[i].players.Count; ip++)
                {
                    matches[i].players[ip].GetComponent<Player>().TargetSpawnHouses(houses, position);
                }
            }
        }
    }

    public GameObject spawnHouse(Vector3 housePos,string matchID)
    {
        GameObject obj = Instantiate(_House, housePos,Quaternion.identity);
        NetworkServer.Spawn(obj);
        obj.GetComponent<NetworkMatch>().matchId = matchID.ToGuid();
        return obj;
    }
    
    public List<Player> getMatchPlayers(string matchID)
    {
        for (int i = 0; i < matches.Count; i++)
        {
            if (matches[i].ID == matchID)
            {
                var res = new List<Player>();
                foreach(var player in matches[i].players)
                {
                    res.Add(player.GetComponent<Player>());
                }

                return res;
                //return new MyMatch(matches[i].players);
            }
        }

        return null;
    }

    private List<T> Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }
    private List<Color> GetRandomColor(int countPlayers)
    {
        List<Color> AvailableColors = new List<Color>
        {
            Color.red,
            Color.blue,
            Color.green,
            Color.yellow,
            Color.cyan,
            Color.magenta,
            Color.grey,
            Color.black
        };
        // Shuffle the list of available colors and return the first one
        List<Color> result = new List<Color>();
        for (int i = 0; i < countPlayers; i++)
        {
            AvailableColors = Shuffle(AvailableColors);
            result.Add(AvailableColors[0]);
            AvailableColors.RemoveAt(0);
        }
        return result;
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

