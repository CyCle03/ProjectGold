using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public InventoryObject shop;
    public ItemDatabaseObject itemDB;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < shop.GetSlots.Length && i < itemDB.ItemObjects.Length ; i++)
        {
            if (shop.AddItem(new Item(itemDB.ItemObjects[i]), 1))
            {
            }
        }
    }
}
