using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[System.Serializable]
public class forMove : NetworkBehaviour
{
    public string name;
    public int countPlayer;
    public List<Vector3> spawndots = new List<Vector3>{
        new Vector3(1.2f,0,-1.2f),
        new Vector3(1.2f,0,0),
        new Vector3(1.2f,0,1.2f),
        new Vector3(0,0,-1.2f),
        new Vector3(0,0,0),
        new Vector3(0,0,1.2f),
        new Vector3(-1.2f,0,-1.2f),
        new Vector3(-1.2f,0,1.2f)
    };

    public forMove()
    {
        countPlayer = 0;
        spawndots = new List<Vector3>
        {
            new Vector3(1,0,-1),
            new Vector3(1,0,0),
            new Vector3(1,0,1),
            new Vector3(0,0,-1),
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(-1,0,-1),
            new Vector3(-1,0,1)
        };
    }

    public forMove(string name)
    {
        this.name = name;
        countPlayer = 0;
        spawndots = new List<Vector3>
        {
            new Vector3(1,0,-1),
            new Vector3(1,0,0),
            new Vector3(1,0,1),
            new Vector3(0,0,-1),
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(-1,0,-1),
            new Vector3(-1,0,1)
        };
    }
    
    public Vector3 getPos()
    {
        return spawndots[countPlayer];
    }
    
}

public class PlayerManager : NetworkBehaviour
{

    public int qqPlayer;
    
    public List<forMove> moves = new List<forMove>();
    public List<Player> players = new List<Player>();
    
    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }

    [Command]
    private void CmdNextPlayer()
    {
        players[qqPlayer].MyMove = false;
        qqPlayer++;
        if (players.Count == qqPlayer)
        {
            qqPlayer = 0;
        }
        players[qqPlayer].MyMove = true;

    }

    public void ToNextPlayer()
    {
        CmdNextPlayer();
    }
    
    
    

    public void UpdatePlayers(List<Player> players)
    {
        this.players = players;
        
    }
    public void FillNodes()
    {
        var r = GameObject.Find("Route").GetComponent<Route>().childNodeList;
        moves.Clear();
        foreach (var node in r)
        {
            moves.Add(new forMove(node.transform.name));
        }
        moves[0].countPlayer = players.Count;
        qqPlayer = 0;
        players[0].MyMove = true;
        Debug.Log("Начальные значения установлены"); 
    }


    public void updateMe(PlayerManager newValue)
    {
        this.players = newValue.players;
        this.moves = newValue.moves;
        this.qqPlayer = newValue.qqPlayer;

    }

    private void setVariables()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
