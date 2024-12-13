using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace QuizPortal
{
    public class PortalQuizAnswerButton : MonoBehaviour
    {
        public bool isCorrectAnswer = false;

        void Update()
        {
            if (Input.GetMouseButtonDown(0)) // When the user left-clicks
            {
                if (isCorrectAnswer) // Check if this is the correct button
                {
                    // Create a PointerEventData for the raycast
                    PointerEventData pointerData = new PointerEventData(EventSystem.current)
                    {
                        position = new Vector2(Screen.width / 2f, Screen.height / 2f) // Crosshair at screen center
                    };
                    // Perform a raycast using the EventSystem
                    var results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerData, results);
                    foreach (var result in results)
                    {
                        if (result.gameObject == gameObject) // Check if this UI element is hit
                        {
                            OpenPortal(); // Open the portal
                        }
                    }
                }
            }
        }

        void OpenPortal()
        {
            GetComponentInParent<QuizPortalOpener>().OpenPortal();
        }

    }

}
