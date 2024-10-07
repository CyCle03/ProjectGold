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

    bool isInventoryOn;
    bool isShopOn;
    bool isBuildOn;
    bool isTestOn;

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
                    {
                        print("Clear sell slots first.");
                        return;
                    }
                    ShopClose();
                }
                CloseInventory();
                return;
            }
            OpenInventory();
        }

        //Shop Window Controll
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isShopOn)
            {
                if (!ShopResetCheck())
                {
                    print("Clear sell slots first.");
                    return;
                }
                ShopClose();
                return;
            }
            if (!isInventoryOn)
            {
                OpenInventory();
            }
            ShopOpen();
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
                    return;
                }
                ShopClose();
            }
            else if (isInventoryOn)
            {
                CloseInventory();
            }
        }
    }

    public bool ShopResetCheck()
    {
        if (shop.OnSlotCount > inventory.EmptySlotCount)
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
        {
            return;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void HPTextUpdate(float hp, float maxhp)
    {
        textHP.text = hp.ToString("n0") + " / " + maxhp;
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
        shop.Clear();
        sell.Clear();
    }
}
