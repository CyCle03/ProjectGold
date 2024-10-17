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

    BuildManager bm;
    GameManager gm;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < buildList.GetListSlots.Length; i++)
        {
            buildList.GetListSlots[i].parent = this;
            buildList.GetListSlots[i].OnAfterUpdate += OnListSlotUpdate;
        }
        CreateListSlots();
        listSlotsOnInterface.UpdateListSlotDisplay();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnListEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnListExitInterface(gameObject); });
    }

    private void Start()
    {
        bm = GameObject.Find("BuildManager").GetComponent<BuildManager>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnListSlotUpdate(ListSlot _slot)
    {
        if (_slot.build.Id >= 0)
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

    public void OnClickBuild(GameObject obj)
    {
        if (BuildMouseData.buyBuild != null)
        {
            DestroyBuyInfo();
        }
        if (BuildMouseData.sellBuild != null)
        {
            if (BuildMouseData.sellBuild == listSlotsOnInterface[obj])
            {
                DestroyTempInfo();
                return;
            }
            DestroyTempInfo();
        }
        BuildMouseData.slotBuildInfo = CreatTempInfo(obj, bm.BuildInfoPrefab);
    }

    public void OnBuyBuild(GameObject obj)
    {
        if (BuildMouseData.sellBuild != null)
        {
            DestroyTempInfo();
        }
        if (BuildMouseData.buyBuild != null)
        {
            if (BuildMouseData.buyBuild == listSlotsOnInterface[obj])
            {
                DestroyBuyInfo();
                return;
            }
            DestroyBuyInfo();
        }
        BuildMouseData.buyBuildinfo = CreatTempInfo(obj, bm.BuyBuildPrefab);
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

    public GameObject CreatTempInfo(GameObject obj, GameObject prefab)
    {
        GameObject tempInfo = null;

        TextMeshProUGUI name;
        TextMeshProUGUI lv;
        TextMeshProUGUI value;
        TextMeshProUGUI buffsDescript;

        if (listSlotsOnInterface[obj].build.Id >= 0)
        {
            tempInfo = Instantiate(prefab, obj.transform.position, Quaternion.identity, gm.InventoryCanvas.transform);

            RectTransform rt = tempInfo.GetComponent<RectTransform>();
            if (rt.anchoredPosition.x > 640)
            {
                rt.anchoredPosition = new Vector2(640, rt.anchoredPosition.y);
            }
            if (rt.anchoredPosition.y < -220)
            {
                rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -220);
            }

            if (prefab == bm.BuyBuildPrefab)
            {
                BuildMouseData.buyBuild = listSlotsOnInterface[obj];
            }
            else if (prefab == bm.BuildInfoPrefab)
            {
                BuildMouseData.sellBuild = listSlotsOnInterface[obj];
            }

            var img = tempInfo.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            name = tempInfo.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            lv = tempInfo.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
            value = tempInfo.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
            buffsDescript = tempInfo.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>();

            name.text = listSlotsOnInterface[obj].build.BuildName;
            lv.text = listSlotsOnInterface[obj].build.BuildLevel.ToString();
            value.text = listSlotsOnInterface[obj].build.BuildValue.ToString("n0") + " G";
            buffsDescript.text = listSlotsOnInterface[obj].build.buffList + listSlotsOnInterface[obj].BuildObject.description;

            img.sprite = listSlotsOnInterface[obj].BuildObject.uiDisplay;
        }
        return tempInfo;
    }

    public void DestroyTempInfo()
    {
        Destroy(BuildMouseData.slotBuildInfo);
        BuildMouseData.slotBuildInfo = null;
        BuildMouseData.sellBuild = null;
    }

    public void DestroyBuyInfo()
    {
        Destroy(BuildMouseData.buyBuildinfo);
        BuildMouseData.buyBuildinfo = null;
        BuildMouseData.buyBuild = null;
    }
}

public static class BuildMouseData
{
    public static BuildInterface interfaceMouseIsOver;
    public static GameObject tempBuildBeingDragged;
    public static GameObject listSlotHoveredOver;
    public static GameObject slotBuildInfo;
    public static GameObject buyBuildinfo;
    public static ListSlot buyBuild;
    public static ListSlot sellBuild;
}

public static class ExtensionBuildMethods
{
    public static void UpdateListSlotDisplay(this Dictionary<GameObject, ListSlot> _listSlotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, ListSlot> _slot in _listSlotsOnInterface)
        {
            if (_slot.Value.build.Id >= 0)
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