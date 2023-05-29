using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{

    public int qqPlayer;
    
    public List<Field> moves = new List<Field>();
    public List<Player> players = new List<Player>();
    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<NetworkMatch>();
    }

    public void NextPlayer()
    {
        Player.localPlayer.NextPlayer(qqPlayer);

    }

    public void updateMove(List<Field> moves)
    {
        this.moves = moves;
    }
    public void updateQQ()
    {
        qqPlayer++;
        if (players.Count == qqPlayer)
        {
            qqPlayer = 0;
        }
        GetComponent<Renderer>().material.color = players[qqPlayer].playerColor;
    }
    public void UpdatePlayers(List<Player> players)
    {
        this.players = players;
        UIController.instance.updateListPlayersUI(this.players);
        
    }
    
    public void FillNodes()
    {
        var r = GameObject.Find("Route").GetComponent<Route>().childNodeList;
        moves.Clear();
        foreach (var node in r)
        {
            moves.Add(new Field(node.tag,node.transform.name,node.GetComponent<FieldInfo>().ToFieldInfoU()));
        }
        moves[0].countPlayer = players.Count;
        qqPlayer = 0;
        players[0].MyMove = true;
        GetComponent<Renderer>().material.color = players[qqPlayer].playerColor;
        
        Debug.Log("Начальные значения установлены"); 
    }
    public void updateMe(PlayerManager newValue)
    {
        this.players = newValue.players;
        this.moves = newValue.moves;
        this.qqPlayer = newValue.qqPlayer;
        Debug.Log("PlayerManager обновлен!");

    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Player.localPlayer.startMove(1);
        }
    }
}
