using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{
    [SerializeField] string interactableName;
    [SerializeField] bool holdable = false;
    [SerializeField] UnityEvent interactionEvent;
    bool isInteractable = true;
    public bool IsInteractable
    {
        get { return isInteractable; }
    }
    public bool Holdable
    {
        get { return holdable; }
    }
    public string InteractableName
    {
        get { return interactableName; }
    }
    Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public void Interact()
    {
        if (interactionEvent != null && isInteractable)
        {
            interactionEvent.Invoke();
        }
    }

    public void Highlight()
    {
        outline.enabled = true;
    }

    public void RemoveHighlight()
    {
        outline.enabled = false;
    }

    public void RemoveInteractability()
    {
        isInteractable = false;
    }
}
