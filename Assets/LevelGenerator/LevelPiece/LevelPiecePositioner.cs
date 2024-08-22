using System.Collections;
using UnityEngine;

/// <summary>
/// moves a position to a set position, rotation, and scale over a given time.
/// </summary>
public class LevelPiecePositioner : MonoBehaviour
{

    [SerializeField] Vector3 defaultPieceScale = Vector3.one;
    [SerializeField] float timeToPosition = 1f;

    public void MoveToPosition(Vector3 targetPosition, Quaternion targetRotation)
    {
        StartCoroutine(MoveToPositionCoroutine(targetPosition, targetRotation));
    }

    IEnumerator MoveToPositionCoroutine(Vector3 targetPosition, Quaternion targetRotation)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 startScale = transform.localScale;

        float elapsedTime = 0;

        while (elapsedTime < timeToPosition)
        {

            float percentageOfTimePassed = elapsedTime / timeToPosition;

            transform.position = Vector3.Lerp(startPosition, targetPosition, percentageOfTimePassed);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, percentageOfTimePassed);
            transform.localScale = Vector3.Lerp(startScale, defaultPieceScale, percentageOfTimePassed);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;
        transform.localScale = defaultPieceScale;
    }
}
