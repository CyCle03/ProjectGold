using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildInfoPrefab : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public void DestroyTempInfo()
    {
        Destroy(BuildMouseData.slotBuildInfo);
        BuildMouseData.slotBuildInfo = null;
        BuildMouseData.sellBuild = null;
    }

    public void DestroyBuyInfo()
    {
        Destroy(BuildMouseData.buyBuildinfo);
        BuildMouseData.buyBuildinfo = null;
        BuildMouseData.buyBuild = null;
    }

    public void BuyBuild()
    {
        if (BuildMouseData.buyBuildinfo != null)
        {
            ListSlot bmdBuyBuild = BuildMouseData.buyBuild;
            if (player.inventory.gold >= bmdBuyBuild.build.BuildValue)
            {
                for (int i = 0; i < player.buildList.GetListSlots.Length; i++)
                {
                    if (player.buildList.GetListSlots[i] == bmdBuyBuild)
                    {
                        player.BuildBuy(bmdBuyBuild.build.BuildValue, bmdBuyBuild, i);
                    }
                }
            }
        }
    }

/*    public void SellBuild()
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
                    { EquipItems(MouseData.useItem, player.inventory.FindEmptySlot()); }
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
    }*/

    /*public void EquipItems(InventorySlot item1, InventorySlot item2)
    {
        if (item1 == item2)
        {
            item2 = player.inventory.FindEmptySlot();
        }
        if (item2 != null)
        {
            player.equipment.SwapItems(item1, item2);
        }
    }*/
}
