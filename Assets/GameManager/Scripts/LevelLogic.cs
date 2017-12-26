using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLogic : MonoBehaviour
{
    [Header("Scene state")]
    public int backScene;
    public static int currentScene;
    public int nextScene;
    int managerScene;
    int sceneCountInBuildSettings;

    [Header("Loader")]
    int sceneToLoad;
    bool loading = false;
    AsyncOperation loadAsync = null;
    AsyncOperation unloadAsync = null; 
    float fadeTime = 1.0f;

    [Header("UI")]
    [SerializeField]
    Image blackScreen;

    void Start()
    {
        blackScreen.color = Color.black;
        blackScreen.CrossFadeAlpha(0, fadeTime, true);

        if (SceneManager.sceneCount >= 2) SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));

        UpdateSceneState();

        if (currentScene == managerScene) StartLoad(nextScene);
    }

    void UpdateSceneState()
    {
        sceneCountInBuildSettings = SceneManager.sceneCountInBuildSettings;

        managerScene = 0;
        currentScene = SceneManager.GetActiveScene().buildIndex;

        if (currentScene <= managerScene + 1) backScene = sceneCountInBuildSettings - 1;
        else backScene = currentScene - 1;

        if (currentScene >= sceneCountInBuildSettings - 1) nextScene = 1;
        else nextScene = currentScene + 1;
    }

    public void StartLoad(int index)
    {
        if (loading) return;
        loading = true;
        sceneToLoad = index;
        FadeOut();
    }

    void Load()
    {
        if (sceneToLoad == -1)
        {
            Application.Quit();
            return;
        }

        if (currentScene != managerScene) unloadAsync = SceneManager.UnloadSceneAsync(currentScene);
        loadAsync = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        StartCoroutine(Loading());
    }

    void FadeIn()
    {
        blackScreen.CrossFadeAlpha(0, fadeTime, true);
    }
    void FadeOut()
    { 
        blackScreen.CrossFadeAlpha(1.0f, fadeTime, true);
        StartCoroutine(WaitForFade());
    }

    IEnumerator Loading()
    {
        while (loading)
        {
            if ((unloadAsync == null || unloadAsync.isDone) && loadAsync.isDone)
            {
                unloadAsync = null;
                loadAsync = null;

                FadeIn();

                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneToLoad));
                UpdateSceneState();

                loading = false;
            }
            yield return null;
        }
    }
    IEnumerator WaitForFade()
    {
        yield return new WaitForSeconds(fadeTime);
        Load();
    }
}
