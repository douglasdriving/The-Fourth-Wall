using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// controls the black cover canvas of the screen
/// </summary>
public class BlackCoverCanvas : MonoBehaviour
{

    [SerializeField] GameObject cover;
    bool isPlayerToggleActive = false;

    void Update()
    {
        CheckPlayerToggle();
    }

    private void CheckPlayerToggle()
    {
        if (!isPlayerToggleActive) return;
        if (!Keyboard.current.lKey.wasPressedThisFrame) return;
        cover.SetActive(!cover.activeInHierarchy);
    }

    public void SetPlayerToggleActive(bool shouldBeActive)
    {
        isPlayerToggleActive = shouldBeActive;
    }

    public void SetCoverActive(bool shouldBeActive)
    {
        cover.SetActive(shouldBeActive);
    }
}
