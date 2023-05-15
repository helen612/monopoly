using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;


public class DiceRoller : MonoBehaviour
{
    public Rigidbody rb;
    public Transform tr;
    public List<Collider> colliders;
    public List<GameObject> Scores;
    public GameObject Trigger;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        foreach (Transform child in tr)
        {
            Scores.Add(child.gameObject);
            colliders.Add(child.GetComponent<Collider>());
            
        }
    }

    private void Update()
    { /*
        if (rb.velocity.magnitude == 0 || rb.angularVelocity.magnitude == 0)
        {
            Debug.Log("Stop");
            //GetScoresDices();
        }
        */
    }

    public void RollDice(int Dice)
    {
        if (Dice == 1)
        {
            tr.position = new Vector3(-3.6f, 34.6f, 91f);
        }
        else
        {
            tr.position = new Vector3(12f, 34.6f, 106f);
        }
        rb.AddForce(UnityEngine.Random.insideUnitSphere * 10, ForceMode.Impulse);
        rb.AddTorque(UnityEngine.Random.insideUnitSphere * 10, ForceMode.Impulse);
        
    }

    private int GetScoresDices()
    {
        foreach (var childObject in Scores)
        {
            Collider collider = childObject.GetComponent<Collider>();

            if (collider != null)
            // &&
                //collider.bounds.Intersects(new Bounds(new Vector3(3.3669f, 24.22f, 99.25f), new Vector3(35.29381f, 35.74021f,  0.01f))))
               //collider.bounds.Intersects((new Bounds(Trigger.transform.position, Trigger)))
            {
                Debug.Log(childObject.name);
                return Convert.ToInt32(childObject.name);
            }
            
        }

        return -1;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }


}
