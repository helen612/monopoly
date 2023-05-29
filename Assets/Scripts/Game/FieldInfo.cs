using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldInfo : MonoBehaviour
{
    public string fullName;
    public Color color;
    public int owner;
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


    public FieldInfoU ToFieldInfoU()
    {
        
        return new FieldInfoU(this.tag,fullName, color, Color.white, forSale, cost, rent, rentGroup,
            home1, home2, home3, home4, hotel, costBuild);
    }
    


}
