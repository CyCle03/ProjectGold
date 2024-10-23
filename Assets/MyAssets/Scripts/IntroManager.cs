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
    public GameObject NewPanel;
    public GameObject LoadPanel;
    public string savePath;
    public TextMeshProUGUI quickSave;
    public bool[] isSaveFile = new bool[4];
    bool loading;

    private void Awake()
    {
        CheckSaveFile();
    }

    private void Start()
    {
        NewPanel.SetActive(false);
        LoadPanel.SetActive(false);
        loading = false;
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
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath + 1)))
        {
            LoadPanel.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            NewPanel.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.1f);
            isSaveFile[0] = true;
        }
        else
        {
            LoadPanel.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.1f);
            NewPanel.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            isSaveFile[0] = false;
        }

        if (File.Exists(string.Concat(Application.persistentDataPath, savePath + 2)))
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

        if (File.Exists(string.Concat(Application.persistentDataPath, savePath + 3)))
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

        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            LoadPanel.transform.GetChild(3).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            isSaveFile[3] = true;
            quickSave.text = PlayerPrefs.GetInt("UseSlot").ToString();
        }
        else
        {
            LoadPanel.transform.GetChild(3).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.1f);
            isSaveFile[3] = false;
            quickSave.text = "";
        }
    }

    public void NewGame(int _slotNum)
    {
        if (!loading)
        {
            loading = true;
            StartCoroutine(TransitionNextScene(_slotNum));
        }
    }

    public void LoadGame(int _slotNum)
    {
        if (isSaveFile[_slotNum] && !loading)
        {
            loading = true;
            StartCoroutine(TransitionNextScene(_slotNum));
        }
    }

    IEnumerator TransitionNextScene(int num)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(1);
        ao.allowSceneActivation = false;

        if (num != 3)
        {
            PlayerPrefs.SetInt("UseSlot", num);
        }

        while (!ao.isDone)
        {
            if (ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }

            yield return null;
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