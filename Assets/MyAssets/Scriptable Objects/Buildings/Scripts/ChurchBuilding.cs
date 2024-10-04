using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Church Building", menuName = "Inventory System/Buildings/Church")]

public class ChurchBuilding : BuildingObject
{
    public void Awake()
    {
        type = BuildType.Church;
    }
}
