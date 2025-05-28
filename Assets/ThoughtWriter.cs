using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThoughtWriter : MonoBehaviour
{
    [SerializeField] TMP_InputField thoughtInputField;
    bool isWritingThoughts = false;
    public void EnableThoughtWriting()
    {
        thoughtInputField.gameObject.SetActive(true);
        isWritingThoughts = true;
        thoughtInputField.ActivateInputField();
        thoughtInputField.text = "";
    }

    private void Update()
    {
        if (isWritingThoughts)
        {
            EnforeFieldFocus();
            SubmitIfCtrlEnterPressed();
        }
    }

    private void EnforeFieldFocus()
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
            Debug.Log("Thought submitted: " + thought);
            thoughtInputField.gameObject.SetActive(false);
            isWritingThoughts = false;
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
}

