using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public Canvas IntroCanvas;
    public Canvas UICanvas;
    public GameObject NewPanel;
    public GameObject LoadPanel;
    public string savePath;
    public TextMeshProUGUI quickSave;
    public bool[] isSaveFile = new bool[4];

    GameManager gm;
    Player player;
    StarterAssetsInputs sai;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<Player>();
        sai = GameObject.Find("Player").GetComponent<StarterAssetsInputs>();

        ReturnMenu();
    }

    public void ReturnMenu()
    {
        CheckSaveFile();
        UICanvas.enabled = false;
        NewPanel.SetActive(false);
        LoadPanel.SetActive(false);
        IntroCanvas.enabled = true;
        sai.cursorLocked = false;
        gm.CursorOn();
    }

    public void NewPanelOpen()
    {
        NewPanel.SetActive(true);
    }

    public void NewPanelClose()
    {
        NewPanel.SetActive(false);
    }

    public void LoadPanelOpen()
    {
        LoadPanel.SetActive(true);
    }

    public void LoadPanelClose()
    {
        LoadPanel.SetActive(false);
    }

    public void CheckSaveFile()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            LoadPanel.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            isSaveFile[0] = true;
            quickSave.text = PlayerPrefs.GetInt("UseSlot").ToString();
        }
        else
        {
            LoadPanel.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.1f);
            isSaveFile[0] = false;
            quickSave.text = "";
        }

        if (File.Exists(string.Concat(Application.persistentDataPath, savePath + "1")))
        {
            LoadPanel.transform.GetChild(1).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            NewPanel.transform.GetChild(1).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.1f);
            isSaveFile[1] = true;
        }
        else
        {
            LoadPanel.transform.GetChild(1).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.1f);
            NewPanel.transform.GetChild(1).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            isSaveFile[1] = false;
        }

        if (File.Exists(string.Concat(Application.persistentDataPath, savePath + "2")))
        {
            LoadPanel.transform.GetChild(2).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            NewPanel.transform.GetChild(2).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.1f);
            isSaveFile[2] = true;
        }
        else
        {
            LoadPanel.transform.GetChild(2).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.1f);
            NewPanel.transform.GetChild(2).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            isSaveFile[2] = false;
        }

        if (File.Exists(string.Concat(Application.persistentDataPath, savePath + "3")))
        {
            LoadPanel.transform.GetChild(3).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            NewPanel.transform.GetChild(3).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.1f);
            isSaveFile[3] = true;
        }
        else
        {
            LoadPanel.transform.GetChild(3).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.1f);
            NewPanel.transform.GetChild(3).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            isSaveFile[3] = false;
        }
    }

    public void NewGame(int _slotNum)
    {
        PlayerPrefs.SetInt("UseSlot", -(_slotNum));
        IntroCanvas.enabled = false;
        gm.isStartGame = true;
        sai.cursorLocked = true;
        UICanvas.enabled = true;
        player.CheckSave();
        //SceneManager.LoadScene(1);
    }

    public void LoadGame(int _slotNum)
    {
        if (isSaveFile[_slotNum])
        {
            PlayerPrefs.SetInt("UseSlot", _slotNum);
            IntroCanvas.enabled = false;
            gm.isStartGame = true;
            sai.cursorLocked = true;
            UICanvas.enabled = true;
            player.CheckSave();
            //SceneManager.LoadScene(1);
        }
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}