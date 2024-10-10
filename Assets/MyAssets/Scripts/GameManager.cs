using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;
    public InventoryObject shop;
    public InventoryObject sell;
    public BuildingListObject build;
    public Canvas InventoryCanvas;
    public TextMeshProUGUI textHP;
    public GameObject ShopScreen;
    public GameObject SellScreen;
    public GameObject InvenScreen;
    public GameObject EquipScreen;
    public GameObject BuildScreen;
    public GameObject TestScreen;
    public GameObject AlertScreen;
    public GameObject ItemInfoPrefab;
    public GameObject BuyItemPrefab;

    TextMeshProUGUI AlertMsg;

    bool isInventoryOn;
    bool isShopOn;
    bool isBuildOn;
    bool isTestOn;
    bool isMsgOn;

    Building tempBuild;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        InventoryCanvas.enabled = false;
        InvenScreen.SetActive(false);
        EquipScreen.SetActive(false);
        isInventoryOn = false;

        ShopScreen.SetActive(false);
        SellScreen.SetActive(false);
        isShopOn = false;

        BuildScreen.SetActive(false);
        isBuildOn = false;

        TestScreen.SetActive(false);
        isTestOn = false;

        AlertMsg = AlertScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        AlertScreen.SetActive(false);
        isMsgOn = false;

        tempBuild = null;

        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveInventory();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            LoadInventory();
        }

        //Inventory Window Controll
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
        {
            if (isInventoryOn)
            {
                if (isShopOn)
                {
                    if (!ShopResetCheck())
                    { Alert("Clear sell slots first."); }
                    else
                    {
                        ShopClose();
                    }
                }
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
            
        }

        //Shop Window Controll
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isShopOn)
            {
                if (!ShopResetCheck())
                {
                    Alert("Clear sell slots first.");
                }
                else
                {
                    ShopClose();
                }
            }
            else
            {
                if (tempBuild.type == BuildType.Store)
                {
                    if (!isInventoryOn)
                        { OpenInventory(); }
                    ShopOpen();
                }
                
            } 
        }

        //Build Window Controll
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isBuildOn)
            {
                if (isInventoryOn)
                {
                    InvenScreen.SetActive(true);
                }
                else
                {
                    InventoryCanvas.enabled = false;
                }
                BuildClose();
                return;
            }
            if (isInventoryOn)
            {
                InvenScreen.SetActive(false);
            }
            else
            {
                InventoryCanvas.enabled = true;
            }
            BuildOpen();
        }

        //Test Window Controll
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (isTestOn)
            {
                if (isInventoryOn)
                {
                    EquipScreen.SetActive(true);
                }
                else
                {
                    InventoryCanvas.enabled = false;
                }
                TestScreen.SetActive(false);
                isTestOn = false;
                CursorOff();
                return;
            }
            if (isInventoryOn)
            {
                EquipScreen.SetActive(false);
            }
            else
            {
                InventoryCanvas.enabled = true;
            }
            TestScreen.SetActive(true);
            isTestOn = true;
            CursorOn();
        }

        //Close Window
        if (Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isBuildOn)
            {
                if (isInventoryOn)
                {
                    InvenScreen.SetActive(true);
                }
                else
                {
                    InventoryCanvas.enabled = false;
                }
                BuildClose();
            }
            else if (isShopOn)
            {
                if (!ShopResetCheck())
                {
                    print("Clear sell slots first.");
                }
                else
                ShopClose();
            }
            else if (isInventoryOn)
            { CloseInventory(); }
        }

        //Interact Build
        if (tempBuild != null && Input.GetKeyDown(KeyCode.E))
        {
            switch (tempBuild.type)
            {
                case BuildType.House:
                    break;
                case BuildType.Farm:
                    break;
                case BuildType.Store:
                    if (!isInventoryOn)
                    { OpenInventory(); }
                    ShopOpen();
                    break;
                case BuildType.Smith:
                    break;
                case BuildType.Stable:
                    break;
                case BuildType.AnimalFarm:
                    break;
                case BuildType.Tarvern:
                    break;
                case BuildType.Castle:
                    break;
                case BuildType.Church:
                    break;
                case BuildType.Windmill:
                    break;
                default:
                    break;
            }
        }

        //Cursor Controll
        if (Input.GetKeyDown(KeyCode.C))
        { CursorOn(); }
    }

    public void ItemInfoClose()
    {
        if (MouseData.slotItemInfo != null)
        { InvenScreen.GetComponent<DynamicInterface>().DestroyTempInfo(); }
    }

    public bool ShopResetCheck()
    {
        if (sell.OnSlotCount > inventory.EmptySlotCount)
        {
            print("Not enough slots on Inventory");
            return false;
        }
        return true;
    }

    public void SaveInventory()
    {
        inventory.Save();
        equipment.Save();
        build.Save();
    }

    public void LoadInventory()
    {
        inventory.Load();
        equipment.Load();
        build.Load();
    }

    public void OpenInventory()
    {
        InventoryCanvas.enabled = true;
        InvenScreen.SetActive(true);
        EquipScreen.SetActive(true);
        isInventoryOn = true;
        CursorOn();
    }

    public void CloseInventory()
    {
        InventoryCanvas.enabled = false;
        InvenScreen.SetActive(false);
        EquipScreen.SetActive(false);
        isInventoryOn = false;
        CursorOff();
        ItemInfoClose();
    }

    public void ShopOpen()
    {
        ShopScreen.SetActive(true);
        SellScreen.SetActive(true);
        isShopOn = true;
        CursorOn();
    }

    public void ShopClose()
    {
        player.ShopAddInventory();
        ShopScreen.SetActive(false);
        SellScreen.SetActive(false);
        isShopOn = false;
        CursorOff();
        ItemInfoClose();
    }

    public void BuildOpen()
    {
        BuildScreen.SetActive(true);
        isBuildOn = true;
        CursorOn();
    }

    public void BuildClose()
    {
        BuildScreen.SetActive(false);
        isBuildOn = false;
        CursorOff();
    }

    public void CursorOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CursorOff()
    {
        if (isInventoryOn)
        { return; }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void HPTextUpdate(float hp, float maxhp)
    { textHP.text = hp.ToString("n0") + " / " + maxhp; }

    public void InteractBuild(Building _build)
    {
        AlertScreen.SetActive(true);
        AlertMsg.text = "Press 'E' To Interact with " + _build.BuildName;
        isMsgOn = true;
        tempBuild = _build;
    }

    public void InteractBuild()
    {
        AlertScreen.SetActive(false);
        AlertMsg.text = "";
        isMsgOn = false;
        tempBuild = null;
    }

    public void Alert(string _msg)
    {
        if (isMsgOn)
        { AlertMsg.text += "\n" + _msg; }
        else
        {
            AlertScreen.SetActive(true);
            AlertMsg.text = _msg;
            isMsgOn = true;
        }
        StartCoroutine(MsgTime(_msg));
    }

    IEnumerator MsgTime(string _msg)
    {
        yield return new WaitForSeconds(5);
        if (AlertMsg.text == _msg)
        { Alert(); }
        else
        { AlertMsg.text.Replace("\n" + _msg, ""); }
    }

    public void Alert()
    {
        if (isMsgOn)
        {
            AlertScreen.SetActive(false);
            AlertMsg.text = "";
            isMsgOn = false;
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
        sell.Clear();
        shop.Clear();
    }
}
