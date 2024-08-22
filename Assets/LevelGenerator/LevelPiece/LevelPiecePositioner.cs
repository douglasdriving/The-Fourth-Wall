using System.Collections;
using UnityEngine;

/// problems

/// <summary>
/// moves a position to a set position, rotation, and scale over a given time.
/// </summary>
public class LevelPiecePositioner : MonoBehaviour
{

    public Vector3 targetPieceScale = Vector3.one;
    public Vector3 targetPosition { get; private set; }
    public Quaternion targetRotation { get; private set; }

    [SerializeField] float timeToPosition = 1f;
    [SerializeField] float hoverTime = 0.5f;

    Collider[] collidersToDisable;

    private void Awake()
    {
        collidersToDisable = GetComponentsInChildren<Collider>();
    }

    public void MoveToPosition(Vector3 targetPosition, Quaternion targetRotation, bool animate)
    {
        this.targetPosition = targetPosition;
        this.targetRotation = targetRotation;

        if (animate)
        {
            StartCoroutine(MoveAnimation());
        }
        else
        {
            SetFinalTransform(targetPosition, targetRotation);
        }
    }

    IEnumerator MoveAnimation()
    {
        SetCollidersEnabled(false);
        yield return HoverInFrontOfCamera(hoverTime);

        //someone way left of the target pos first
        // float multiplier = Random.value > 0.5f ? 1 : -1;
        // Vector3 posLeftOfTarget = targetPosition + Vector3.left * 2 * multiplier;

        Vector3 posWithSameZAsTarget = new Vector3(transform.position.x, transform.position.y, targetPosition.z);

        yield return MoveToPosition(posWithSameZAsTarget, 0.8f);
        yield return new WaitForSeconds(0.1f);

        yield return ScaleToScale(targetPieceScale, 0.2f);
        yield return new WaitForSeconds(0.1f);
        yield return RotateToRotation(targetRotation, 0.2f);
        yield return new WaitForSeconds(0.1f);
        yield return MoveToPosition(targetPosition, 0.3f);

        SetCollidersEnabled(true);
    }

    IEnumerator HoverInFrontOfCamera(float hoverTime)
    {
        transform.parent = Camera.main.transform; //might get fucked by functions above
        yield return new WaitForSeconds(hoverTime);
        transform.parent = null; //SOULD BE WALKWAY
    }

    IEnumerator MoveToPosition(Vector3 targetPos, float moveTime)
    {

        // Vector3 startPosition = transform.position;
        // float elapsedTime = 0;
        // while (elapsedTime < moveTime)
        // {
        //     float percentageOfTimePassed = elapsedTime / moveTime;
        //     transform.position = Vector3.Lerp(startPosition, targetPos, percentageOfTimePassed);
        //     elapsedTime += Time.deltaTime;
        //     yield return null;
        // }

        float distanceToTarget = Vector3.Distance(targetPos, transform.position);
        Vector3 directionToTarget = (targetPos - transform.position).normalized;
        float moveSpeed = 15f;

        while (distanceToTarget > 0.1f)
        {
            transform.position += directionToTarget * moveSpeed * Time.deltaTime;
            distanceToTarget = Vector3.Distance(targetPos, transform.position);
            yield return null;
        }

        transform.position = targetPos;
    }

    IEnumerator RotateToRotation(Quaternion targetRot, float rotationTime)
    {
        float elapsedTime = 0;
        Quaternion startRotation = transform.rotation;
        while (elapsedTime < rotationTime)
        {
            float percentageOfTimePassed = elapsedTime / rotationTime;
            transform.rotation = Quaternion.Lerp(startRotation, targetRot, percentageOfTimePassed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRot;
    }

    IEnumerator ScaleToScale(Vector3 targetScale, float scaleTime)
    {
        float elapsedTime = 0;
        Vector3 startScale = transform.localScale;
        while (elapsedTime < scaleTime)
        {
            float percentageOfTimePassed = elapsedTime / scaleTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, percentageOfTimePassed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
    }

    private void SetFinalTransform(Vector3 targetPosition, Quaternion targetRotation)
    {
        transform.position = targetPosition;
        transform.rotation = targetRotation;
        transform.localScale = targetPieceScale;
    }

    void SetCollidersEnabled(bool enabled)
    {
        foreach (Collider collider in collidersToDisable)
        {
            collider.enabled = enabled;
        }
    }
}
