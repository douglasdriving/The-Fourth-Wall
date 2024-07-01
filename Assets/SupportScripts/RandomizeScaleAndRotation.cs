using UnityEngine;

public class RandomizeScaleAndRotation : MonoBehaviour
{
    [SerializeField] float minScale = 1;
    [SerializeField] float maxScale = 10;

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
        float xRot = GetRandomValueBetween(-40, 40); //replace magic numbers
        float yRot = GetRandomValueBetween(0, 360);
        float zRot = GetRandomValueBetween(-40, 40);
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
