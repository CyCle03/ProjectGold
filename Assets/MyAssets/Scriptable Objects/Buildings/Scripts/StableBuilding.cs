using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stable Building", menuName = "Inventory System/Buildings/Stable")]

public class StableBuilding : BuildingObject
{
    public void Awake()
    {
        type = BuildType.Stable;
    }
}
