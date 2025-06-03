using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Narration;

public class SceneTransitioner : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] bool fadeInOnStart = true;
    [SerializeField, TextArea(3, 10)] string introText = "";
    [SerializeField] float fadeTime = 1;
    [SerializeField] SceneIntroTextPrinter textPrinter;
    [SerializeField] NarrationManager narrationManager;
    [SerializeField] string fallBackNextSceneNameOverride = "";

    private bool textPrintingComplete = false;

    void Start()
    {
        if (fadeInOnStart)
        {
            if (introText != null && introText != "" && textPrinter != null)
            {
                StartCoroutine(ShowIntroAndFade());
            }
            else
            {
                StartCoroutine(FadeInAndStartNarraiton());
            }
        }
        else
        {
            fadeImage.gameObject.SetActive(false);
            StartNarrationIfPresent();
        }
    }

    private IEnumerator ShowIntroAndFade()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = Color.black;

        textPrintingComplete = false;
        textPrinter.OnTypingComplete += HandleTypingComplete;
        textPrinter.PrintText(introText);

        // Wait for typing to complete
        yield return new WaitUntil(() => textPrintingComplete);
        textPrinter.OnTypingComplete -= HandleTypingComplete;

        // Add a small pause after text completes before starting fade
        yield return new WaitForSeconds(0.5f);

        // Now start the fade
        yield return StartCoroutine(FadeInAndStartNarraiton());
        textPrinter.ClearText();
    }

    private void HandleTypingComplete()
    {
        textPrintingComplete = true;
    }

    public IEnumerator FadeInAndStartNarraiton()
    {
        fadeImage.gameObject.SetActive(true);
        for (float i = fadeTime; i >= 0; i -= Time.deltaTime)
        {
            fadeImage.color = new Color(0, 0, 0, i);
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
        StartNarrationIfPresent();
    }

    private void StartNarrationIfPresent()
    {
        if (narrationManager == null)
        {
            narrationManager = FindObjectOfType<NarrationManager>();
        }

        if (narrationManager != null)
        {
            narrationManager.PlayNarration();
        }
    }

    public void EndScene(bool fadeOut = true, string sceneNameOverride = "")
    {
        if (fadeOut) StartCoroutine(FadeToNextScene(sceneNameOverride));
        else LoadNextScene(sceneNameOverride);
    }

    public IEnumerator FadeToNextScene(string sceneNameOverride = "")
    {
        fadeImage.gameObject.SetActive(true);
        for (float i = 0; i <= fadeTime; i += Time.deltaTime)
        {
            fadeImage.color = new Color(0, 0, 0, i);
            yield return null;
        }
        LoadNextScene(sceneNameOverride);
    }

    private void LoadNextScene(string sceneNameOverride = "")
    {

        if (sceneNameOverride != "")
        {
            SceneManager.LoadScene(sceneNameOverride);
            return;
        }

        if (fallBackNextSceneNameOverride != "")
        {
            SceneManager.LoadScene(fallBackNextSceneNameOverride);
            return;
        }

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

    public void RestartScene()
    {
        FindObjectOfType<PauseMenu>().SetGamePaused(false);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}