using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject item;

    public void GItemUpdate()
    {
        if (item != null)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
#if UNITY_EDITOR
            EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
#endif
        }
    }

    public void OnAfterDeserialize()
    {
        
    }

    public void OnBeforeSerialize()
    {
        GItemUpdate();
    }
}
