using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace QuizPortal
{
    public class PortalQuizSetter : MonoBehaviour
    {
        [SerializeField] TMP_Text questionText;
        [SerializeField] GameObject firstAnswerButton;
        public void SetQuestion(string question, string[] answers, int correctAnswerIndex)
        {
            questionText.text = question;
            Transform buttonRow = firstAnswerButton.transform.parent;
            for (int i = 0; i < answers.Length; i++)
            {
                GameObject answerButton;
                if (i == 0)
                {
                    answerButton = firstAnswerButton;
                }
                else
                {
                    answerButton = Instantiate(firstAnswerButton, buttonRow);
                }
                answerButton.GetComponentInChildren<TMP_Text>().text = answers[i];
                bool isCorrect = i == correctAnswerIndex;
                answerButton.GetComponent<PortalQuizAnswerButton>().isCorrectAnswer = isCorrect;
            }
        }
    }
}

