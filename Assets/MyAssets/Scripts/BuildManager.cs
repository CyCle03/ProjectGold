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

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < townBuilds.Length; i++)
        {
            ListSlot slot;
            slot = buildList.IsBuildOnList(townBuilds[i].buildObj);
            if (slot == null)
            {
                townBuilds[i].controllBuild.SetActive(false);
            }
        }

        for (int i = 0; i < shopList.GetListSlots.Length; i++)
        {
            ShopListUpdate(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShopListUpdate(int _index)
    {
        var buildSlot = buildList.GetListSlots[_index];
        var shopSlot = shopList.GetListSlots[_index];
        BuildType bType;

        if (buildSlot.build.B_Id <= -1)
        {
            for (int i = 0; i < buildSlot.AllowedBuilds.Length; i++)
            {
                bType = buildSlot.AllowedBuilds[i];
                for (int j = 0; j < buildDB.BuildObjects.Length; j++)
                {
                    if (buildDB.BuildObjects[j].type == bType)
                    {
                        shopSlot.UpdateListSlot(buildDB.BuildObjects[j].data);
                    }
                }
            }
        }
        else
        {
            var DBnextBuild = buildDB.BuildObjects[buildSlot.build.B_Id + 1];
            if (DBnextBuild.type == buildSlot.build.type && DBnextBuild.data.BuildLevel > buildSlot.build.BuildLevel)
            {
                shopSlot.UpdateListSlot(DBnextBuild.data);
            }
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