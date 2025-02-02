using UnityEngine;

public class TalkingHead : MonoBehaviour
{
    [SerializeField] float distanceToWalkway = 3f;
    [SerializeField] Vector3 defaultRotation = new Vector3(0, -22.5f, 0);
    [SerializeField] float randomRotationRange = 10f;
    public Transform mouth;

    public void MoveToEndOfWalkway(GameObject lastLevelPiece)
    {
        Vector3 finalWalkoffPoint = lastLevelPiece.GetComponent<LevelPiece.Positioner>().GetFinalWalkOffPoint();
        Vector3 targetPosition = finalWalkoffPoint + new Vector3(-1, 1, 0) * distanceToWalkway;
        transform.position = targetPosition;
        SetRandomRotation();
    }

    private void SetRandomRotation()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-randomRotationRange, randomRotationRange),
            Random.Range(-randomRotationRange, randomRotationRange),
            Random.Range(-randomRotationRange, randomRotationRange)
        );
        transform.rotation = Quaternion.Euler(defaultRotation + randomOffset);
    }
}
