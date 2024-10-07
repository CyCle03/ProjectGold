using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfoPrefab : MonoBehaviour
{
    public Player player;

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
}
