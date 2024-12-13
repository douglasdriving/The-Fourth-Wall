using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuizPortal
{
    public class QuizPortalOpener : MonoBehaviour
    {
        [SerializeField] GameObject quizCanvas;
        [SerializeField] GameObject portal;

        public void OpenPortal()
        {
            Destroy(quizCanvas);
            portal.SetActive(true);
        }
    }

}
