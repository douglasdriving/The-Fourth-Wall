using UnityEngine;

public class TransformRandomizer : MonoBehaviour
{
    [SerializeField] float minScale = 1;
    [SerializeField] float maxScale = 10;
    [SerializeField] Vector3 minRot = new Vector3(-30, -180, -30);
    [SerializeField] Vector3 maxRot = new Vector3(30, 180, 30);

    public void Randomize()
    {
        RandomizeScale();
        RandomizeRotation();
    }

    private void RandomizeScale()
    {
        float xScale = GetRandomValueBetween(minScale, maxScale);
        float yScale = GetRandomValueBetween(minScale, maxScale);
        float zScale = GetRandomValueBetween(minScale, maxScale);
        Vector3 scale = new Vector3(xScale, yScale, zScale);
        transform.localScale = scale;
    }

    private void RandomizeRotation()
    {
        float xRot = GetRandomValueBetween(minRot.x, maxRot.x);
        float yRot = GetRandomValueBetween(minRot.y, maxRot.y);
        float zRot = GetRandomValueBetween(minRot.z, maxRot.z);
        Quaternion rot = Quaternion.Euler(xRot, yRot, zRot);
        transform.rotation = rot;
    }

    private float GetRandomValueBetween(float min, float max)
    {
        float diff = max - min;
        float randomValue = min + Random.value * diff;
        return randomValue;
    }
}
