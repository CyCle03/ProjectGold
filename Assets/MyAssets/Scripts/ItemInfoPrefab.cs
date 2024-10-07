using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfoPrefab : MonoBehaviour
{
    public void DestroyTempInfo()
    {
        Destroy(MouseData.slotItemInfo);
        MouseData.slotItemInfo = null;
    }
}
