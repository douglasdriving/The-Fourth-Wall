using System.Collections;
using System.Collections.Generic;
using Narration;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    bool isPaused = false;
    NarrationManager narrationManager;
    FirstPersonController playerController;

    private void Start()
    {
        narrationManager = FindObjectOfType<NarrationManager>();
        playerController = FindObjectOfType<FirstPersonController>();
        SetCursorEnabled(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetGamePaused(!isPaused);
        }
    }

    public void SetGamePaused(bool paused)
    {
        if (paused)
        {
            narrationManager.Pause();
        }
        else
        {
            narrationManager.Resume();
        }
        playerController.SetPlayerFrozen(paused);
        Time.timeScale = paused ? 0f : 1f;
        canvas.SetActive(paused);
        isPaused = paused;
        SetCursorEnabled(paused);
    }

    private static void SetCursorEnabled(bool paused)
    {
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;
    }

    public bool CheckIsPaused()
    {
        return isPaused;
    }
}
