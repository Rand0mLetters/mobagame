using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PreliminaryLoader : MonoBehaviour
{
    public AssetReference scene;
    public Button btn;
    public Slider progressSlider;
    public TextMeshProUGUI percentage;
    public TextMeshProUGUI btnText;
    public string[] labels;
    bool alreadyDownloaded = false;
    long size = -1;

    IEnumerator Start()
    {
        btn.interactable = false;
        AsyncOperationHandle<Int64> getDownloadSize = Addressables.GetDownloadSizeAsync(labels);
        yield return getDownloadSize;
        size = getDownloadSize.Result;
        Debug.Log("sizE: " + size);
        if (size == 0)
        {
            PlayGame();
            alreadyDownloaded = true;
            btnText.text = ". . .";
            btn.interactable = false;
        }
        else
        {
            btnText.text = "Download";
            btn.interactable = true;
        }
    }

    public void ButtonClicked()
    {
        if (alreadyDownloaded) { 
            PlayGame();
        }
        else
        {
            StartCoroutine("DownloadGame");
        }
    }

    void PlayGame()
    {
        Addressables.LoadSceneAsync(scene);
    }

    IEnumerator DownloadGame()
    {
        btn.interactable = false;
        progressSlider.gameObject.SetActive(true);
        AsyncOperationHandle downloadHandle = Addressables.DownloadDependenciesAsync(labels, Addressables.MergeMode.Union);
        while (downloadHandle.Status == AsyncOperationStatus.None) {
            float percentageComplete = downloadHandle.GetDownloadStatus().Percent;
            progressSlider.value = percentageComplete;
            percentage.text = ((int) ((percentageComplete * 100f) + 0.5f)).ToString() + "%";
            yield return null;
        }
        Debug.Log(size);
        PlayGame();
        btnText.text = ". . .";
        alreadyDownloaded = true;
    }
}
