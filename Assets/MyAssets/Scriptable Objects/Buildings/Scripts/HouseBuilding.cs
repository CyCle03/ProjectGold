using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New House Building", menuName = "Inventory System/Buildings/House")]

public class HouseBuilding : BuildingObject
{
    public void Awake()
    {
        type = BuildType.House;
        data.type = type;
    }
}
