using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cards
{
    public string text;
    public int money;
    public bool jail;
    public int move;
    public string mode;
    

    public cards(string text, int money)
    {
        this.text = text;
        this.money = money;
        this.mode = "money";
    }
    public cards(string text, bool jail)
    {
        this.text = text;
        this.jail = jail;
        this.mode = "jail";
    }
    public cards(int RPos, string text)
    {
        this.text = text;
        this.move = RPos;
        this.mode = "move";
    }
    public cards()
    {
        
    }
}

public class FieldInfoU
{
    #region Vars

    public string tag;
    public string fullName;
    public Color Color;
    public Color owner;
    public int level;
    public bool forSale;
    public int cost;

    public int rent;
    public int rentGroup;
    public int home1;
    public int home2;
    public int home3;
    public int home4;
    public int hotel;

    public int costBuild;


    #endregion
    
    public FieldInfoU()
    {
        this.fullName = "";
        this.owner = Color.white;
        this.forSale = false;
        this.cost = 0;
        this.rent = 0;
        this.rentGroup = 0;
        this.home1 = 0;
        this.home2 = 0;
        this.home3 = 0;
        this.home4 = 0;
        this.hotel = 0;
        this.costBuild = 0;
    }
    public FieldInfoU(string tag,string fullName, Color color, Color owner, bool forSale, int cost, int rent, int rentGroup,
        int home1, int home2, int home3, int home4, int hotel, int costBuild)
    {
        this.level = 0;
        this.fullName = fullName;
        this.Color = color;
        this.Color.a = 1;
        this.owner = owner;
        this.forSale = forSale;
        this.cost = cost;
        this.rent = rent;
        this.rentGroup = rentGroup;
        this.home1 = home1;
        this.home2 = home2;
        this.home3 = home3;
        this.home4 = home4;
        this.hotel = hotel;
        this.costBuild = costBuild;
    }

}

public class Field
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
 
    public Field()
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

    public Field(string tag,string name, FieldInfoU field)
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
        Player.localPlayer.updateCash(200);
        UIController.instance.updateCash();
    }
    private void GoJail()
    {
        if (Player.localPlayer.FreeJail == 0)
        {
            Debug.Log("Отправляйтесь в тюрьму");
            GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().moves[Player.localPlayer.routePosition]
                .countPlayer--;
            Player.localPlayer.transform.position = GameObject.Find("Jail").transform.position;
            Player.localPlayer.routePosition = 10;
            GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().moves[Player.localPlayer.routePosition]
                .countPlayer++;
            Player.localPlayer.forSkip = 4;  
        }
        else
        {
            Message.show("Шанс", "Вы не попадаете в тюрьму и ваша карточка сгорает");
            Player.localPlayer.FreeJail--;
        }
        
    }
    private void stayTax()
    {
        Debug.Log("Pay " + field.rent);
        Player.localPlayer.updateCash(-1 * field.rent);
        UIController.instance.updateCash();
    }
    
    private void StayPT()
    {
        Debug.Log("Public treausuru");
        Debug.Log("Chance");
        var r = GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().CardsList.Dequeue();
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().CardsList.Enqueue(r);
        Message.show("Общественная казна",r.text);

        switch (r.mode)
        {
            case "move":
            {
                int rSteps = 0;
                if (Player.localPlayer.routePosition > r.move)
                {
                    rSteps = 40 - Player.localPlayer.routePosition;
                    rSteps += r.move;
                }
                else
                {
                    rSteps = r.move - Player.localPlayer.routePosition;
                }
                Player.localPlayer.startMove(rSteps);
                break;
            }
            case "money":
            {
                Player.localPlayer.updateCash(r.money);
                UIController.instance.updateCash();
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
                    Player.localPlayer.FreeJail++;
                }
                break;
            }
            default:
            {
                Debug.Log("карточку шанс невозможно вытянуть");
                break;
            }
        }
    }

    private void stayChance()
    {
        Debug.Log("Chance");
        var r = GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().CardsList.Dequeue();
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().CardsList.Enqueue(r);
        Message.show("Шанс",r.text);

        switch (r.mode)
        {
            case "move":
            {
                int rSteps = 0;
                if (Player.localPlayer.routePosition > r.move)
                {
                    rSteps = 40 - Player.localPlayer.routePosition;
                    rSteps += r.move;
                }
                else
                {
                    rSteps = r.move - Player.localPlayer.routePosition;
                }
                Player.localPlayer.startMove(rSteps);
                break;
            }
            case "money":
            {
                Player.localPlayer.updateCash(r.money);
                UIController.instance.updateCash();
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
                    Player.localPlayer.FreeJail++;
                }
                break;
            }
            default:
            {
                Debug.Log("карточку шанс невозможно вытянуть");
                break;
            }
        }
    }
     
    
    private void stayStreet()
    {
        Debug.Log("Улица" + field.fullName);
        if(Player.localPlayer.playerColor == field.owner) return;
        switch (field.level)
        {
            case 0:
            {
                Debug.Log("Платить ренту не надо!");
                break;
            }
            case 1:
            {
                payToOwner(field.rent);
                break;
            }
            case 2:
            {
                payToOwner(field.rentGroup);
                break;
            }
            case 3:
            {
                payToOwner(field.home1);
                break;
            }
            case 4:
            {
                payToOwner(field.home2);
                break;
            }
            case 5:
            {
                payToOwner(field.home3);
                break;
            }
            case 6:
            {
                payToOwner(field.home4);
                break;
            }
            case 7:
            {
                payToOwner(field.hotel);
                break;
            }
        }

    }
    

    private void stayTR()
    {
        Debug.Log("Train Road " + field.fullName);
        if(Player.localPlayer.playerColor == field.owner) return;
        switch (field.level)
        {
            case 0:
            {
                Debug.Log("Платить ренту не надо!");
                break;
            }
            case 3:
            {
                payToOwner(field.home1);
                break;
            }
            case 4:
            {
                payToOwner(field.home2);
                break;
            }
            case 5:
            {
                payToOwner(field.home3);
                break;
            }
            case 6:
            {
                payToOwner(field.home4);
                break;
            }
        }
        
    }
    
    private void stayCom()
    {
        Debug.Log("Com " + field.fullName);
        if(Player.localPlayer.playerColor == field.owner || field.owner == Color.white) return;
        
        GameObject.Find("Dice1").GetComponent<DiceRoller>().RollDice(1);
        GameObject.Find("Dice2").GetComponent<DiceRoller>().RollDice(2);
        GameObject.FindWithTag("PlayerManager").GetComponent<PlayerManager>().forCom = true;
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
        Player.localPlayer.updateCash(-1 * money);
        Player.localPlayer.payTo(field.owner, money);
    }
    
}
