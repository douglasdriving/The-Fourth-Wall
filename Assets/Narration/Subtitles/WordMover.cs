using TMPro;
using UnityEngine;

namespace Narration
{
    public class WordMover : MonoBehaviour
    {
        [SerializeField] TMP_Text word;
        [SerializeField] PointVisibilityScanner scanner;
        Camera cam;

        void Awake()
        {
            cam = Camera.main;
            UpdateWordPosition();
        }

        public void UpdateWordPosition()
        {
            Vector3? bestPoint = scanner.GetClosestVisiblePoint();
            if (bestPoint == null)
            {
                word.enabled = false;
            }
            else
            {
                ShowWordAtPoint((Vector3)bestPoint);
            }
        }

        private void ShowWordAtPoint(Vector3 point)
        {
            word.enabled = true;
            transform.position = point;
            RotateTowardsCamera();
        }

        private void RotateTowardsCamera()
        {
            Vector3 directionVector = (transform.position - cam.transform.position).normalized;
            transform.forward = directionVector;
        }
    }

}
