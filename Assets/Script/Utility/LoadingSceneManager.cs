using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using System;

public enum Scene
{
    MainMenu,
    Gameplay
}

public class LoadingSceneManager : PersistentSingleton<LoadingSceneManager>
{
    [SerializeField] private Scene currentScene;
    [SerializeField] private bool isLoading = false;
    public event EventHandler<Scene> OnSceneChange;

    public void LoadScene(Scene scene)
    {
        if (isLoading)
        {
            Debug.LogWarning("A scene is already loading");
            return;
        }

        StartCoroutine(LoadSceneAsync(scene));
    }

    IEnumerator LoadSceneAsync(Scene scene)
    {
        isLoading = true;

        int sceneId = (int)scene;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone) 
            yield return null;

        currentScene = scene;
        OnSceneChange?.Invoke(this, currentScene);
        isLoading = false;
    }
}
