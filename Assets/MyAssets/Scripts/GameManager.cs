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

    bool isUIOn;

    // Start is called before the first frame update
    void Start()
    {
        UpdateGoldText();
        InventoryCanvas.enabled = false;
        isUIOn = false;
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
            if (isUIOn)
            {
                CloseInventory();
                return;
            }
            OpenInventory();
        }
        if (Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isUIOn)
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
        isUIOn = true;
        CursorOn();
    }

    public void CloseInventory()
    {
        InventoryCanvas.enabled = false;
        isUIOn = false;
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

    public void UpdateGoldText()
    {
        textGold.text = gold + " G";
    }
    public void AddGold(int _gold)
    {
        gold += _gold;
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
    }
}
