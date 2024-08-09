using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class NPCVendor : NetworkBehaviour
{
   public NPCVendorEvent OnPlayerClose;
   public UnityEvent OnPlayerAway;
   public SO_Inventory npcInventory;

   private void OnTriggerEnter2D(Collider2D collision)
   {
       Player player = collision.GetComponent<Player>();
       if(player != null && player.isLocalPlayer)
       {
           OnPlayerClose.Invoke(npcInventory);
       }
   }

   private void OnTriggerExit2D(Collider2D collision)
   {
       Player player = collision.GetComponent<Player>();
       if (player != null && player.isLocalPlayer)
       {
           OnPlayerAway.Invoke();
       }
   }
}