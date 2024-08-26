using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    /// <summary>
    /// reloads the scene if the player falls for too long
    /// </summary>
    public class FallReset : MonoBehaviour
    {
        [SerializeField] float fallTimeForReset = 3;
        Rigidbody rb;
        float timeSpentFalling = 0;

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
                ReloadScene();
                timeSpentFalling = 0;
            }
        }

        void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
