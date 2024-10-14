using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public Transform[] buildPos;
    public BuildingListObject buildList;
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
    }

    // Update is called once per frame
    void Update()
    {
        
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