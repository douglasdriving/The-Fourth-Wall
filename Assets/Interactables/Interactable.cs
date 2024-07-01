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
    public bool Holdable
    {
        get { return holdable; }
    }
    Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public void Interact()
    {
        if (interactionEvent != null)
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
}
