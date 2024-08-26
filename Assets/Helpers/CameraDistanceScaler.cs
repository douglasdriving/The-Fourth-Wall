using UnityEngine;

namespace Helpers
{
    /// <summary>
    /// scales an object based on the distance to the camera
    /// </summary>
    public class CameraDistanceScaler : MonoBehaviour
    {
        [SerializeField] float baseScaleFactor = 1;
        Camera cam;
        Vector3 baseScaleVector;


        void Awake()
        {
            baseScaleVector = transform.localScale * baseScaleFactor;
            cam = Camera.main;
        }

        void Update()
        {
            ScaleWithDistanceToCamera();
        }

        private void ScaleWithDistanceToCamera()
        {
            float distance = Vector3.Distance(transform.position, cam.transform.position);
            transform.localScale = baseScaleVector * distance;
        }
    }
}

