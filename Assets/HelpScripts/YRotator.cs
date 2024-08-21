using UnityEngine;

public class YRotator : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 20;
    bool isRotating = true;

    void Update()
    {
        if (!isRotating) return;
        Vector3 oldRotation = transform.rotation.eulerAngles;
        Vector3 newRotation = oldRotation;
        newRotation.y += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(newRotation);
    }

    public void StopRotation()
    {
        isRotating = false;
    }
}
