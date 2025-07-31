using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using Utility;
using System;

/// <summary>
/// Enum representing the different scenes available in the game.
/// </summary>
public enum Scene
{
    MainMenu,
    Gameplay
}

/// <summary>
/// Manages scene transitions and broadcasts scene change events.
/// </summary>
public class LoadingSceneManager : PersistentSingleton<LoadingSceneManager>
{
    [Header("Scene Management")]
    [SerializeField] private Scene currentScene;
    [SerializeField] private bool isLoading = false;

    public event EventHandler<Scene> OnSceneChange;

    /// <summary>
    /// Starts loading the given scene asynchronously.
    /// If a scene is already loading, the method exits early.
    /// </summary>
    public void LoadScene(Scene scene)
    {
        if (isLoading)
        {
            Debug.LogWarning("A scene is already loading.");
            return;
        }

        StartCoroutine(LoadSceneAsync(scene));
    }

    /// <summary>
    /// Coroutine responsible for loading a new scene asynchronously.
    /// Updates internal state and notifies listeners upon completion.
    /// </summary>
    private IEnumerator LoadSceneAsync(Scene scene)
    {
        isLoading = true;

        AsyncOperation operation = SceneManager.LoadSceneAsync((int)scene);

        while (!operation.isDone)
            yield return null;

        currentScene = scene;
        isLoading = false;

        OnSceneChange?.Invoke(this, currentScene);
    }
}
