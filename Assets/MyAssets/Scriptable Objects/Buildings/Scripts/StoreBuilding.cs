using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Store Building", menuName = "Inventory System/Buildings/Store")]

public class StoreBuilding : BuildingObject
{
    public void Awake()
    {
        type = BuildType.Store;
    }
}
