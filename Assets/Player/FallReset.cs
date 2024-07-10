using UnityEngine;

public class FallReset : MonoBehaviour
{
    // [SerializeField] float resetHeight = -15;
    [SerializeField] float fallTimeForReset = 3;
    Vector3 spawnPos;
    Quaternion spawnRot;
    Rigidbody rb;

    //calc time falling
    //and die if you have been falling for too long
    float timeSpentFalling = 0;

    private void Awake()
    {
        spawnPos = transform.position;
        spawnRot = transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        bool isFalling = rb.velocity.y < -0.2;
        if (!isFalling)
        {
            timeSpentFalling = 0;
        }
        else
        {
            timeSpentFalling += Time.deltaTime;
        }

        if (timeSpentFalling > fallTimeForReset)
        {
            ResetPlayer();
            timeSpentFalling = 0;
        }


        // ResetIfTooLow();
    }

    public void SetSpawnPoint()
    {
        spawnPos = transform.position + Vector3.up * 0.02f;
        spawnRot = transform.rotation;
    }

    // void ResetIfTooLow()
    // {
    //     float height = transform.position.y;
    //     bool isBelowResetHeight = height < resetHeight;
    //     if (isBelowResetHeight)
    //     {
    //         ResetPlayer();
    //     }
    // }

    void ResetPlayer()
    {
        rb.velocity = Vector3.zero;
        transform.position = spawnPos;
        transform.rotation = spawnRot;
    }
}
