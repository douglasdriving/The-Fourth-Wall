using UnityEngine;

public class FallReset : MonoBehaviour
{
    [SerializeField] float resetHeight = -15;
    Vector3 spawnPos;
    Quaternion spawnRot;
    Rigidbody rb;

    private void Awake()
    {
        spawnPos = transform.position;
        spawnRot = transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ResetIfTooLow();
    }

    public void SetSpawnPoint(Vector3 postition)
    {
        spawnPos = postition;
        spawnRot = transform.rotation;
    }

    void ResetIfTooLow()
    {
        float height = transform.position.y;
        bool isBelowResetHeight = height < resetHeight;
        if (isBelowResetHeight)
        {
            ResetPlayer();
        }
    }

    void ResetPlayer()
    {
        transform.position = spawnPos;
        transform.rotation = spawnRot;
        rb.velocity = Vector3.zero;
    }
}
