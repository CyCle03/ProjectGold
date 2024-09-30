using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster Database", menuName = "Inventory System/Monsters/Database")]
public class MonsterDBObject : ScriptableObject, ISerializationCallbackReceiver
{
    public MonsterObject[] MonsterObjects;

    [ContextMenu("Update ID's")]
    public void UpdateID()
    {
        for (int i = 0; i < MonsterObjects.Length; i++)
        {
            if (MonsterObjects[i].data.mId != i)
            {
                MonsterObjects[i].data.mId = i;
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
