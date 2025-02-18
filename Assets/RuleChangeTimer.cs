using System.Collections;
using System.Collections.Generic;
using Narration;
using UnityEngine;

public class RuleChangeTimer : MonoBehaviour
{
    [SerializeField] float startGeneratingLevelAtSecond = -1;
    [SerializeField] float startPlayerMovementAtSecond = -1;

    void Start()
    {
        if (startGeneratingLevelAtSecond > 0) StartCoroutine(StartGeneratingLevelAfterDelay());
        if (startPlayerMovementAtSecond > 0) StartCoroutine(StartPlayerMovementAfterDelay());
    }

    IEnumerator StartGeneratingLevelAfterDelay()
    {
        yield return new WaitForSeconds(startGeneratingLevelAtSecond);
        FindObjectOfType<SubtitlePlayer>().EnableLevelGeneration();
    }

    IEnumerator StartPlayerMovementAfterDelay()
    {
        yield return new WaitForSeconds(startPlayerMovementAtSecond);
        FindObjectOfType<FirstPersonController>().enabled = true;
    }
}
