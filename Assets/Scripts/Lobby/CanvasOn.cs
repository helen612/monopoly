using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasOn : MonoBehaviour
{
    public GameObject Canvas;

    private void Start()
    {
        Debug.Log("¬ключаю Canvas");
        Canvas.SetActive(true);
       
    }
}
