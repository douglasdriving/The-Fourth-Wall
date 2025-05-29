using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FirstPersonController))]
public class PlayerStartFreezer : MonoBehaviour
{
    private FirstPersonController playerController;

    [SerializeField] float timeBeforePlayerCanMove = 0f;

    private void Awake()
    {
        playerController = GetComponent<FirstPersonController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerStartFreezer requires a FirstPersonController component.");
        }
    }

    private void Start()
    {
        if (playerController != null && timeBeforePlayerCanMove > 0f)
        {
            StartCoroutine(FreezePlayerForDuration(timeBeforePlayerCanMove));
        }
    }

    private IEnumerator FreezePlayerForDuration(float duration)
    {
        playerController.SetPlayerFrozen(true);
        yield return new WaitForSeconds(duration);
        playerController.SetPlayerFrozen(false);
    }
}
