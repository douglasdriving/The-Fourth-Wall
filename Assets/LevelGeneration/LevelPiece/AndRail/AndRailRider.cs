using UnityEngine;

namespace LevelPiece
{
    /// <summary>
    /// allows a player to ride a rail between 2 points
    /// </summary>
    public class AndRailRider : MonoBehaviour
    {
        //balance variables
        [SerializeField] float distanceFromEndPlayerWillBeDroppedAt = 0.5f;
        [SerializeField] float rideSpeed = 10;
        [SerializeField] float rideHeight = 1;

        //player
        Transform player;
        FirstPersonController playerController;
        Rigidbody playerRb;
        Collider playerCollider;

        //rail positions
        Vector3 playerStartPos;
        Vector3 playerEndPos;
        [SerializeField] Transform walkoffPoint;

        //can start ride
        [SerializeField] GameObject ridePromptCanvas;
        bool playerIsInRideStartArea = false;
        bool isRiding = false;

        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            playerController = player.GetComponent<FirstPersonController>();
            playerRb = player.GetComponent<Rigidbody>();
            playerCollider = player.GetComponent<Collider>();
            ridePromptCanvas.SetActive(false);
        }

        public void UpdatePlayerStartAndEnd(Vector3 _railStart, Vector3 _railEnd)
        {
            playerStartPos = _railStart + Vector3.up * rideHeight;
            playerEndPos = _railEnd + Vector3.up * rideHeight;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player entered ride start area");
                ridePromptCanvas.SetActive(true);
                playerIsInRideStartArea = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ridePromptCanvas.SetActive(false);
                playerIsInRideStartArea = false;
            }
        }

        void Update()
        {

            if (isRiding)
            {
                UpdateRide();
            }

            if (!isRiding && playerIsInRideStartArea && Input.GetKeyDown(KeyCode.E))
            {
                StartRide();
            }

        }

        private void StartRide()
        {
            ridePromptCanvas.SetActive(false);
            SetPlayerControllEnabled(false);
            isRiding = true;
            player.position = playerStartPos;
        }

        private void UpdateRide()
        {
            Vector3 rideDir = (playerEndPos - playerStartPos).normalized;
            playerRb.MovePosition(playerRb.position + rideDir * rideSpeed * Time.deltaTime);

            bool hasReachedEnd = player.position.z >= walkoffPoint.position.z;
            if (hasReachedEnd)
            {
                EndRide();
            }
        }

        private void EndRide()
        {
            player.position = playerEndPos + Vector3.forward * distanceFromEndPlayerWillBeDroppedAt;
            playerIsInRideStartArea = false;
            ridePromptCanvas.SetActive(false);
            SetPlayerControllEnabled(true);
            isRiding = false;
        }

        void SetPlayerControllEnabled(bool enabled)
        {
            if (playerController.canMove == enabled && playerRb.useGravity == enabled) return;
            playerController.canMove = enabled;
            playerRb.useGravity = enabled;
            playerCollider.enabled = enabled;
        }
    }
}
