using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wall Building", menuName = "Inventory System/Buildings/Wall")]
public class WallBuilding : BuildingObject
{
    private void Awake()
    {
        type = BuildType.Wall;
        data.type = type;
    }
}
