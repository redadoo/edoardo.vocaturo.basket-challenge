using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using System;
using Utility;

/// <summary>
/// Enum representing the different scenes available in the game.
/// </summary>
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

    /// <summary>
    /// Initiates asynchronous loading of the specified scene.
    /// If a scene is already loading, the call is ignored.
    /// </summary>
    /// <param name="scene">The scene to be loaded.</param>
    public void LoadScene(Scene scene)
    {
        if (isLoading)
        {
            Debug.LogWarning("A scene is already loading");
            return;
        }

        StartCoroutine(LoadSceneAsync(scene));
    }

    /// <summary>
    /// Coroutine that performs the asynchronous scene loading.
    /// Updates the current scene and triggers the OnSceneChange event upon completion.
    /// </summary>
    /// <param name="scene">The scene to load.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
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
