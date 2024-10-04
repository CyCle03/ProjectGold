using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Windmill Building", menuName = "Inventory System/Buildings/Windmill")]

public class WindmillBuilding : BuildingObject
{
    public void Awake()
    {
        type = BuildType.Windmill;
    }
}
