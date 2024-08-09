using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
 
public class Player_CollectItem : NetworkBehaviour
{
    public SO_Inventory inventory;
 
    public UnityEvent OnItemCollect;
 
    InventoryHUD hud;
 
    private void Start()
    {
        hud = FindObjectOfType<InventoryHUD>();
        OnItemCollect.AddListener(hud.UpdateInventory);
    }
    private void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                GameObject inventoryPanel =
                    hud.transform.GetChild(0).gameObject;
 
                inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            }
        }
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item collectedItem = collision.GetComponent<Item>();
 
        if (collectedItem)
        {
            if (isLocalPlayer)
            {
                inventory.AddItem(collectedItem.itemData, 1);
                OnItemCollect.Invoke();
            }
 
            Destroy(collision.gameObject);
        }
    }
}