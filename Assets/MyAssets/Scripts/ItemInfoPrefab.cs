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
            if (player.inventory.gold >= mdBuyItem.ItemValue && player.inventory.EmptySlotCount >= 1)
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
                    player.equipment.SwapItems(MouseData.useItem, player.equipment.GetSlots[0]);
                    break;
                case ItemType.Weapon:
                    if(player.equipment.GetSlots[1].item.Id <= -1)
                    { player.equipment.SwapItems(MouseData.useItem, player.equipment.GetSlots[1]); }
                    else if (player.equipment.GetSlots[2].item.Id <= -1)
                    { player.equipment.SwapItems(MouseData.useItem, player.equipment.GetSlots[2]); }
                    else { player.equipment.SwapItems(MouseData.useItem, player.equipment.GetSlots[1]); }
                    break;
                case ItemType.Shield:
                    player.equipment.SwapItems(MouseData.useItem, player.equipment.GetSlots[2]);
                    break;
                case ItemType.Boots:
                    player.equipment.SwapItems(MouseData.useItem, player.equipment.GetSlots[4]);
                    break;
                case ItemType.Chest:
                    player.equipment.SwapItems(MouseData.useItem, player.equipment.GetSlots[3]);
                    break;
                case ItemType.Default:
                    break;
                default:
                    break;
            }
        }
    }
}
