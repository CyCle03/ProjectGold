using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemInfoPrefab : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public void DestroyBuyInfo()
    {
        Destroy(MouseData.buyIteminfo);
        MouseData.buyIteminfo = null;
        MouseData.buyItem = null;
    }
    public void DestroyTempInfo()
    {
        Destroy(MouseData.slotItemInfo);
        MouseData.slotItemInfo = null;
        MouseData.useItem = null;
    }

    public void BuyItem()
    {
        if (MouseData.buyIteminfo != null)
        {
            Item mdBuyItem = MouseData.buyItem.item;
            if (player.inven.GetGold >= mdBuyItem.ItemValue && player.inven.EmptySlotCount >= 1)
            {
                player.ShopBuy(mdBuyItem.ItemValue, mdBuyItem, 1);
            }
        }
    }

    public void UseItem()
    {
        if (MouseData.slotItemInfo != null)
        {
            ItemObject useItemObj = MouseData.useItem.ItemObject;
            switch (useItemObj.type)
            {
                case ItemType.Food:
                    player.EatFood(useItemObj, 1);
                    break;
                case ItemType.Helmet:
                    EquipItems(MouseData.useItem, player.equipment.GetSlots[0]);
                    break;
                case ItemType.Weapon:
                    if (MouseData.useItem == player.equipment.GetSlots[1] || MouseData.useItem == player.equipment.GetSlots[2])
                    { EquipItems(MouseData.useItem, player.inven.FindEmptySlot()); }
                    else if (player.equipment.GetSlots[1].item.Id <= -1)
                    { player.equipment.SwapItems(MouseData.useItem, player.equipment.GetSlots[1]); }
                    else if (player.equipment.GetSlots[2].item.Id <= -1)
                    { player.equipment.SwapItems(MouseData.useItem, player.equipment.GetSlots[2]); }
                    else 
                    { player.equipment.SwapItems(MouseData.useItem, player.equipment.GetSlots[1]); }
                    break;
                case ItemType.Shield:
                    EquipItems(MouseData.useItem, player.equipment.GetSlots[2]);
                    break;
                case ItemType.Boots:
                    EquipItems(MouseData.useItem, player.equipment.GetSlots[4]);
                    break;
                case ItemType.Chest:
                    EquipItems(MouseData.useItem, player.equipment.GetSlots[3]);
                    break;
                case ItemType.Default:
                    break;
                default:
                    break;
            }
        }
    }

    public void EquipItems(InventorySlot item1, InventorySlot item2)
    {
        if (item1 == item2)
        {
            item2 = player.inven.FindEmptySlot();
        }
        if (item2 != null)
        {
            player.equipment.SwapItems(item1, item2);
        }
    }
}
