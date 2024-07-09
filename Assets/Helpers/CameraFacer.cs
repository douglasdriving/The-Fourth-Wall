using UnityEngine;

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

        // Project the direction vector onto the horizontal plane (y = 0)
        directionVector.y = 0;

        // If the direction vector is zero, default it to some direction
        if (directionVector == Vector3.zero)
        {
            directionVector = Vector3.forward;
        }

        transform.forward = directionVector;
    }

}
