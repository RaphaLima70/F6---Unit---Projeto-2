using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
 
 
public class InventoryHUD : MonoBehaviour
{
    public SO_Inventory playerInventory;
    public GameObject itemPanel;
    MouseItem mouse = new MouseItem();
    public Dictionary<GameObject, ItemSlot> GetSlotByIcon;
    public GameObject npcItemPanel;
    SO_Inventory npcInventory;
    public TextMeshProUGUI npcCoins;
    public TextMeshProUGUI playerCoins;
    public EquipEvent OnChangeEquipment;
    public Image equipSprite;
 
    private void Start()
    {
        DrawInventory();
    }
 
    void DrawInventory()
    {
        GetSlotByIcon = new Dictionary<GameObject, ItemSlot>();
 
        playerCoins.text = playerInventory.coins.ToString();
 
        foreach (ItemSlot slot in playerInventory.itemList)
        {
            GameObject new_icon = Instantiate(slot.item.icon, itemPanel.transform);
 
            new_icon.GetComponentInChildren<TextMeshProUGUI>().text =
                slot.amount.ToString();
 
            GetSlotByIcon.Add(new_icon, slot);
 
            AddTriggerEvent(new_icon, EventTriggerType.PointerEnter,
                delegate { OnEnter(new_icon); });
 
            AddTriggerEvent(new_icon, EventTriggerType.PointerExit,
                delegate { OnExit(new_icon); });
 
            AddTriggerEvent(new_icon, EventTriggerType.BeginDrag,
                delegate { OnStartDrag(new_icon); });
 
            AddTriggerEvent(new_icon, EventTriggerType.Drag,
                delegate { OnDrag(new_icon); });
 
            AddTriggerEvent(new_icon, EventTriggerType.EndDrag,
                delegate { OnStopDrag(new_icon); });
 
            new_icon.GetComponent<Button>().onClick.AddListener
            (delegate { OnClick(slot, npcInventory, playerInventory); });
        }
 
    }
    public void UpdateInventory()
    {
        foreach (Transform child in itemPanel.transform)
        {
            Destroy(child.gameObject);
        }
 
        DrawInventory();
    }
 
    public void DrawNPCInventory()
    {
        npcCoins.text = npcInventory.coins.ToString();
 
        foreach (Transform child in npcItemPanel.transform)
        {
            Destroy(child.gameObject);
        }
 
        foreach (ItemSlot slot in npcInventory.itemList)
        {
            GameObject new_icon = Instantiate(slot.item.icon, npcItemPanel.transform);
 
            new_icon.GetComponentInChildren<TextMeshProUGUI>().text =
                slot.amount.ToString();
 
            new_icon.GetComponent<Button>().onClick.AddListener
             (delegate { OnClick(slot, playerInventory, npcInventory); });
 
        }
    }
 
    void OnClick(ItemSlot slot, SO_Inventory buyInventory, SO_Inventory sellInventory)
    {
        if (buyInventory == null || sellInventory == null)
        {
            ChangeEquipment(slot);
            return;
        }
 
        if (buyInventory.coins >= slot.item.value)
        {
            buyInventory.AddItem(slot.item, 1);
            buyInventory.SpendCoins(slot.item.value);
 
            sellInventory.AddCoins(slot.item.value);
            sellInventory.RemoveItem(slot.item, 1);
 
            UpdateInventory();
            DrawNPCInventory();
        }
    }
 
    public void DefineNPCInventory(SO_Inventory inventory)
    {
        npcInventory = inventory;
        if (npcInventory)
        {
            DrawNPCInventory();
            UpdateInventory();
        }
    }
 
    public void RemoveNPCInventory()
    {
        npcInventory = null;
    }
 
    void ChangeEquipment(ItemSlot slot)
    {
        if (slot.item.type == ItemType.weapon)
        {
            equipSprite.gameObject.SetActive(true);
            equipSprite.sprite = slot.item.icon.GetComponent<Image>().sprite;
            //playerInventory.RemoveItem(slot.item, 1);
            //UpdateInventory();
            OnChangeEquipment.Invoke(slot.item as SO_Weapons);
        }
    }
 
    void AddTriggerEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
 
    public void OnEnter(GameObject obj)
    {
        if (GetSlotByIcon.ContainsKey(obj))
        {
            mouse.slotHovered = GetSlotByIcon[obj];
        }
    }
 
    public void OnExit(GameObject obj)
    {
        mouse.slotHovered = null;
    }
 
    public void OnStartDrag(GameObject obj) //quando começamos a arrastar o ícone
    {
        GameObject pointerIcon = new GameObject();
        RectTransform rect = pointerIcon.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(16, 16);
        pointerIcon.transform.SetParent(transform);
        Image img = pointerIcon.AddComponent<Image>();
        img.sprite = obj.GetComponent<Image>().sprite;
        img.raycastTarget = false;
 
        mouse.itemIcon = pointerIcon;
        mouse.slotClicked = GetSlotByIcon[obj];
    }
 
    public void OnDrag(GameObject obj) //quando estamos arrastando o ícone
    {
        if (mouse.itemIcon != null)
        {
            mouse.itemIcon.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
 
    public void OnStopDrag(GameObject obj) //quando paramos de arrastar o ícone
    {
        if (mouse.slotHovered != null)
        {
            playerInventory.SwapItems(mouse.slotClicked, mouse.slotHovered);
            UpdateInventory();
        }
        Destroy(mouse.itemIcon);
        mouse.itemIcon = null;
    }
}
 