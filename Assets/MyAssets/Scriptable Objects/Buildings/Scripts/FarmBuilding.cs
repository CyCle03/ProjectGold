using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Farm Building", menuName = "Inventory System/Buildings/Farm")]

public class FarmBuilding : BuildingObject
{
    public void Awake()
    {
        type = BuildType.Farm;
    }
}
