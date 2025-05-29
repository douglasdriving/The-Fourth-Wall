using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThoughtWriter : MonoBehaviour
{
    [SerializeField] GameObject thoughtCanvas;
    [SerializeField] TMP_InputField thoughtInputField;
    bool isWritingThoughts = false;
    public void EnableThoughtWriting()
    {
        thoughtCanvas.SetActive(true);
        isWritingThoughts = true;
        thoughtInputField.ActivateInputField();
        thoughtInputField.text = "";
    }

    private void Update()
    {
        if (isWritingThoughts)
        {
            EnforceFieldFocus();
            SubmitIfCtrlEnterPressed();
        }
    }

    private void EnforceFieldFocus()
    {
        if (!thoughtInputField.isFocused)
        {
            thoughtInputField.ActivateInputField();
        }
    }

    private void SubmitIfCtrlEnterPressed()
    {
        bool enterPressed = Input.GetKeyDown(KeyCode.Return);
        if (!enterPressed) return;

        bool ctrlOrCmdPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);
        if (!ctrlOrCmdPressed) return;

        string thought = thoughtInputField.text.Trim();
        if (!string.IsNullOrEmpty(thought))
        {
            thoughtCanvas.SetActive(false);
            isWritingThoughts = false;
            SubmitThought(thought);
        }
        else
        {
            thoughtInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "dont submit an empty thought!";
            thoughtInputField.text = "";
            StartCoroutine(BlockWritingTemporarily(1f));
        }
    }

    private IEnumerator BlockWritingTemporarily(float delay)
    {
        isWritingThoughts = false;
        thoughtInputField.DeactivateInputField();
        yield return new WaitForSeconds(delay);
        thoughtInputField.ActivateInputField();
        isWritingThoughts = true;
    }

    private void SubmitThought(string thought)
    {
        SceneInfo sceneInfo = FindObjectOfType<SceneInfo>();
        if (sceneInfo == null || sceneInfo.sceneId == -1)
        {
            Debug.LogError("SceneInfo not found or sceneId is not set. Cannot submit thought.");
            return;
        }
        StartCoroutine(SupabaseConnect.AddThought(thought, sceneInfo.sceneId, OnSubmitComplete));
    }

    private void OnSubmitComplete()
    {
        SceneTransitioner sceneTransitioner = FindObjectOfType<SceneTransitioner>();
        if (sceneTransitioner != null)
        {
            sceneTransitioner.EndScene();
        }
        else
        {
            Debug.LogError("SceneTransitioner not found in the scene. Cannot end scene after thought submission.");
        }
    }
}

