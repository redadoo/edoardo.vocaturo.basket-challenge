using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : GenericSingleton<LoadingScene>
{
    [SerializeField] private GameObject LoadingScreen;
    [SerializeField] private Image LoadingBar;

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        
        LoadingScreen.SetActive(true);

        while (!operation.isDone) 
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingBar.fillAmount = progressValue;

            yield return null;
        }
    }
}
