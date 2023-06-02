using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Message : MonoBehaviour
{
    public static void show(string title, string message)
    {
        string mode = "";
        if (Player.localPlayer.InGame) mode = "G";
        GameObject Mistake = GameObject.FindGameObjectWithTag(mode + "Mistake");
        Animator animator = Mistake.GetComponent<Animator>();


        var TitleText = GameObject.Find(mode + "Mistake/Title");
        var MessageText = GameObject.Find(mode + "Mistake/Message");
        TitleText.GetComponent<TMP_Text>().text = title;
        MessageText.GetComponent<TMP_Text>().text = message;

        animator.SetTrigger("showMessage");

    }

    public static void showSingle(string title, string message)
    {
        
        GameObject Mistake = GameObject.FindGameObjectWithTag("GMistake");
        Animator animator = Mistake.GetComponent<Animator>();


        var TitleText = GameObject.Find("GMistake/Title");
        var MessageText = GameObject.Find("GMistake/Message");
        TitleText.GetComponent<TMP_Text>().text = title;
        MessageText.GetComponent<TMP_Text>().text = message;

        animator.SetTrigger("showMessage");

    }
    
    public static void showNotification(string message)
    {
        GameObject Notification = GameObject.FindGameObjectWithTag("PopUp");
        var NotificationMessage =  GameObject.Find("PopUp/message");
        NotificationMessage.GetComponent<TMP_Text>().text = message;
        Animator animator = Notification.GetComponent<Animator>();
        animator.SetTrigger("showNotification");

    }

}
