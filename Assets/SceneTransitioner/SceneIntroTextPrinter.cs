using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class SceneIntroTextPrinter : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] TextMeshProUGUI introTextMeshPro;
    [SerializeField] float charactersPerSecond = 10f;
    [SerializeField] float timingVariation = 0.1f;

    [Header("Audio Settings")]
    [SerializeField] AudioSource audioSource;
    [SerializeField, Range(0.9f, 1.1f)] float minPitchVariation = 0.95f;
    [SerializeField, Range(0.9f, 1.1f)] float maxPitchVariation = 1.05f;
    [SerializeField, Range(0.8f, 1.0f)] float minVolumeVariation = 0.85f;
    [SerializeField, Range(0.8f, 1.0f)] float maxVolumeVariation = 1.0f;
    [SerializeField, Range(0f, 0.2f)] float stereoPanRange = 0.1f;

    private string fullText;
    private Coroutine printRoutine;
    private float baseVolume;
    private bool isTyping = false;
    public event Action OnTypingComplete;

    public float CharactersPerSecond => charactersPerSecond;
    public bool IsTyping => isTyping;

    private void Awake()
    {
        baseVolume = audioSource != null ? audioSource.volume : 1f;
    }

    public void PrintText(string text)
    {
        if (printRoutine != null)
        {
            StopCoroutine(printRoutine);
        }
        if (text == null || text.Length == 0)
        {
            introTextMeshPro.text = "";
            Debug.LogWarning("Attempted to print empty text.");
            return;
        }
        fullText = text;
        isTyping = true;
        printRoutine = StartCoroutine(PrintTextCharByChar());
    }

    public void ClearText()
    {
        introTextMeshPro.text = "";
    }

    private IEnumerator PrintTextCharByChar()
    {
        introTextMeshPro.text = "";
        float baseDelay = 1f / charactersPerSecond;

        for (int i = 0; i <= fullText.Length; i++)
        {
            introTextMeshPro.text = fullText.Substring(0, i);
            if (i < fullText.Length && !char.IsWhiteSpace(fullText[i]))
            {
                if (audioSource != null)
                {
                    // Vary pitch
                    audioSource.pitch = UnityEngine.Random.Range(minPitchVariation, maxPitchVariation);

                    // Vary volume
                    audioSource.volume = baseVolume * UnityEngine.Random.Range(minVolumeVariation, maxVolumeVariation);

                    // Vary stereo pan
                    audioSource.panStereo = UnityEngine.Random.Range(-stereoPanRange, stereoPanRange);

                    audioSource.Play();
                }
            }

            // Add slight random variation to timing
            float delay = baseDelay * (1f + UnityEngine.Random.Range(-timingVariation, timingVariation));
            yield return new WaitForSeconds(delay);
        }

        // Reset audio parameters
        if (audioSource != null)
        {
            audioSource.pitch = 1f;
            audioSource.volume = baseVolume;
            audioSource.panStereo = 0f;
        }

        isTyping = false;
        OnTypingComplete?.Invoke();
        printRoutine = null;
    }
}
