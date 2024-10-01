using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int gold = 0;
    public TextMeshProUGUI textGold;
    public InventoryObject inventory;
    public InventoryObject equipment;
    public Canvas InventoryCanvas;
    public TextMeshProUGUI textHP;
    public GameObject shopScreen;

    bool isInventoryOn;
    bool isShopOn;

    // Start is called before the first frame update
    void Start()
    {
        GoldTextUpdate();
        InventoryCanvas.enabled = false;
        isInventoryOn = false;
        shopScreen.SetActive(false);
        isShopOn = false;
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
                CloseInventory();
                return;
            }
            OpenInventory();
        }
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
        {
            if (isShopOn)
            {
                ShopClose();
                return;
            }
            ShopOpen();
        }
        if (Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isShopOn)
            {
                ShopClose();
            }
            else if (isInventoryOn)
            {
                CloseInventory();
            }
        }
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
        shopScreen.SetActive(true);
        isShopOn = true;
        CursorOn();
    }

    public void ShopClose()
    {
        shopScreen.SetActive(false);
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

    public void GoldTextUpdate()
    {
        textGold.text = gold + " G";
    }
    public void AddGold(int _gold)
    {
        gold += _gold;
        GoldTextUpdate();
    }

    public void RemoveGold(int _gold)
    {
        gold -= _gold;
        GoldTextUpdate();
    }

    public void HPTextUpdate(float hp, float maxhp)
    {
        textHP.text = hp + " / " + maxhp;
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
    }
}
