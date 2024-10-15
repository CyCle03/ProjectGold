using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BShopInterface : BuildInterface
{
    public GameObject buildListPrefab;

    public int X_START;
    public int Y_START;
    public int X_SPACE_BETEEN_BUILD;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETEEN_BUILD;

    public override void CreateListSlots()
    {
        listSlotsOnInterface = new Dictionary<GameObject, ListSlot>();
        for (int i = 0; i < buildList.GetListSlots.Length; i++)
        {
            var obj = Instantiate(buildListPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnListEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnListExit(obj); });
            AddEvent(obj, EventTriggerType.PointerClick, delegate { OnBuyBuild(obj); });

            buildList.GetListSlots[i].listSlotDisplay = obj;
            buildList.GetListSlots[i].indexNum = i;
            listSlotsOnInterface.Add(obj, buildList.GetListSlots[i]);
        }
    }

    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETEEN_BUILD * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETEEN_BUILD * (i / NUMBER_OF_COLUMN)), 0f);
    }
}
