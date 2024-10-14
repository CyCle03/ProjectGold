using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stable Building", menuName = "Inventory System/Buildings/AnimalFarm")]

public class AnimalFarmBuilding : BuildingObject
{
    public void Awake()
    {
        type = BuildType.AnimalFarm;
        data.type = type;
    }
}
