using UnityEngine;

namespace Player
{
    /// <summary>
    /// respawns they player if they fall for too long
    /// </summary>
    public class FallReset : MonoBehaviour
    {
        [SerializeField] float fallTimeForReset = 3;
        Vector3 spawnPos;
        Quaternion spawnRot;
        Rigidbody rb;
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
        }

        public void SetSpawnPoint()
        {
            spawnPos = transform.position + Vector3.up * 0.02f;
            spawnRot = transform.rotation;
        }

        void ResetPlayer()
        {
            rb.velocity = Vector3.zero;
            transform.position = spawnPos;
            transform.rotation = spawnRot;
        }
    }

}
