using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Slider = UnityEngine.UI.Slider;
using TMPro;

public class PlayMode : MonoBehaviour
{
    public GameObject CountPlayers;
    public TMP_Text CountPlayersText;
    
    public void TruStartGame()
    {
        
        
        try
        {
            NetworkManager.singleton.StartHost();
        }
        catch
        {
            try
            {
                NetworkManager.singleton.StartClient();
            }
            catch (Exception e)
            {
                Message.show(e.Source,e.Message);
                throw;
            }

        }
        
    }

    public void StartSinglePlay()
    {
        //Message.show("Ошибка","Мы в разработке!");
        //SetStartParam.CountPlayers = Convert.ToInt32(CountPlayers.GetComponent<Slider>().value);
        PlayerPrefs.SetInt("CountPlayers", Convert.ToInt32(CountPlayers.GetComponent<Slider>().value));
        SceneManager.LoadScene("SinglePlayer");
        
    }

    public void ChangeValueSlider()
    {
        CountPlayersText.text = "Количество игроков: " + CountPlayers.GetComponent<Slider>().value.ToString();
    }

}
