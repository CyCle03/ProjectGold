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
            if (player.inven.GetGold >= bmdBuyBuild.build.BuildValue)
            {
                player.BuildBuy(bmdBuyBuild.build.BuildValue, bmdBuyBuild); 
            }
        }
    }

    public void SellBuild()
    {
        if (BuildMouseData.slotBuildInfo != null)
        {
            player.BuildSell(BuildMouseData.sellBuild);
        }
    }
}
