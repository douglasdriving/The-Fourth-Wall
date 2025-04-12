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

    private bool textPrintingComplete = false;

    void Start()
    {
        if (fadeInOnStart)
        {
            if (!string.IsNullOrEmpty(introText) && textPrinter != null)
            {
                StartCoroutine(ShowIntroAndFade());
            }
            else
            {
                StartCoroutine(FadeIn());
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
        yield return StartCoroutine(FadeIn());

        // Start narration after fade completes
        StartNarrationIfPresent();
        textPrinter.ClearText();
    }

    private void HandleTypingComplete()
    {
        textPrintingComplete = true;
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

        // If this was called directly (not through ShowIntroAndFade), start narration
        if (!string.IsNullOrEmpty(introText) && textPrinter != null)
        {
            // Don't start narration here as it will be started by ShowIntroAndFade
        }
        else
        {
            StartNarrationIfPresent();
        }
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
