using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;
    public InventoryObject shop;
    public InventoryObject sell;
    public BuildingListObject build;
    public BuildingListObject shopBuild;
    public Canvas InventoryCanvas;
    public TextMeshProUGUI textHP;
    public GameObject ShopScreen;
    public GameObject SellScreen;
    public GameObject InvenScreen;
    public GameObject EquipScreen;
    public GameObject BuildScreen;
    public GameObject BuildShopScreen;
    public GameObject AlertScreen;
    public GameObject StatScreen;
    public GameObject InvenGoldBar;
    public GameObject ItemInfoPrefab;
    public GameObject BuyItemPrefab;
    public InventoryObject tutorial;
    public Canvas OptionCanvas;
    public GameObject OptionPanel;

    TextMeshProUGUI AlertMsg;

    public bool isStartGame;
    bool isInventoryOn;
    bool isShopOn;
    bool isBuildOn;
    bool isBShopOn;
    bool isMsgOn;
    bool isStatOn;
    bool isOptionOn;

    bool[] isSaveFile = new bool[4];

    int alertCnt;
    float alertTimer;

    Building tempBuild;
    Player player;
    IntroManager introManager;

    // Start is called before the first frame update
    void Start()
    {
        isStartGame = false;
        player = GameObject.Find("Player").GetComponent<Player>();
        introManager = GameObject.Find("IntroManager").GetComponent<IntroManager>();
        SetUI();
    }

    public void SetUI()
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

        BuildShopScreen.SetActive(false);
        isBShopOn = false;

        StatScreen.SetActive(false);
        isStatOn = false;

        OptionCanvas.enabled = false;
        isOptionOn = false;

        InvenGoldBar.SetActive(false);

        AlertMsg = AlertScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        AlertScreen.SetActive(false);
        isMsgOn = false;
        alertCnt = 0;
        alertTimer = 0f;

        tempBuild = null;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isStartGame)
        {
            if (Input.GetKeyDown(KeyCode.PageUp))
            { SaveInventory(); }
            if (Input.GetKeyDown(KeyCode.PageDown))
            { LoadInventory(); }

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
                        { ShopClose(); }
                    }
                    CloseInventory();
                }
                else
                {
                    if (isBuildOn)
                    {
                        if (isBShopOn)
                        { BShopClose(); }
                        BuildClose();
                    }
                    OpenInventory();
                }
            }

            //Shop Window Controll
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (isShopOn)
                {
                    if (!ShopResetCheck())
                    { Alert("Clear sell slots first."); }
                    else
                    { ShopClose(); }
                }
                else
                {
                    if (tempBuild != null && tempBuild.type == BuildType.Store)
                    {
                        if (isBuildOn)
                        {
                            if (isBShopOn)
                            { BShopClose(); }
                            BuildClose();
                        }
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
                    if (isBShopOn)
                    { BShopClose(); }
                    BuildClose();
                }
                else
                {
                    if (isInventoryOn)
                    {
                        if (isShopOn)
                        { ShopClose(); }
                        CloseInventory();
                    }
                    BuildOpen();
                }
            }

            //Build Shop Window Controll
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (isBShopOn)
                {
                    BShopClose();
                    return;
                }
                if (isInventoryOn)
                {
                    if (isShopOn)
                    { ShopClose(); }
                    CloseInventory();
                }
                if (!isBuildOn)
                { BuildOpen(); }
                BShopOpen();
            }

            //Close Window
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isOptionOn)
                {
                    OptionClose();
                }
                else if (isBShopOn)
                {
                    BShopClose();
                }
                else if (isBuildOn)
                {
                    BuildClose();
                }
                else if (isShopOn)
                {
                    if (!ShopResetCheck())
                    { Alert("Clear sell slots first."); }
                    else
                    { ShopClose(); }
                }
                else if (isInventoryOn)
                { CloseInventory(); }
                else
                { OptionOpen(); }
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
                        if (isBuildOn)
                        {
                            if (isBShopOn)
                            { BShopClose(); }
                            BuildClose();
                        }
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

            //Stat Screen Controll
            if (Input.GetKeyDown(KeyCode.O))
            {
                if (isStatOn)
                {
                    StatClose();
                }
                else
                {
                    if (isBuildOn)
                    {
                        if (isBShopOn)
                        { BShopClose(); }
                        BuildClose();
                    }
                    StatOpen();
                }
            }

            //Cursor Controll
            if (Input.GetKeyDown(KeyCode.C))
            { CursorOn(); }

            if (Input.GetKeyDown(KeyCode.V))
            { CursorOff(); }

            //gold cheat
            if (Input.GetKeyDown(KeyCode.G))
            { inventory.AddGold(1000); player.InvenGoldUpdate(); }

            //Move character Origin Pos
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                player.transform.position = Vector3.zero;
            }

            if (isMsgOn)
            {
                alertTimer -= Time.deltaTime;
                if (alertTimer <= 0)
                {
                    Alert();
                }
            }
        }
    }

    public void ItemInfoClose()
    {
        if (MouseData.slotItemInfo != null)
        { InvenScreen.GetComponent<DynamicInterface>().DestroyTempInfo(); }
    }

    public void ShopInfoClose()
    {
        if (MouseData.buyIteminfo != null)
        { ShopScreen.GetComponent<ShopInterface>().DestroyBuyInfo(); }
    }

    public void BuildInfoClose()
    {
        if (BuildMouseData.slotBuildInfo != null)
        { BuildScreen.GetComponent<DynamicBuild>().DestroyTempInfo(); }
    }

    public void BuildShopClose()
    {
        if (BuildMouseData.buyBuildinfo != null)
        { BuildShopScreen.GetComponent<BShopInterface>().DestroyBuyInfo(); }
    }

    public bool ShopResetCheck()
    {
        if (sell.OnSlotCount > inventory.EmptySlotCount)
        {
            Alert("Not enough slots on Inventory");
            return false;
        }
        return true;
    }

    public void SaveInventory()
    {
        inventory.Save();
        equipment.Save();
        build.Save();
        player.Save();
        Debug.Log("Save");
        PlayerPrefs.SetInt("UseSlot", 0);
    }

    public void SaveInventory(int _slotNum)
    {
        inventory.Save(_slotNum);
        equipment.Save(_slotNum);
        build.Save(_slotNum);
        player.Save(_slotNum);
        Debug.Log("Save" + _slotNum);
        PlayerPrefs.SetInt("UseSlot", _slotNum);
        CheckSaveFile(_slotNum);
    }

    public void LoadInventory()
    {
        inventory.Load();
        player.InvenGoldUpdate();
        equipment.Load();
        build.Load();
        player.Load();
        Debug.Log("Load");
        PlayerPrefs.SetInt("UseSlot", 0);
    }

    public void LoadInventory(int _slotNum)
    {
        inventory.Load(_slotNum);
        player.InvenGoldUpdate();   
        equipment.Load(_slotNum);
        build.Load(_slotNum);
        player.Load(_slotNum);
        Debug.Log("Load" + _slotNum);
        PlayerPrefs.SetInt("UseSlot", _slotNum);
        CheckSaveFile(_slotNum);
    }

    public void OpenInventory()
    {
        CursorOn();
        InvenScreen.SetActive(true);
        EquipScreen.SetActive(true);
        InvenGoldBar.SetActive(true);
        isInventoryOn = true;
    }

    public void CloseInventory()
    {
        InvenScreen.SetActive(false);
        EquipScreen.SetActive(false);
        InvenGoldBar.SetActive(false);
        isInventoryOn = false;
        CursorOff();
        ItemInfoClose();
    }

    public void ShopOpen()
    {
        CursorOn();
        ShopScreen.SetActive(true);
        SellScreen.SetActive(true);
        isShopOn = true;
    }

    public void ShopClose()
    {
        player.ShopAddInventory();
        ShopScreen.SetActive(false);
        SellScreen.SetActive(false);
        isShopOn = false;
        CursorOff();
        ShopInfoClose();
    }

    public void BuildOpen()
    {   
        CursorOn();
        BuildScreen.SetActive(true);
        InvenGoldBar.SetActive(true);
        isBuildOn = true;
    }

    public void BuildClose()
    {
        BuildScreen.SetActive(false);
        InvenGoldBar.SetActive(false);
        isBuildOn = false;
        CursorOff();
        BuildInfoClose();
    }

    public void BShopOpen()
    {   
        CursorOn();
        BuildShopScreen.SetActive(true);
        isBShopOn = true;
    }

    public void BShopClose()
    {
        BuildShopScreen.SetActive(false);
        isBShopOn = false;
        CursorOff();
        BuildShopClose();
    }

    public void StatOpen()
    {
        CursorOn();
        StatScreen.SetActive(true);
        isStatOn = true;
    }

    public void StatClose()
    {
        StatScreen.SetActive(false);
        isStatOn = false;
        CursorOff();
    }

    public void OptionOpen()
    {
        OptionCanvas.enabled = true;
        isOptionOn = true;
        CheckSaveFile();
        Time.timeScale = 0f;
        CursorOn();
    }

    public void OptionClose()
    {
        OptionCanvas.enabled = false;
        isOptionOn = false;
        Time.timeScale = 1f;
        CursorOff();
    }

    public void CursorOn()
    {
        InventoryCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CursorOff()
    {
        if (isInventoryOn || isStatOn || isBuildOn || isOptionOn)
        { return; }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InventoryCanvas.enabled = false;
    }

    public void HPTextUpdate(float hp, float maxhp)
    { textHP.text = hp.ToString("n0") + " / " + maxhp; }

    public void InteractBuild(Building _build)
    {
        Alert("Press 'E' To Interact with " + _build.BuildName);
        tempBuild = _build;
    }

    public void InteractBuild()
    {
        Alert();
        tempBuild = null;
    }

    public void Alert(string _msg)
    {
        if (isMsgOn)
        {
            if(alertCnt <= 5)
            {
                alertTimer += 3f;

                if (alertTimer >= 5f)
                { alertTimer = 5f; }
                
                AlertMsg.text += "\n" + _msg;
            }
        }
        else
        {
            alertTimer = 3f;
            AlertScreen.SetActive(true);
            AlertMsg.text = _msg;
            isMsgOn = true;
        }
        alertCnt++;
        //StartCoroutine(MsgTime(_msg));
    }

    IEnumerator MsgTime(string _msg)
    {
        yield return new WaitForSeconds(3);
        //if (alertCnt == 1)
        Alert();
        /*else
        {
            AlertMsg.text.Replace("\n" + _msg, "");
            alertCnt--;
        }*/
    }

    public void Alert()
    {
        if (isMsgOn)
        {
            AlertMsg.text = "";
            AlertScreen.SetActive(false);
            alertTimer = 0f;
            alertCnt = 0;
            isMsgOn = false;
        }
    }

    public void DieMsg()
    {
        if (!isMsgOn)
        {
            alertTimer = 3f;
            AlertScreen.SetActive(true);
            isMsgOn = true;
        }
        AlertMsg.text = "You Died";
    }

    public bool TempBuildType(BuildType _buildType)
    {
        if (tempBuild != null)
        {
            if (tempBuild.type == _buildType)
            { return true; }
        }
        return false;
    }

    public void CheckSaveFile()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, player.savePath + "1")))
        {
            OptionPanel.transform.GetChild(1).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            OptionPanel.transform.GetChild(4).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            isSaveFile[1] = true;
        }
        else
        {
            OptionPanel.transform.GetChild(4).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.1f);
            isSaveFile[1] = false;
        }

        if (File.Exists(string.Concat(Application.persistentDataPath, player.savePath + "2")))
        {
            OptionPanel.transform.GetChild(2).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            OptionPanel.transform.GetChild(5).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            isSaveFile[2] = true;
        }
        else
        {
            OptionPanel.transform.GetChild(5).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.1f);
            isSaveFile[2] = false;
        }

        if (File.Exists(string.Concat(Application.persistentDataPath, player.savePath + "3")))
        {
            OptionPanel.transform.GetChild(3).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            OptionPanel.transform.GetChild(6).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            isSaveFile[3] = true;
        }
        else
        {
            OptionPanel.transform.GetChild(6).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.1f);
            isSaveFile[3] = false;
        }
    }

    public void CheckSaveFile(int _slotNum)
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, player.savePath + _slotNum)))
        {
            OptionPanel.transform.GetChild(_slotNum).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            OptionPanel.transform.GetChild(_slotNum + 3).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            isSaveFile[_slotNum] = true;
        }
        else
        {
            OptionPanel.transform.GetChild(_slotNum + 3).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.1f);
            isSaveFile[_slotNum] = false;
        }
    }

    public void MainReturn()
    {
        SceneManager.LoadScene(0);
        introManager.ReturnMenu();
        //DontDestroyOnLoad(); animator
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
        sell.Clear();
        shop.Clear();
        shopBuild.Clear();
        build.Clear();
        tutorial.Clear();
    }
}
