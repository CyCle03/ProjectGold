using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Smith Building", menuName = "Inventory System/Buildings/Smith")]

public class SmithBuilding : BuildingObject
{
    public void Awake()
    {
        type = BuildType.Smith;
        data.type = type;
    }
}
