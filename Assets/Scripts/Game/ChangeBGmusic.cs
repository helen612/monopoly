using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeBGmusic : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] public AudioClip audioSource;
    
    [Header("Tags")] 
    [SerializeField] private string MusicTag;
    private void Start()
    {
        GameObject audS = GameObject.FindWithTag(this.MusicTag);
        if (audS != null)
        {
            this.audioSource = audS.GetComponent<AudioSource>().clip;
            this.audioSource = Resources.Load<AudioClip>("Assets/Music and sound/InGame.mpp3");
        }
    }
}
