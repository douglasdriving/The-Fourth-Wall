using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuizPortal
{
    public class PortalLock : MonoBehaviour
    {
        [SerializeField] GameObject quizCanvas;
        [SerializeField] GameObject portal;
        [SerializeField] GameObject wrongAnswerCanvas;

        public void Open()
        {
            Destroy(quizCanvas);
            portal.SetActive(true);
        }

        public void Close()
        {
            Destroy(portal);
            Destroy(quizCanvas);
            wrongAnswerCanvas.SetActive(true);
        }
    }

}
