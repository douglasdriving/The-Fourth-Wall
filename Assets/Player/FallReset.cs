using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    /// <summary>
    /// reloads the scene if the player falls for too long
    /// </summary>
    public class FallReset : MonoBehaviour
    {

        enum DeathAction
        {
            ResetLevel,
            RespawnOnLastPiece
        }


        [SerializeField] float fallTimeForReset = 3;
        [SerializeField] DeathAction deathAction = DeathAction.ResetLevel;
        Rigidbody rb;
        float timeSpentFalling = 0;
        Transform lastLevelPieceTouched;

        private void Awake()
        {
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
                PlayerDeath();
                timeSpentFalling = 0;
            }
        }

        void PlayerDeath()
        {
            if (deathAction == DeathAction.ResetLevel)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (deathAction == DeathAction.RespawnOnLastPiece)
            {
                TeleportToLastPiece();
            }
        }

        void TeleportToLastPiece()
        {
            transform.position = lastLevelPieceTouched.transform.position + Vector3.up;
            transform.rotation = lastLevelPieceTouched.transform.rotation;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Platform"))
            {
                lastLevelPieceTouched = collision.transform;
            }
        }
    }

}
