using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using Unity.VisualScripting;

public abstract class UserInterface : MonoBehaviour
{
    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

    GameManager gm;
    
    void Awake()
    {
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].parent = this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }
        CreateSlots();
        slotsOnInterface.UpdateSlotDisplay();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    // Start is called before the first frame update
    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnSlotUpdate(InventorySlot _slot)
    {
        if (_slot.item.Id >= 0)
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.ItemObject.uiDisplay;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = _slot.amount == 1 ? "" : _slot.amount.ToString("n0");
        }
        else
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }

    // Update is called once per frame
/*    void Update()
    {
        slotsOnInterface.UpdateSlotDisplay();
    }*/


    public abstract void CreateSlots();

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }

    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }

    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }

    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }

    public void OnDragStart(GameObject obj)
    {
        MouseData.tempItemBeingDragged = CreatTempItem(obj);
    }

    public void OnClickItem(GameObject obj)
    {
        if (MouseData.slotItemInfo != null)
        {
            DestroyTempInfo();
        }
        else
        {
            MouseData.slotItemInfo = CreatTempInfo(obj, gm.ItemInfoPrefab);
        }
    }

    public void OnBuyItem(GameObject obj)
    {
        if (MouseData.buyIteminfo != null)
        {
            Destroy(MouseData.buyIteminfo);
            MouseData.buyIteminfo = null;
            MouseData.buyItem = null;
        }
        else
        {
            MouseData.buyIteminfo = CreatTempInfo(obj, gm.BuyItemPrefab);
        }
    }

    public GameObject CreatTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (slotsOnInterface[obj].item.Id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(100, 100);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.uiDisplay;
            img.raycastTarget = false;
        }
        return tempItem;
    }

    public GameObject CreatTempInfo(GameObject obj, GameObject prefab)
    {
        GameObject tempInfo = null;

        TextMeshProUGUI name;
        TextMeshProUGUI type;
        TextMeshProUGUI value;
        TextMeshProUGUI buffsDescript;

        if (slotsOnInterface[obj].item.Id >= 0)
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

            MouseData.buyItem = slotsOnInterface[obj].item;

            var img = tempInfo.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            var amt = tempInfo.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
            name = tempInfo.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            type = tempInfo.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
            value = tempInfo.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
            buffsDescript = tempInfo.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>();

            name.text = slotsOnInterface[obj].item.Name;
            type.text = slotsOnInterface[obj].ItemObject.type.ToString();
            value.text = slotsOnInterface[obj].item.ItemValue.ToString("n0") + " G";
            buffsDescript.text = slotsOnInterface[obj].item.buffList + slotsOnInterface[obj].ItemObject.description;

            img.sprite = slotsOnInterface[obj].ItemObject.uiDisplay;
            amt.text = slotsOnInterface[obj].amount.ToString("n0");
        }
        return tempInfo;
    }

    public void DestroyTempInfo()
    {
        Destroy(MouseData.slotItemInfo);
        MouseData.slotItemInfo = null;
    }

    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);

        if (MouseData.interfaceMouseIsOver == null)
        {
            slotsOnInterface[obj].RemoveItem();
            return;
        }
        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
        }
    }

    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
}

public static class MouseData
{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;
    public static GameObject slotItemInfo;
    public static GameObject buyIteminfo;
    public static Item buyItem;
}

public static class ExtensionMethods
{
    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> _slotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in _slotsOnInterface)
        {
            if (_slot.Value.item.Id >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.Value.ItemObject.uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
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