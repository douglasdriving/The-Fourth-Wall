using UnityEngine;

public class TransformRandomizer : MonoBehaviour
{
    [SerializeField] Vector3 minScale = Vector3.one;
    [SerializeField] Vector3 maxScale = Vector3.one * 10;
    [SerializeField] Vector3 minRot = new Vector3(-30, -180, -30);
    [SerializeField] Vector3 maxRot = new Vector3(30, 180, 30);

    public void Randomize()
    {
        RandomizeScale();
        RandomizeRotation();
    }

    public void Reset()
    {
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;
    }

    private void RandomizeScale()
    {
        float xScale = GetRandomValueBetween(minScale.x, maxScale.x);
        float yScale = GetRandomValueBetween(minScale.y, maxScale.y);
        float zScale = GetRandomValueBetween(minScale.z, maxScale.z);
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
