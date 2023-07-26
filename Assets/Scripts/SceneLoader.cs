using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    [SerializeField] private TextMeshProUGUI loadingText;

    [SerializeField] private GameObject loaderPanel;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;

        StartCoroutine(LoadSceneAsyncCoroutine("OkiGenTest"));
    }

    public IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        loadingText.text = "Loading: 0%";

        loaderPanel.SetActive(true);
        loaderPanel.GetComponent<CanvasGroup>().DOFade(1, .5f);
        yield return new WaitForSeconds(1);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
         while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingText.text = "Loading: " + ((int)progress * 100) + "%";

            yield return null;
        }

        loaderPanel.GetComponent<CanvasGroup>().DOFade(0, .5f).OnComplete(() => loaderPanel.SetActive(false));
    }
}