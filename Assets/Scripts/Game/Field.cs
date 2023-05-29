using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cards
{
    public string text;
    public int money;
    public bool jail;
    
}

public class FieldInfoU
{
    public string tag;
    public string fullName;
    public Color Color;
    public Color owner;
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
        Player.localPlayer.cash += 200;
        UIController.instance.updateCash();
    }
    private void GoJail()
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
    private void stayTax()
    {
        Debug.Log("Pay " + field.rent);
        Player.localPlayer.cash -= field.rent;
        UIController.instance.updateCash();
    }
    
    private void stayStreet()
    {
        Debug.Log("Улица" + field.fullName);
       

    }

    private void stayTR()
    {
        Debug.Log("Train Road " + field.fullName);
        
    }
    
    private void stayCom()
    {
        Debug.Log("Com " + field.fullName);
    }

    private void StayPT()
    {
        Debug.Log("Public treausuru");
    }

    private void stayChance()
    {
        Debug.Log("Chance");
    }

    
}
