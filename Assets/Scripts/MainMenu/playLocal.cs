using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class playLocal : MonoBehaviour
{
    //public NetworkManager NM;
    public void ImHostClickgf()
    {
        string host = System.Net.Dns.GetHostName();
        // Получение ip-адреса.
        var ip = System.Net.Dns.GetHostByName(host).AddressList;
        // Показ адреса в label'е.

        foreach (var adr in ip)
        {
            if (adr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                
                //NM.networkAddress = adr.ToString();
                break;
            }
        }
        //NM.maxConnections = 3;
    }
}
