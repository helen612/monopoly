using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TurnManager : NetworkBehaviour
{
    
    public readonly SyncList<Player> players = new SyncList<Player>();

   // public void AddPlayer(Player player)
   // {
   //     players.Add(player);
   // }
}
