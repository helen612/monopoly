using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeBGmusic : MonoBehaviour
{
    [Header("Tags")] 
    [SerializeField] private string MusicTag;
    private void Start()
    {
        GameObject audS = GameObject.Find("Music");
        if (audS != null)
        {
            Destroy(audS);
        }
    }
}
