using System.Collections;
using System.Collections.Generic;
using QuizPortal;
using UnityEngine;

public class EndQuizSetter : MonoBehaviour
{
    [SerializeField] string question;
    [SerializeField] string[] answers;
    [SerializeField] int correctAnswerIndex;

    public void SetQuestion(GameObject portal)
    {
        portal.GetComponent<PortalQuizSetter>().SetQuestion(question, answers, correctAnswerIndex);
    }
}
