using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UIElements;

public class PlayMode : MonoBehaviour
{
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
        Message.show("Ошибка","Мы в разработке!");
        
    }

}
