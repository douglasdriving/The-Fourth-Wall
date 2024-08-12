using UnityEngine;

public class LookAwayChecker : MonoBehaviour
{
  [SerializeField] Camera playerCamera;

  public bool IsPlayerLookingAway(Transform targetTransform, float desiredLookPercentage)
  {
    // Get the direction the player is looking
    Vector3 playerForward = playerCamera.transform.forward;

    // Get the direction from the player to the target
    Vector3 directionToTarget = (targetTransform.position - playerCamera.transform.position).normalized;

    // Calculate the angle between the player's forward direction and the direction to the target
    float angle = Vector3.Angle(playerForward, directionToTarget);

    // Determine the maximum angle allowed based on the desired look percentage
    // If desiredLookPercentage is 1, maxAllowedAngle will be 0 (must be looking exactly at it)
    // If desiredLookPercentage is 0, maxAllowedAngle will be 180 (looking directly away)
    float maxAllowedAngle = 180 * (1 - desiredLookPercentage);

    // If the angle is greater than the maxAllowedAngle, the player is looking away
    return angle > maxAllowedAngle;
  }
}