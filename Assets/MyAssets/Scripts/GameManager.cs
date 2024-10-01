using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;
    public InventoryObject shop;
    public Canvas InventoryCanvas;
    public TextMeshProUGUI textHP;
    public Canvas shopCanvas;

    bool isInventoryOn;
    bool isShopOn;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        InventoryCanvas.enabled = false;
        isInventoryOn = false;
        shopCanvas.enabled = false;
        isShopOn = false;
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
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.B))
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
        if (Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.Escape))
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
    }

    public void LoadInventory()
    {
        inventory.Load();
        equipment.Load();
    }

    public void OpenInventory()
    {
        InventoryCanvas.enabled = true;
        isInventoryOn = true;
        CursorOn();
    }

    public void CloseInventory()
    {
        InventoryCanvas.enabled = false;
        isInventoryOn = false;
        CursorOff();
    }

    public void ShopOpen()
    {
        shopCanvas.enabled = true;
        isShopOn = true;
        CursorOn();
    }

    public void ShopClose()
    {
        player.ShopAddInventory();
        shopCanvas.enabled = false;
        isShopOn = false;
        CursorOff();
    }

    public void CursorOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CursorOff()
    {
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
    }
}
