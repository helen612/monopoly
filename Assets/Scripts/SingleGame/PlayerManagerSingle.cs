using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldSingle
{

    public string name;
    public int countPlayer;
    public FieldInfoU field;
    public List<GameObject> houses;
    public List<Vector3> spawndots = new List<Vector3>
    {
        new Vector3(1.2f, 0, -1.2f),
        new Vector3(1.2f, 0, 0),
        new Vector3(1.2f, 0, 1.2f),
        new Vector3(0, 0, -1.2f),
        new Vector3(0, 0, 0),
        new Vector3(0, 0, 1.2f),
        new Vector3(-1.2f, 0, -1.2f),
        new Vector3(-1.2f, 0, 1.2f)
    };
 
    public FieldSingle()
    {
        countPlayer = 0;
        spawndots = new List<Vector3>
        {
            new Vector3(1, 0, -1),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1),
            new Vector3(0, 0, -1),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(-1, 0, -1),
            new Vector3(-1, 0, 1)
        };
        field = null;
        houses = new List<GameObject>();
    }

    public FieldSingle(string tag,string name, FieldInfoU field)
    {
        this.name = name;
        countPlayer = 0;
        spawndots = new List<Vector3>
        {
            new Vector3(1, 0, -1),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1),
            new Vector3(0, 0, -1),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(-1, 0, -1),
            new Vector3(-1, 0, 1)
        };
        this.field = field;
        this.field.tag = tag;
        houses = new List<GameObject>();
    }

    public Vector3 getPos()
    {
        return spawndots[countPlayer];
    }

    
    
    public void onHere(string tag)
    {
        switch (tag)
        {
            case "GO":
                stayGo();
                break;
            case "Street":
                stayStreet();
                break;
            case "TR":
                stayTR();
                break;
            case "Tax":
                stayTax();
                break;
            case "Com":
                stayCom();
                break;
            case "Free":
                Debug.Log("Бесплатное место");
                break;
            case "PT":
                StayPT();
                break;
            case "Chanse":
                stayChance();
                break;
            case "Jail":
                GoJail();
                break;
        }

    }

    private void stayGo()
    {
        Debug.Log("GO");
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().changeMoney(200);
    }
    private void GoJail()
    {
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().GoJail();
        
        
    }
    private void stayTax()
    {
        Debug.Log("Pay " + field.rent);
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().changeMoney(-1 * field.rent);
    }
    
    private void StayPT()
    {
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().popUpCard();
    }

    private void stayChance()
    {
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().popUpCard();
    }
     
    
    private void stayStreet()
    {
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().street(field);
    }
    

    private void stayTR()
    {
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().trainRoad(field);
        
        
    }
    
    private void stayCom()
    {
        Debug.Log("Com " + field.fullName);
        var movingPlayer = GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>()
            .Players[GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().QqPlayer];
        if(movingPlayer.GetComponent<PlayerSingle>().playerColor == field.owner || field.owner == Color.white) return;
        
        GameObject.Find("Dice1").GetComponent<DiceRoller>().RollDice(1);
        GameObject.Find("Dice2").GetComponent<DiceRoller>().RollDice(2);
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().forCom = true;
    }

    public void PayCom(int value)
    {
        
        switch (field.level)
        {
            case 0:
            {
                Debug.Log("Платить ренту не надо!");
                break;
            }
            case 3:
            {
                payToOwner(field.home1 * value);
                break;
            }
            case 4:
            {
                payToOwner(field.home2 * value);
                break;
            }
        }
    }

    private void payToOwner(int money)
    {
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManagerSingle>().PayTo(field.owner, money);
    }
    
}


public class PlayerManagerSingle : MonoBehaviour
{
    //префаб дома/отеля
    public GameObject _House;
    //инлдекс игрока который сейчас ходит
    public int QqPlayer;
    // количество игроков
    public int CountPlayers;
    //префаб игрока
    public GameObject PlayerPrefab;
    
    //список игроков
    public List<GameObject> Players;
    //список изменений на дороге
    public List<FieldSingle> moves = new List<FieldSingle>();
    //карточки (общ казна и шанс)
    public Queue<cards> CardsList = new Queue<cards>();
    //расположение домов на карте (множетели)
    public List<Vector3> Houses = new List<Vector3>
    {
        new Vector3(-1.28f, 0.8f, -2.79f),
        new Vector3(-2.79f, -0.8f, 1.28f),
        new Vector3(1.28f, -0.8f, 2.79f),
        new Vector3(2.79f, 0.8f, -1.28f)
    };
    //проверка на бросание кубика на ком предприятии
    public bool forCom;
    
    //текущий маршрут
    public Route currentRoute;

    //координаты для спавна
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

    private List<Color> ColorPlayers;


    #region stayHere

     public void GoJail()
    {
        if ( Players[QqPlayer].GetComponent<PlayerSingle>().FreeJail == 0)
        {
            Debug.Log("Отправляйтесь в тюрьму");
            moves[Players[QqPlayer].GetComponent<PlayerSingle>().routePosition].countPlayer--;
            Players[QqPlayer].GetComponent<PlayerSingle>().transform.position = GameObject.Find("Jail").transform.position;
            Players[QqPlayer].GetComponent<PlayerSingle>().routePosition = 10;
            moves[Players[QqPlayer].GetComponent<PlayerSingle>().routePosition].countPlayer++;
            Players[QqPlayer].GetComponent<PlayerSingle>().forSkip = 4;  
        }
        else
        {
            Message.show("Шанс", "Вы не попадаете в тюрьму и ваша карточка сгорает");
            Players[QqPlayer].GetComponent<PlayerSingle>().FreeJail--;
        }
    }

    public void popUpCard()
    {
        var r = CardsList.Dequeue();
        CardsList.Enqueue(r);
        Message.showSingle("Карточка",r.text);

        switch (r.mode)
        {
            case "move":
            {
                int rSteps = 0;
                if (Players[QqPlayer].GetComponent<PlayerSingle>().routePosition > r.move)
                {
                    rSteps = 40 - Players[QqPlayer].GetComponent<PlayerSingle>().routePosition;
                    rSteps += r.move;
                }
                else
                {
                    rSteps = r.move - Players[QqPlayer].GetComponent<PlayerSingle>().routePosition;
                }
                Players[QqPlayer].GetComponent<PlayerSingle>().startMove(rSteps);
                break;
            }
            case "money":
            {
                changeMoney(r.money);
                break;
            }
            case "jail":
            {
                if (r.jail)
                {
                    GoJail();
                }
                else
                {
                    Players[QqPlayer].GetComponent<PlayerSingle>().FreeJail++;
                }
                break;
            }
            default:
            {
                Debug.Log("карточку невозможно вытянуть");
                break;
            }
        }
    }

    public void street(FieldInfoU field)
    {
        Debug.Log("Улица" + field.fullName);
        if(Players[QqPlayer].GetComponent<PlayerSingle>().playerColor == field.owner) return;
        switch (field.level)
        {
            case 0:
            {
                Debug.Log("Платить ренту не надо!");
                break;
            }
            case 1:
            {
                PayTo(field.owner,field.rent);
                break;
            }
            case 2:
            {
                PayTo(field.owner,field.rentGroup);
                break;
            }
            case 3:
            {
                PayTo(field.owner,field.home1);
                break;
            }
            case 4:
            {
                PayTo(field.owner,field.home2);
                break;
            }
            case 5:
            {
                PayTo(field.owner,field.home3);
                break;
            }
            case 6:
            {
                PayTo(field.owner,field.home4);
                break;
            }
            case 7:
            {
                PayTo(field.owner,field.hotel);
                break;
            }
        }
    }

    public void trainRoad(FieldInfoU field)
    {
        Debug.Log("Train Road " + field.fullName);
        if(Players[QqPlayer].GetComponent<PlayerSingle>().playerColor == field.owner) return;
        switch (field.level)
        {
            case 0:
            {
                Debug.Log("Платить ренту не надо!");
                break;
            }
            case 3:
            {
                PayTo(field.owner,field.home1);
                break;
            }
            case 4:
            {
                PayTo(field.owner,field.home2);
                break;
            }
            case 5:
            {
                PayTo(field.owner,field.home3);
                break;
            }
            case 6:
            {
                PayTo(field.owner,field.home4);
                break;
            }
        }
    }
    

    #endregion

    #region Spawners

    public void SpawnPlayers()
    {
        for (int i = 0; i < CountPlayers; i++)
        {
            GameObject obj = Instantiate(PlayerPrefab);
            obj.GetComponent<PlayerSingle>().playerCord = beginCoord[0];
            obj.GetComponent<Transform>().position = beginCoord[0];
            beginCoord.RemoveAt(0);
            obj.GetComponent<PlayerSingle>().playerColor = ColorPlayers[0];
            obj.GetComponent<Renderer>().material.color = ColorPlayers[0];
            ColorPlayers.RemoveAt(0);
            Players.Add(obj);
            obj.GetComponent<PlayerSingle>().cash = 1500;
        }
    }

    public GameObject spawnHouse(int PositionOnRoad)
    {
        var posNode = currentRoute.childNodeList[PositionOnRoad].position;
        int mode = PositionOnRoad / 10;
        var Multipliers = Houses[mode];
        switch (mode) 
        {
            case 0:
            {
                posNode.z += Multipliers.z;
                posNode.x += Multipliers.x;
                
                posNode.x += moves[PositionOnRoad].houses.Count * Multipliers.y;
                
                break;
            }
            case 1:
            {
                posNode.z += Multipliers.z;
                posNode.x += Multipliers.x;
                
                posNode.z += moves[PositionOnRoad].houses.Count * Multipliers.y;
                
                break;
            }
            case 2:
            {
                posNode.z += Multipliers.z;
                posNode.x += Multipliers.x;
                
                posNode.x += moves[PositionOnRoad].houses.Count * Multipliers.y;
                
                break;
            }
            case 3:
            {
                posNode.z += Multipliers.z;
                posNode.x += Multipliers.x;
                
                posNode.z += moves[PositionOnRoad].houses.Count * Multipliers.y;
                
                break;
            }
        }
        return Instantiate(_House, posNode,Quaternion.identity);
    }

    public GameObject spawnHotel(int PositionOnRoad)
    {
        var posNode = currentRoute.childNodeList[PositionOnRoad].position;
        int r = PositionOnRoad / 10;
        var HousePos = Houses[r];
        switch (r)
        {
            case 0:
            {
                posNode.z += HousePos.z;
                
                break;
            }
            case 1:
            {
                posNode.x += HousePos.x;
                break;
            }
            case 2:
            {
                posNode.z += HousePos.z;
                break;
            }
            case 3:
            {
                posNode.x += HousePos.x;
                break;
            }
            default:
            {
                break;
            }
        }
        return Instantiate(_House, posNode,Quaternion.identity);
    }
    #endregion
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Players[QqPlayer].GetComponent<PlayerSingle>().startMove(1);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Players[QqPlayer].GetComponent<PlayerSingle>().startMove(2);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            changeMoney(-1499);
        }
    }

    public void Start()
    {
        
        QqPlayer = 0;
        forCom = false;
        CountPlayers = PlayerPrefs.GetInt("CountPlayers");
        ColorPlayers = GetRandomColor(CountPlayers);
        SpawnPlayers();
        FillNodes();
        CardsList = new Queue<cards>(Shuffle(CardsList.ToList()));
        var pl = Players[QqPlayer].GetComponent<PlayerSingle>();
        UIControllerSingle.instance.updateCash(pl.cash, pl.playerColor);
    }
    
    public void updateQQ()
    {
        if (Players[QqPlayer].GetComponent<PlayerSingle>().forSkip == 0) Players[QqPlayer].GetComponent<PlayerSingle>().forSkip--;
        Players[QqPlayer].GetComponent<PlayerSingle>().MyMove = false;
        for (int i = 0; i < Players[QqPlayer].GetComponent<PlayerSingle>().Owns.Count; i++)
        {
            Players[QqPlayer].GetComponent<PlayerSingle>().Owns[i].SetActive(false);
        }
        QqPlayer++;
        if (QqPlayer >= Players.Count )
        {
            QqPlayer = 0;
        }
        Players[QqPlayer].GetComponent<PlayerSingle>().MyMove = true;
        for (int i = 0; i < Players[QqPlayer].GetComponent<PlayerSingle>().Owns.Count; i++)
        {
            Players[QqPlayer].GetComponent<PlayerSingle>().Owns[i].SetActive(true);
        }
        GetComponent<Renderer>().material.color = Players[QqPlayer].GetComponent<PlayerSingle>().playerColor;
        var pl = Players[QqPlayer].GetComponent<PlayerSingle>();
        UIControllerSingle.instance.updateCash(pl.cash, pl.playerColor);
        if (Players[QqPlayer].GetComponent<PlayerSingle>().forSkip == 0)
        {
            UIControllerSingle.instance.bDragRoll.interactable = true;
            UIControllerSingle.instance.bNextPlayer.interactable = false;
        }
        else
        {
            UIControllerSingle.instance.bDragRoll.interactable = false;
            UIControllerSingle.instance.bNextPlayer.interactable = true;
        }



    }
    
    public void FillNodes()
    {
        var r = GameObject.Find("Route").GetComponent<Route>().childNodeList;
        moves.Clear();
        foreach (var node in r)
        {
            moves.Add(new FieldSingle(node.tag,node.transform.name,node.GetComponent<FieldInfo>().ToFieldInfoU()));
        }
        moves[0].countPlayer = Players.Count;
        GetComponent<Renderer>().material.color = Players[QqPlayer].GetComponent<PlayerSingle>().playerColor;
        
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

    

    public void PayTo(Color owner, int money)
    {
        Players[QqPlayer].GetComponent<PlayerSingle>().cash -= money;
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].GetComponent<PlayerSingle>().playerColor == owner)
            {
                Players[i].GetComponent<PlayerSingle>().cash += money;
            }
        }
        UIControllerSingle.instance.updateCash
            (Players[QqPlayer].GetComponent<PlayerSingle>().cash,
                Players[QqPlayer].GetComponent<PlayerSingle>().playerColor);
        if(Players[QqPlayer].GetComponent<PlayerSingle>().cash < 0) toLose();
    }

    public void changeMoney(int money)
    {
        Players[QqPlayer].GetComponent<PlayerSingle>().cash += money;
        UIControllerSingle.instance.updateCash(Players[QqPlayer].GetComponent<PlayerSingle>().cash,
            Players[QqPlayer].GetComponent<PlayerSingle>().playerColor);
        if(Players[QqPlayer].GetComponent<PlayerSingle>().cash < 0) toLose();
    }

    public void dicesDroped(int first, int second)
    {
        if (forCom)
        {
            moves[Players[QqPlayer].GetComponent<PlayerSingle>().routePosition].PayCom(first + second);
            forCom = false;
            //first = 0;
            //second = 0;
            return;
        }
              
        if (first == second)
        {
            Players[QqPlayer].GetComponent<PlayerSingle>().DoubleCount++;
            //UIController.instance.bNextPlayer.interactable = false;
            UIControllerSingle.instance.bDragRoll.interactable = true;
            if (Players[QqPlayer].GetComponent<PlayerSingle>().DoubleCount == 3)
            {
                Players[QqPlayer].GetComponent<PlayerSingle>().DoubleCount = 0;
                UIControllerSingle.instance.bDragRoll.interactable = false;
            }
            else 
                Players[QqPlayer].GetComponent<PlayerSingle>().startMove(first + second);
        }
        else
        {
            Players[QqPlayer].GetComponent<PlayerSingle>().DoubleCount = 0;
            Players[QqPlayer].GetComponent<PlayerSingle>().startMove(first + second);
        }
    }

    public FieldSingle changeOwner()
    {
      
        //списываем деньги со счета
        var curPos = Players[QqPlayer].GetComponent<PlayerSingle>().routePosition;
        changeMoney(-1 * moves[curPos].field.cost);
        //Устанавливаем владельца улице
        moves[curPos].field.owner = Players[QqPlayer].GetComponent<PlayerSingle>().playerColor;
        
        //Добавляем кнопку в магазин
        var newButtonOwner = ShopUiControllerSingle.instance.spawnButtowOwber();
        newButtonOwner.GetComponent<OwnButtonUISingle>().SetOwnName(moves[curPos].field.fullName, moves[curPos].field.Color, curPos);
        //получаем наш field
        setLevel(moves[curPos]);
        Players[QqPlayer].GetComponent<PlayerSingle>().Owns.Add(newButtonOwner);
        return moves[curPos];
    }
    
    
    public int getCountInFieldGroup(Color groupColor)
    {
        int streetsInGroup = 0;
        foreach (var fiels in moves)
        {
            if (fiels.field.Color == groupColor)
            {
                streetsInGroup++;
            }
        }

        return streetsInGroup;
    }
    public int getCountBoughtInGroupGroup(Color groupColor, Color Player)
    {
        int MyOwn = 0;
        foreach (var fiels in moves)
        {
            if (fiels.field.Color == groupColor)
            {
                if (fiels.field.owner == Player)
                {
                    MyOwn++;
                }
            }
        }

        return MyOwn;
    }

    public void setFieldLevel(Color groupColor, Color owner, int level)
    {
        for (int i = 0; i < moves.Count; i++)
        {
            if (groupColor == moves[i].field.Color &&
                owner == moves[i].field.owner)
            { 
                moves[i].field.level = level;
            }
        }
    }
    
    public void setLevel(FieldSingle field)
    {
        int countOwn = 0;
        countOwn = getCountBoughtInGroupGroup(field.field.Color, Players[QqPlayer].GetComponent<PlayerSingle>().playerColor);
        switch (field.field.tag)
        {
            case "Street":
            {
                if (getCountInFieldGroup(field.field.Color) == countOwn)
                {
                    setFieldLevel(field.field.Color,Players[QqPlayer].GetComponent<PlayerSingle>().playerColor,2);
                }
                else
                {
                    setFieldLevel(field.field.Color,Players[QqPlayer].GetComponent<PlayerSingle>().playerColor,1);
                }
                break;
            }
            case "TR":
            {
                setFieldLevel(field.field.Color,Players[QqPlayer].GetComponent<PlayerSingle>().playerColor, 2 + countOwn);
                break;
            }
            case "Com":
            {
                setFieldLevel(field.field.Color,Players[QqPlayer].GetComponent<PlayerSingle>().playerColor, 2 + countOwn);
                break;
            }
            default:
            {
                Debug.Log("Не удалось установить уровень");
                break;
            }
        }
 
    }

    public FieldSingle addHouseOnStreet(int choosenRoutePosition)
    {
        changeMoney(moves[choosenRoutePosition].field.costBuild * -1);
        moves[choosenRoutePosition].field.level++;
        if (moves[choosenRoutePosition].field.level > 2 && moves[choosenRoutePosition].field.level < 7)
        {
            var house = spawnHouse(choosenRoutePosition);
            moves[choosenRoutePosition].houses.Add(house);
        }
        else
        {

            foreach (var vhouse in moves[choosenRoutePosition].houses)
            {
                Destroy(vhouse);
            }
            moves[choosenRoutePosition].houses.Clear();
            var hotel = spawnHotel(choosenRoutePosition);
            hotel.GetComponent<Renderer>().material.color = Color.red;
            moves[choosenRoutePosition].houses.Add(hotel);
        }

        return moves[choosenRoutePosition];
    }

    public FieldSingle destroyHouseOnStreet(int choosenRoutePosition)
    {
        
        changeMoney(moves[choosenRoutePosition].field.costBuild/2);
        moves[choosenRoutePosition].field.level--;
        if (moves[choosenRoutePosition].field.level == 6)
        {
            Destroy(moves[choosenRoutePosition].houses[0]);
            moves[choosenRoutePosition].houses.Clear();
            for (int i = 0; i < 4; i++)
            {
                var house = spawnHouse(choosenRoutePosition);
                moves[choosenRoutePosition].houses.Add(house);
            }
        }
        else if (moves[choosenRoutePosition].field.level >= 2 )
        {
            var indexHouse = moves[choosenRoutePosition].houses.Count - 1;
            Destroy(moves[choosenRoutePosition].houses[indexHouse]);
            moves[choosenRoutePosition].houses.RemoveAt(indexHouse);
        }

        return moves[choosenRoutePosition];
    }

    public void toLose()
    {
        var loser = Players[QqPlayer];
        Players.RemoveAt(QqPlayer);
        for (int i = 0; i < moves.Count; i++)
        {
            if (moves[i].field.owner == loser.GetComponent<PlayerSingle>().playerColor)
            {
                moves[i].field.owner = Color.white;
                moves[i].field.level = 0;
                foreach (var house in moves[i].houses)
                {
                    Destroy(house);
                }
                moves[i].houses.Clear();
            }
        }
        
        moves[loser.GetComponent<PlayerSingle>().routePosition].countPlayer--;
        
        
        
        UIControllerSingle.instance.updateCash(Players[QqPlayer].GetComponent<PlayerSingle>().cash, Players[QqPlayer].GetComponent<PlayerSingle>().playerColor);
        GetComponent<Renderer>().material.color = Players[QqPlayer].GetComponent<PlayerSingle>().playerColor;
        if (Players[QqPlayer].GetComponent<PlayerSingle>().forSkip == 0)
        {
            UIControllerSingle.instance.bDragRoll.interactable = true;
            UIControllerSingle.instance.bNextPlayer.interactable = false;
        }
        else
        {
            UIControllerSingle.instance.bDragRoll.interactable = false;
            UIControllerSingle.instance.bNextPlayer.interactable = true;
        }

        if (Players.Count == 1)
        {
            Message.showSingle("Конец игры!", $"<color=#{ColorUtility.ToHtmlStringRGB(loser.GetComponent<PlayerSingle>().playerColor)}>Игрок</color> " +
                                           $"разорился и выбывает из игры. Монополистом становиться: " +
                                           $"<color=#{ColorUtility.ToHtmlStringRGB(Players[QqPlayer].GetComponent<PlayerSingle>().playerColor)}>Игрок</color>");
        }
        else
        {
            Message.showSingle("Банкрот!", $"<color=#{ColorUtility.ToHtmlStringRGB(loser.GetComponent<PlayerSingle>().playerColor)}>Игрок</color> " +
                                           $"разорился и выбывает из игры. все долги были выплачены банкам, а его исущество онулировано!\n");
        }

        foreach (var buttons in loser.GetComponent<PlayerSingle>().Owns)
        {
            Destroy(buttons);
        }
        for (int i = 0; i < Players[QqPlayer].GetComponent<PlayerSingle>().Owns.Count; i++)
        {
            Players[QqPlayer].GetComponent<PlayerSingle>().Owns[i].SetActive(true);
        }
        Destroy(loser);    
    }
    
}
