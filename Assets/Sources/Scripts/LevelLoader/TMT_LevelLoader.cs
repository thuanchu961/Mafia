using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TMT_LevelLoader : MonoBehaviour
{
    // public GameObject loadingScene;
    public Slider slider;
    bool startGame;

    private void Start()
    {
        TMT_LoadLevel(1);
    }

    public void TMT_LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        startGame = true;
        // yield return new WaitForSeconds(3);
        yield return new WaitUntil(() => startGame);
        startGame = false;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        // loadingScene.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;

            yield return null;
        }
    }
}
