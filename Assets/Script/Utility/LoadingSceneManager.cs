using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public enum Scene
{
    MainMenu,
    Gameplay
}

public class LoadingSceneManager : GenericSingleton<LoadingSceneManager>
{
    //[SerializeField] private GameObject LoadingScreen;
    [SerializeField] private Scene currentScene;
    [SerializeField] private Image LoadingBar;

    public event EventHandler<Scene> OnSceneChange;

    public void LoadScene(Scene scene)
    {
        StartCoroutine(LoadSceneAsync(scene));
    }

    IEnumerator LoadSceneAsync(Scene scene)
    {
        int sceneId = (int)scene;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        
        //LoadingScreen.SetActive(true);

        while (!operation.isDone) 
        {
            //float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            //LoadingBar.fillAmount = progressValue;
            yield return null;
        }

        currentScene = scene;
        OnSceneChange?.Invoke(this, currentScene);
    }
}
