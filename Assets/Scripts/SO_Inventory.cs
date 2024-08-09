using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Inventory")]
public class SO_Inventory : ScriptableObject
{
    public float coins;

    public void SpendCoins(float value)
    {
        coins -= value;
        coins = Mathf.Clamp(coins, 0, 999);
    }

    public void AddCoins(float value)
    {
        coins += value;
        coins = Mathf.Clamp(coins, 0, 999);
    }

    public List<ItemSlot> itemList = new List<ItemSlot>();
    public void AddItem(SO_ItemBase item, int amount)
    {
        bool itemExists = false;

        foreach (ItemSlot slot in itemList)
        {
            if (slot.item == item)
            {
                slot.AddAmount(amount);
                itemExists = true;
                break;
            }
        }

        if (itemExists == false)
        {
            ItemSlot slot = new ItemSlot(item, amount);
            itemList.Add(slot);
        }
    }
    public void SwapItems(ItemSlot itemClicked, ItemSlot itemHovered)
    {
        if (itemList.Contains(itemClicked) && itemList.Contains(itemHovered))
        {
            int i1 = itemList.IndexOf(itemClicked);
            int i2 = itemList.IndexOf(itemHovered);

            itemList[i1] = itemHovered;
            itemList[i2] = itemClicked;
        }
    }

    public void RemoveItem(SO_ItemBase item, int amount)
    {
        foreach (ItemSlot slot in itemList)
        {
            if (slot.item == item)
            {
                if (slot.amount > 1)
                {
                    slot.RemoveAmount(amount);
                }
                else
                {
                    itemList.Remove(slot);
                }

                break;
            }
        }
    }
}

[Serializable]
public class ItemSlot
{
    public SO_ItemBase item;
    public int amount;

    //construtor que usaremos para criar novos slots já com o item e quantidade de item dele
    public ItemSlot(SO_ItemBase new_item, int new_amount)
    {
        item = new_item;
        amount = new_amount;
    }

    //função reponsável por adicionar uma quantidade de itens no slot
    public void AddAmount(int value)
    {
        amount += value;
    }
    //função reponsável por subtrai uma quantidade de itens no slot
    public void RemoveAmount(int value)
    {
        amount -= value;
    }
}
