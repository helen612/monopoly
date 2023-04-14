using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

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
                Console.WriteLine(e);
                throw;
            }

        }
    }

}
