using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{

    public int qqPlayer;
    
    public List<Field> moves = new List<Field>();
    public List<Player> players = new List<Player>();
    public Queue<cards> CardsList = new Queue<cards>();

    public List<Vector3> Houses = new List<Vector3>
    {
        new Vector3(-1.28f, 0.8f, -2.79f),
        new Vector3(-2.79f, -0.8f, 1.28f),
        new Vector3(1.28f, -0.8f, 2.79f),
        new Vector3(2.79f, 0.8f, -1.28f)
    };
    public bool forCom = false;
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

    public void updateCards(List<cards> queue)
    {
        this.CardsList = new Queue<cards>(queue);
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
        FieldCards();
    }

    public void FieldCards()
    {
        CardsList.Enqueue( new cards("Отпускные. Получите М100", 100));
        CardsList.Enqueue( new cards(0,"Идите на поле \"Вперед\""));
        CardsList.Enqueue( new cards("Ссуда на строительство. Получите М150", 150));
        CardsList.Enqueue(new cards(39,"Отправляйтесь на ул. Арбат" ));
        CardsList.Enqueue( new cards("Отправляйтесь прямо в тюрьму. Вы не проходите поле \"Вперед\" и не получаете М200", true));
        CardsList.Enqueue( new cards("Вас избрали председателем совета директоров, заплатите в банк М100", -100));
        CardsList.Enqueue( new cards("Вы заняли первое место на конкурсе красоты. Получите М10 ", 10));
        CardsList.Enqueue( new cards(5,"Отправляйтесь в путешествие до Рижской железной дороги"));
        CardsList.Enqueue( new cards("Банк платит вам диведенты в размере М50", 50));
        CardsList.Enqueue( new cards("Вас избрали председателем совета директоров, заплатите в банк 100", 100));
        CardsList.Enqueue( new cards("Банковская ошибка в вашу пользу. Получите М200", 200));
        CardsList.Enqueue( new cards(24,"Отправляйтесь площадь Маяковского."));
        CardsList.Enqueue( new cards("Выйти из тюрьмы бесплатно. Эта карточка используеться автоматически.", false));
        CardsList.Enqueue( new cards("Штраф за превышение скорости. Заплатите М15", -15));
        CardsList.Enqueue( new cards("Возврат подоходного налога. Получите М20", 20));
        CardsList.Enqueue( new cards("Оплата страховки. Получите М100", 100));
        CardsList.Enqueue( new cards("На продаже акций вы зарабатываете М50", 50));
        CardsList.Enqueue( new cards("Расходы на лечение у врача. Заплатите М50", -50));
        CardsList.Enqueue( new cards("Расходы на лечение в больнице. Заплатите М100", -100));
        CardsList.Enqueue( new cards(11,"Отправляйтесь на ул. Полянка"));
        CardsList.Enqueue( new cards("Вы получаете наследство М100", 100));
        CardsList.Enqueue( new cards("Отправляйтесь прямо в тюрьму. Вы не проходите поле \"Вперед\" и не получаете М200", true));
        CardsList.Enqueue( new cards("Выйти из тюрьмы бесплатно. Эта карточка используеться автоматически.", false));
        CardsList.Enqueue( new cards(0,"Идите на поле \"Вперед\" получите М200"));
        CardsList.Enqueue( new cards("Расходы на обучение. Заплатите М50", -50));
        CardsList.Enqueue( new cards("Получите за консультацию М25", 25));
        
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

        if (Input.GetKeyDown(KeyCode.W))
        {
            Player.localPlayer.startMove(2);
        }
    }
}
