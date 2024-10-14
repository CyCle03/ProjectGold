using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "New Castle Building", menuName = "Inventory System/Buildings/Castle")]

public class CastleBuilding : BuildingObject
{
    public void Awake()
    {
        type = BuildType.Castle;
        data.type = type;
    }
}
