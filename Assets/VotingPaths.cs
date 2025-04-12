using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VotingPaths : MonoBehaviour
{

    [SerializeField] TMP_Text answerText1;
    [SerializeField] TMP_Text answerText2;
    [SerializeField] TMP_Text answerText3;

    void Start()
    {
        EndQuizSetter endQuizSetter = FindObjectOfType<EndQuizSetter>();
        if (endQuizSetter == null)
        {
            Debug.LogError("EndQuizSetter not found in the scene.");
            return;
        }
        string[] answers = endQuizSetter.GetAnswers();
        if (answers.Length != 3)
        {
            Debug.LogError("Expected 3 answers for voting paths, but got " + answers.Length);
            return;
        }
        answerText1.text = answers[0];
        answerText2.text = answers[1];
        answerText3.text = answers[2];

        // okay and then, we have to remember to also actually CAST the vote. hook up the portals to a backend thing that will actually do the voting.
    }
}
