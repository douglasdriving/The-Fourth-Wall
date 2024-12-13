using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuizPortal
{
    public class PortalQuizAnswerButton : MonoBehaviour
    {
        public bool isCorrectAnswer = false;

        public void Press()
        {
            if (isCorrectAnswer)
            {
                GetComponentInParent<PortalLock>().Open();
            }
            else
            {
                GetComponentInParent<PortalLock>().Close();
            }
        }
    }
}
