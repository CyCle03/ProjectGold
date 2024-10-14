using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tarvern Building", menuName = "Inventory System/Buildings/Tarvern")]

public class TarvernBuilding : BuildingObject
{
    public void Awake()
    {
        type = BuildType.Tarvern;
        data.type = type;
    }
}
