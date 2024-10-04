using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class BuildInterface : MonoBehaviour
{

    public BuildingListObject buildList;
    public Dictionary<GameObject, ListSlot> listSlotsOnInterface = new Dictionary<GameObject, ListSlot>();

    // Start is called before the first frame update
    void Start()
    {
        listSlotsOnInterface.UpdateListSlotDisplay();
        for (int i = 0; i < buildList.GetListSlots.Length; i++)
        {
            buildList.GetListSlots[i].parent = this;
            buildList.GetListSlots[i].OnAfterUpdate += OnListSlotUpdate;
        }
        CreateListSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnListEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnListExitInterface(gameObject); });
    }

    private void OnListSlotUpdate(ListSlot _slot)
    {
        if (_slot.build.B_Id >= 0)
        {
            _slot.listSlotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.BuildObject.uiDisplay;
            _slot.listSlotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            _slot.listSlotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
        else
        {
            _slot.listSlotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            _slot.listSlotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            _slot.listSlotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }
    public abstract void CreateListSlots();

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnListEnter(GameObject obj)
    {
        BuildMouseData.listSlotHoveredOver = obj;
    }

    public void OnListExit(GameObject obj)
    {
        BuildMouseData.listSlotHoveredOver = null;
    }

    public void OnListEnterInterface(GameObject obj)
    {
        BuildMouseData.interfaceMouseIsOver = obj.GetComponent<BuildInterface>();
    }

    public void OnListExitInterface(GameObject obj)
    {
        BuildMouseData.interfaceMouseIsOver = null;
    }

    public void OnListDragStart(GameObject obj)
    {
        BuildMouseData.tempBuildBeingDragged = CreatTempBuild(obj);
    }

    public GameObject CreatTempBuild(GameObject obj)
    {
        GameObject tempbuild = null;
        if (listSlotsOnInterface[obj].build.B_Id >= 0)
        {
            tempbuild = new GameObject();
            var rt = tempbuild.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(100, 100);
            tempbuild.transform.SetParent(transform.parent);
            var img = tempbuild.AddComponent<Image>();
            img.sprite = listSlotsOnInterface[obj].BuildObject.uiDisplay;
            img.raycastTarget = false;
        }
        return tempbuild;
    }

    public void OnListDragEnd(GameObject obj)
    {
        Destroy(BuildMouseData.tempBuildBeingDragged);

        if (BuildMouseData.interfaceMouseIsOver == null)
        {
            listSlotsOnInterface[obj].RemoveBuild();
            return;
        }
        if (BuildMouseData.listSlotHoveredOver)
        {
            ListSlot mouseHoverSlotData = BuildMouseData.interfaceMouseIsOver.listSlotsOnInterface[BuildMouseData.listSlotHoveredOver];
            buildList.SwapBuilds(listSlotsOnInterface[obj], mouseHoverSlotData);
        }
    }

    public void OnListDrag(GameObject obj)
    {
        if (BuildMouseData.tempBuildBeingDragged != null)
        {
            BuildMouseData.tempBuildBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
}

public static class BuildMouseData
{
    public static BuildInterface interfaceMouseIsOver;
    public static GameObject tempBuildBeingDragged;
    public static GameObject listSlotHoveredOver;
}

public static class ExtensionBuildMethods
{
    public static void UpdateListSlotDisplay(this Dictionary<GameObject, ListSlot> _listSlotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, ListSlot> _slot in _listSlotsOnInterface)
        {
            if (_slot.Value.build.B_Id >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.Value.BuildObject.uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
}