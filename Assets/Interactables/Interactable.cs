using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{
    [SerializeField] string interactableName;
    [SerializeField] UnityEvent interactionEvent;
    [SerializeField] UnityEvent secondaryInteractionEvent;
    bool isInteractable = true;
    public bool IsInteractable
    {
        get { return isInteractable; }
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

    public void InteractSecondary()
    {
        if (secondaryInteractionEvent != null && isInteractable)
        {
            secondaryInteractionEvent.Invoke();
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
