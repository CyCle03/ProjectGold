using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building Database", menuName = "Inventory System/Buildings/Database")]
public class BuildingDBObject : ScriptableObject, ISerializationCallbackReceiver
{
    public BuildingObject[] BuildObjects;

    [ContextMenu("Update ID's")]
    public void UpdateID()
    {
        for (int i = 0; i < BuildObjects.Length; i++)
        {
            if (BuildObjects[i].data.B_Id != i)
            {
                BuildObjects[i].data.B_Id = i;
            }

        }
    }

    public void OnAfterDeserialize()
    {
        UpdateID();
    }

    public void OnBeforeSerialize()
    {
    }
}
