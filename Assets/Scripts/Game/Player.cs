using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;


public class Player : NetworkBehaviour
{
    public static Player localPlayer;
    [SyncVar] public string matchId;
    private NetworkMatch NetworkMatch;




    private void Start()
    {
        NetworkMatch = GetComponent<NetworkMatch>();
        if (isLocalPlayer)
        {
            localPlayer = this;
        }
        else
        {
           // MainMenu.instance.SpawnPlayerUIPrefab(this);
        }
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
    // Update is called once per frame
    void Update()
    {
        //if (hasAuthority)
        //{
        //    Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //    float speed = 6f * Time.deltaTime;
        //    transform.Translate(new Vector2(input.x * speed, input.y * speed));
            
        //}
    }
}
