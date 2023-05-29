using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using Unity.VisualScripting;


[System.Serializable]
public class MyMatch : NetworkBehaviour
{
    public int qqPlayer;
    public List<Player> players = new List<Player>();
    public Route currentRoute;


    public MyMatch(List<Player> players)
    {
        this.players = players;
        currentRoute = null;
        qqPlayer = 0;
    }

    public MyMatch()
    {
        qqPlayer = 0;
        this.players = null;
        currentRoute = null;
    }
}
public class Player : NetworkBehaviour
{
    public static Player localPlayer;
    [SyncVar] public string matchId;

    private NetworkMatch NetworkMatch;
    //public TextMesh NameDisplayText;
    [SyncVar(hook = "DisplayPlayerName")] public string PlayerDisplayName;
    
    [SyncVar(hook = nameof(OnColorChanged))] public Color playerColor;
    [SyncVar(hook = nameof(OnPositionChanged))] public Vector3 playerCord;
    
    public Match CurrentMatch;
  
    //[SyncVar (hook = nameof(OnMCChanged))]
    //[SyncObject] 
    public MyMatch MatchControl;

    public List<Player> PlayersWithMe;
    public PlayerManager playerManager;
    public GameObject PlayerLobbyUI;
    private Guid netIDGuid;

    public bool InGame;
    public int cash;

    public Route currentRoute;
    public int routePosition; 
    private int steps;
    private bool isMoving;

    [SyncVar(hook = nameof(ChangeMyMove))] public bool MyMove;
    public int DoubleCount;
    public int forSkip;

    private void Update()
    {
        
    }

    private void Awake()
    {
        NetworkMatch = GetComponent<NetworkMatch>();
    }

    private void Start()
    {
        InGame = false;
        cash = 1500;
        MatchControl = new MyMatch();
        MyMove = false;
        DoubleCount = 0;
        if (isLocalPlayer)
        {
            
            CmdSendName(MainMenu.instance.DisplayName);
        }
    }
    private void OnPositionChanged(Vector3 oldValue, Vector3 newValue)
    {
        // Update the player's material color to match their assigned color
        GetComponent<Transform>().position = newValue;
    }
    private void OnColorChanged(Color oldValue, Color newValue)
    {
        // Update the player's material color to match their assigned color
        GetComponent<Renderer>().material.color = newValue;
    }

    private void OnMCChanged(MyMatch oldValue, MyMatch newValue)
    {
        MatchControl = newValue;
    }
    public override void OnStartServer()
    {
        netIDGuid = netId.ToString().ToGuid();
        NetworkMatch.matchId = netIDGuid;
    }

    public void ChangeMyMove(bool oldValue, bool newValue)
    {
        MyMove = newValue;
        if (UIController.instance != null)
        {
            UIController.instance.updateUI();
        }
    }

    
    public override void OnStartClient()
    {
        if (isLocalPlayer) localPlayer = this;
        else PlayerLobbyUI = MainMenu.instance.SpawnPlayerUIPrefab(this);
    }

    public override void OnStopClient()
    {
        ClientDiscoonect();
    }
    

    public override void OnStopServer()
    {
        ServerDisconnect();
    }


    [Command]
    public void CmdSendName(string name)
    {
        PlayerDisplayName = name;
    }

    public void DisplayPlayerName(string name, string playerName)
    {
        name = PlayerDisplayName;
        Debug.Log("Имя " + name + " : " + playerName);
        //NameDisplayText.text = playerName;
    }
    
    public void HostGame()
    {
        
        string id = MainMenu.GetRandomID();
        CmdHostGame(id);
    }
    [Command]
    public void CmdHostGame(string ID)
    {
        matchId = ID;
        if (MainMenu.instance.HostGame(ID, gameObject))
        {
            Debug.Log("Комната была создана успешно!");
            NetworkMatch.matchId = ID.ToGuid();
            //MainMenu.instance.SpawnPlayerUIPrefab(this);
            TargetHostGame(true, ID);
        }
        else
        {
            Debug.Log("Ошибка в создании лобби");
            TargetHostGame(false, ID);
        }
    }
    [TargetRpc]
    void TargetHostGame(bool success, string ID)
    {
        matchId = ID;
        Debug.Log($"ID {matchId} == {ID}");
        MainMenu.instance.HostSuccess(success, ID);
    }

    public void DisconnectGame()
    {
        CmdDisconnectGame();
    }

    [Command]
    void CmdDisconnectGame()
    {
        ServerDisconnect();
    }

    void ServerDisconnect()
    {
        MainMenu.instance.PlayerDisconnected(gameObject, matchId);
        RpcDisconnectGame();
        NetworkMatch.matchId = netIDGuid;
    }

    [ClientRpc]
    void RpcDisconnectGame()
    {
        ClientDiscoonect();
    }

    void ClientDiscoonect()
    {
        if (PlayerLobbyUI != null)
        {
            if (!isServer) Destroy(PlayerLobbyUI);
            else PlayerLobbyUI.SetActive(false);
        }
        
    }

    [Server]
    public void PlayerCountUpdated(int playerCount)
    {
        TargetPlayerCountUpdated(playerCount);
    }

    [TargetRpc]
    void TargetPlayerCountUpdated(int playerCount)
    {
        if (playerCount > 1) MainMenu.instance.SetBeginButtonActive(true);
        else MainMenu.instance.SetBeginButtonActive(false);
    }
    
    public void JoinGame(string inputID)
    {
        CmdJoinGame(inputID);
    }
    [Command]
    public void CmdJoinGame(string ID)
    {
        matchId = ID;
        if (MainMenu.instance.JoinGame(ID, gameObject))
        {
            Debug.Log("Успешное подключение к лобби!");
            NetworkMatch.matchId = ID.ToGuid();
            //MainMenu.instance.SpawnPlayerUIPrefab(this);
            TargetJoinGame(true, ID);
        }
        else
        {
            Debug.Log("Не удалось подключиться!");
            TargetJoinGame(false, ID);
        }
    }
    [TargetRpc]
    void TargetJoinGame(bool success, string ID)
    {
        matchId = ID;
        Debug.Log($"ID {matchId} == {ID}");
        MainMenu.instance.JoinSuccess(success, ID);
    }

    public void setPM()
    {
        CmdSetPM(matchId);
    }

    [Command]
    public void CmdSetPM(string MatchID)
    {
        Debug.Log("Установка PlayerManager");
        MainMenu.instance.SetStartV(MatchID);
    }

    [TargetRpc]
    public void TargetStartPM(List<Player> pwm)
    {
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().UpdatePlayers(pwm);
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().FillNodes();
        Debug.Log("StartPM");
    }

    public void NextPlayer(int oldIndex)
    {
        if (forSkip != 0) forSkip--;
        CmdNextPlayer(matchId, oldIndex,GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().moves );
    }
    
    [Command]
    public void CmdNextPlayer(string MatchID, int oldIndex, List<Field> moves)
    {
        Debug.Log("Обновляю PlayerManager для " + MatchID);
        MainMenu.instance.NextPlayer(MatchID, oldIndex, moves);
    }
    
    [TargetRpc]
    public void TargetNextPlayer(List<Field> newRoute, List<Player> players)
    {
        Debug.Log("Мой PlayerManager обновляеться");
        //GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().UpdatePlayers(pwm);
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().updateMove(newRoute);
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().UpdatePlayers(players);
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().updateQQ();
    }

    public void payTo(int owner, int money)
    {
        CmdPayTo(matchId,owner, money);
    }

    [Command]
    public void CmdPayTo(string MatchID, int owner, int money)
    {
        MainMenu.instance.SendMoney(MatchID, owner , money);
    }

    [TargetRpc]
    public void TargetGetMoney(int money)
    {
        cash += money;
        UIController.instance.updateCash();
    }
    
    
    public void BeginGame()
    {
        CmdBeginGame();
    }
    [Command]
    public void CmdBeginGame()
    {
        Debug.Log("Игра началась!");
        MainMenu.instance.BeginGame(matchId);
        
    }

    public void StartGame()
    {
        InGame = true;
        TargetBeginGame();
    }
    [TargetRpc]
    void TargetBeginGame()
    {
        Debug.Log($"ID {matchId} | Начало");
        var players = FindObjectsOfType<Player>();
        for (int i = 0; i < players.Length; i++)
        {
            DontDestroyOnLoad(players[i]);
        }
        MainMenu.instance.inGame = true;
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
    }
    
    public void setRoute(Route route)
    {
        
        this.currentRoute = route;
        
    }
    public void startMove(int steps)
    {
        this.steps = steps;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        if (isMoving)
        {
            yield break;
        }

        isMoving = true;
        while (steps > 0)
        {

            GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().moves[routePosition].countPlayer--;
            //currentRoute.childNodeList[routePosition].countPlayer--;

            routePosition++;
            routePosition %= currentRoute.childNodeList.Count;

            Vector3 nextPos = currentRoute.childNodeList[routePosition].position + GameObject
                .FindWithTag("PlayerManager").GetComponent<PlayerManager>().moves[routePosition].getPos();
            //Vector3 nextPos = currentRoute.childNodeList[routePosition].node.transform.position;
            Debug.Log(GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().moves[routePosition].name);
            GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().moves[routePosition].countPlayer++;
            //Vector3 nextPos = currentRoute.childNodeList[routePosition].position;
            while (MoveToNextNode(nextPos))
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);
            steps--;
            if (routePosition == 0)
                GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().moves[0].onHere("GO");
        }


        isMoving = false;
        if (routePosition != 0)
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().moves[routePosition].onHere(currentRoute.childNodeList[routePosition].tag);
        if (DoubleCount != 0)
        {
            UIController.instance.bNextPlayer.interactable = false;
        }
        else
            UIController.instance.bNextPlayer.interactable = true;


    }

    bool MoveToNextNode(Vector3 goal)
    {
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, 8f * Time.deltaTime));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
        
    }
}
