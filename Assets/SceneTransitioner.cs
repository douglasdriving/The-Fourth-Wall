using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitioner : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] bool fadeInOnStart = true;
    [SerializeField] float fadeTime = 1;

    void Start()
    {
        if (fadeInOnStart) StartCoroutine(FadeIn());
        else fadeImage.gameObject.SetActive(false);
    }

    public IEnumerator FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        for (float i = fadeTime; i >= 0; i -= Time.deltaTime)
        {
            fadeImage.color = new Color(0, 0, 0, i);
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
    }

    public void EndScene(bool fadeOut = true)
    {
        if (fadeOut) StartCoroutine(FadeToNextScene());
        else LoadNextScene();
    }

    public IEnumerator FadeToNextScene()
    {
        fadeImage.gameObject.SetActive(true);
        for (float i = 0; i <= fadeTime; i += Time.deltaTime)
        {
            fadeImage.color = new Color(0, 0, 0, i);
            yield return null;
        }
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene in build settings.");
        }
    }
}
