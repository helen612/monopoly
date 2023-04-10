using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class WaitMenu : NetworkBehaviour
{
    public TMP_Text IpText;
    public TMP_Text addressIP;
    public TMP_Text ConnectedPlayers;

    [SyncVar]
    private int countClient;
    // Start is called before the first frame update

   
    void Start()
    {
        addressIP.SetText(NetworkManager.singleton.networkAddress);
        if (isClient)
            IpText.SetText("IP Сервера");
      
    }


    // Update is called once per frame
    void Update()
    {
        if (isServer)
            countClient = NetworkServer.connections.Count;
        ConnectedPlayers.SetText(countClient.ToString() + "/" + NetworkManager.singleton.maxConnections);
    }
}
