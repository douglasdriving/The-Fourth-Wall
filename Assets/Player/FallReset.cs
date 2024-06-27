using UnityEngine;

public class FallReset : MonoBehaviour
{
    [SerializeField] float resetHeight = -5;
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

    private void OnTriggerEnter(Collider other)
    {
        bool isRespawnPoint = other.CompareTag("RespawnPoint");
        if (isRespawnPoint)
        {
            Transform spawnPoint = other.transform.root;
            spawnPos = spawnPoint.position;
            spawnRot = spawnPoint.rotation;
        }
    }
}
