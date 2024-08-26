using UnityEngine;

namespace Helpers
{
    /// <summary>
    /// makes an object always face the camera
    /// </summary>
    public class CameraFacer : MonoBehaviour
    {
        Camera cam;

        void Awake()
        {
            cam = Camera.main;
        }

        void Update()
        {
            FaceCamera();
        }

        private void FaceCamera()
        {
            Vector3 directionVector = (transform.position - cam.transform.position).normalized;
            directionVector.y = 0;
            if (directionVector == Vector3.zero)
            {
                directionVector = Vector3.forward;
            }
            transform.forward = directionVector;
        }

    }
}

