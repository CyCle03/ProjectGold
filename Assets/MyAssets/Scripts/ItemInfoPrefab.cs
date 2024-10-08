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
    public void DestroyTempInfo()
    {
        Destroy(MouseData.slotItemInfo);
        MouseData.slotItemInfo = null;
    }

    public void BuyItem()
    {
        if (MouseData.buyIteminfo != null)
        {
            if (player.inventory.gold >= MouseData.buyItem.ItemValue && player.inventory.EmptySlotCount >= 1)
            {
                player.ShopBuy(MouseData.buyItem.ItemValue, MouseData.buyItem, 1);
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
                    int _foodHP = useItemObj.foodHP;
                    player.inventory.RemoveItem(useItemObj.data, 1);
                    if (player.maxHP - player.curruntHP >= _foodHP)
                    { player.curruntHP += _foodHP; }
                    else { player.curruntHP = player.maxHP; }
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
