using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace QuizPortal
{
    public class QuizAnswerChecker : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetMouseButtonDown(0)) // When the user left-clicks
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
                    PortalQuizAnswerButton answerButton = result.gameObject.GetComponent<PortalQuizAnswerButton>();
                    if (answerButton) // Check if answer button was hit
                    {
                        answerButton.Press();
                    }
                }
            }
        }
    }
}
