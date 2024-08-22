using System.Collections;
using UnityEngine;

/// <summary>
/// moves a position to a set position, rotation, and scale over a given time.
/// </summary>
public class LevelPiecePositioner : MonoBehaviour
{

    public Vector3 targetPieceScale = Vector3.one;
    public Vector3 targetPosition { get; private set; }
    [SerializeField] float timeToPosition = 1f;
    Collider[] collidersToDisable;

    private void Awake()
    {
        collidersToDisable = GetComponentsInChildren<Collider>();
    }

    public void MoveToPosition(Vector3 targetPosition, Quaternion targetRotation)
    {
        StartCoroutine(MoveToPositionCoroutine(targetPosition, targetRotation));
    }

    IEnumerator MoveToPositionCoroutine(Vector3 targetPosition, Quaternion targetRotation)
    {

        this.targetPosition = targetPosition;

        SetCollidersEnabled(false);

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 startScale = transform.localScale;

        float elapsedTime = 0;

        while (elapsedTime < timeToPosition)
        {

            float percentageOfTimePassed = elapsedTime / timeToPosition;

            transform.position = Vector3.Lerp(startPosition, targetPosition, percentageOfTimePassed);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, percentageOfTimePassed);
            transform.localScale = Vector3.Lerp(startScale, targetPieceScale, percentageOfTimePassed);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;
        transform.localScale = targetPieceScale;

        SetCollidersEnabled(true);
    }

    void SetCollidersEnabled(bool enabled)
    {
        foreach (Collider collider in collidersToDisable)
        {
            collider.enabled = enabled;
        }
    }
}
