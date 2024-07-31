using UnityEngine;

public class DirectionWall : MonoBehaviour
{
    void OnCollisionStay(Collision other)
    {
        if (other.transform.tag != "Player") return;

        Transform playerTransform = other.transform;
        Vector3 directionToWall = (transform.position - playerTransform.position).normalized;
        Vector3 playerForward = playerTransform.forward;

        directionToWall.y = 0;
        playerForward.y = 0;

        float angle = Vector3.SignedAngle(playerForward, directionToWall, Vector3.up);
        Direction playerCollisionDirection = DetermineDirection(angle);

        if (playerCollisionDirection == CurrentGameRules.rules.wallDirection)
        {
            Destroy(gameObject);
        }
    }

    private Direction DetermineDirection(float angle)
    {
        if (angle > -45 && angle <= 45)
        {
            return Direction.FORWARD;
        }
        else if (angle > 45 && angle <= 135)
        {
            return Direction.RIGHT;
        }
        else if (angle > -135 && angle <= -45)
        {
            return Direction.LEFT;
        }
        else
        {
            return Direction.BACKWARD;
        }
    }
}
