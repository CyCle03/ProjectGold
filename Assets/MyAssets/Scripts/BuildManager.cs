using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public Transform[] buildPos;
    public BuildingListObject buildList;
    public BuildingListObject shopList;
    public BuildingDBObject buildDB;
    public TownBuild[] townBuilds;
    public GameObject BuildInfoPrefab;
    public GameObject BuyBuildPrefab;

    // Start is called before the first frame update
    void Start()
    {
        buildList.database.UpdateID();

        for (int i = 0; i < shopList.GetListSlots.Length; i++)
        {
            if (buildList.GetListSlots[i].AllowedBuild == BuildType.Store && buildList.GetListSlots[i].build.Id <= -1)
            {
                for (int j = 0; j < buildDB.BuildObjects.Length; j++)
                {
                    if (buildDB.BuildObjects[j].type == BuildType.Store)
                    {
                        buildList.GetListSlots[i].UpdateListSlot(new Building(buildDB.BuildObjects[j]));
                        break;
                    }
                }
            }
            ShopListUpdate(i);
        }

        for (int i = 0; i < townBuilds.Length; i++)
        {
            ListSlot slot;

            slot = buildList.IsBuildOnList(townBuilds[i].buildObj);
            if (slot == null)
            {
                townBuilds[i].controllBuild.SetActive(false);
            }
        }
    }

    public void BuildTownUpdate()
    {
        for (int i = 0; i < townBuilds.Length; i++)
        {
            ListSlot slot;
            slot = buildList.IsBuildOnList(townBuilds[i].buildObj);
            if (slot == null)
            {
                townBuilds[i].controllBuild.SetActive(false);
            }
            else
            {
                townBuilds[i].controllBuild.SetActive(true);
            }
        } 
    }

    public void ShopListUpdate(int _index)
    {
        var buildSlot = buildList.GetListSlots[_index];
        var shopSlot = shopList.GetListSlots[_index];
        BuildType bType;
        if (buildSlot.build.Id <= -1)
        {
            bType = buildSlot.AllowedBuild;
            for (int i = 0; i < buildDB.BuildObjects.Length; i++)
            {
                if (buildDB.BuildObjects[i].type == bType)
                {
                    shopSlot.UpdateListSlot(new Building(buildDB.BuildObjects[i]));
                    return;
                }
            }
        }
        else
        {
            if (buildSlot.build.Id < buildDB.BuildObjects.Length - 1)
            {
                var DBnextBuild = buildDB.BuildObjects[buildSlot.build.Id + 1];
                if (DBnextBuild.type == buildSlot.build.type && DBnextBuild.data.BuildLevel > buildSlot.build.BuildLevel)
                {
                    shopSlot.UpdateListSlot(new Building(DBnextBuild));
                    return;
                }
            }
            shopSlot.UpdateListSlot(new Building());
        }
    }
}

[System.Serializable]
public class TownBuild
{
    [System.NonSerialized]
    public ListSlot townSlot;
    public BuildingObject buildObj;
    public GameObject controllBuild;
}