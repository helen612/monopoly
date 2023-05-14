using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public class XYZCoord
{
    public float x;
    public float y;
    public float z;

    public XYZCoord(float _x, float _y, float _z)
    {
        x = _x;
        y = _y;
        z = _z;
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
    
    [SyncVar] public Match CurrentMatch;
    public GameObject PlayerLobbyUI;
    private Guid netIDGuid;

    public bool InGame;
    public int cash;

    private Route currentRoute;
    private int routePosition;
    private int steps;
    private bool isMoving;
    
    
    private void Awake()
    {
        NetworkMatch = GetComponent<NetworkMatch>();
    }

    private void Start()
    {
        InGame = false;
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
    
    public override void OnStartServer()
    {
        netIDGuid = netId.ToString().ToGuid();
        NetworkMatch.matchId = netIDGuid;
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

    public void BeginGame()
    {
        CmdBeginGame();
    }
    [Command]
    public void CmdBeginGame()
    {
        MainMenu.instance.BeginGame(matchId);
        Debug.Log("Игра началась!");
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
        DontDestroyOnLoad(gameObject);
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
            routePosition++;
            routePosition %= currentRoute.childNodeList.Count;

            Vector3 nextPos = currentRoute.childNodeList[routePosition].position;
            while (MoveToNextNode(nextPos))
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);
            steps--;

        }

        isMoving = false;

    }

    bool MoveToNextNode(Vector3 goal)
    {
        return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, 8f * Time.deltaTime));
    }
    
    // Update is called once per frame
    
    void Update()
    {
        /*
        if (GetComponent<Transform>().position.y < 0)
        {
            
        }*/
        //if (hasAuthority)
        //{
        //    Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //    float speed = 6f * Time.deltaTime;
        //    transform.Translate(new Vector2(input.x * speed, input.y * speed));
            
        //}
    }
}
