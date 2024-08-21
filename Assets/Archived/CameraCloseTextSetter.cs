using TMPro;
using UnityEngine;

public class CameraCloseTextSetter : MonoBehaviour
{
    [SerializeField] TMP_Text textElement;
    [SerializeField] string text;
    [SerializeField] float distanceToSetAt = 2;

    Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        float distanceToCamera = Vector3.Distance(transform.position, cam.transform.position);
        if (distanceToCamera < distanceToSetAt)
        {
            textElement.text = text;
            enabled = false;
        }
    }
}
