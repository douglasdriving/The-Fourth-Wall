using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ThoughtLevitator : MonoBehaviour
{
    [Header("Levitation Settings")]
    [Tooltip("How fast the player will float upward")]
    public float levitationSpeed = 1.5f;

    private FirstPersonController playerController;
    private Rigidbody playerRigidbody;
    private bool isLevitating = false;

    private Transform playerTransform;

    void Start()
    {
        // Make sure the collider is a trigger
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void Update()
    {
        if (isLevitating && playerRigidbody != null)
        {
            // Apply upward force to make player float continuously
            playerRigidbody.velocity = new Vector3(0, levitationSpeed, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Get player controller and rigidbody
            playerController = other.GetComponent<FirstPersonController>();
            playerRigidbody = other.GetComponent<Rigidbody>();
            playerTransform = other.transform;

            if (playerController != null && playerRigidbody != null)
            {
                StartLevitation();
                SetInvisible();
                ThoughtWriter thoughtWriter = FindObjectOfType<ThoughtWriter>();
                if (thoughtWriter != null)
                {
                    thoughtWriter.EnableThoughtWriting();
                }
                else
                {
                    Debug.LogError("ThoughtLevitator: No ThoughtWriter found in the scene.");
                }
            }
        }
    }

    private void SetInvisible()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }
    }

    private void StartLevitation()
    {


        // Disable player movement but keep camera rotation
        playerController.canMove = false;
        playerController.cameraCanMove = true;

        // Freeze horizontal movement but allow vertical movement
        playerRigidbody.constraints = RigidbodyConstraints.FreezePositionX |
                                     RigidbodyConstraints.FreezePositionZ |
                                     RigidbodyConstraints.FreezeRotationX |
                                     RigidbodyConstraints.FreezeRotationY |
                                     RigidbodyConstraints.FreezeRotationZ;

        // Disable gravity
        playerRigidbody.useGravity = false;

        // Set levitating flag
        isLevitating = true;
    }

    // Optional: Add a method to stop levitation if needed
    public void StopLevitation()
    {
        if (isLevitating && playerController != null && playerRigidbody != null)
        {
            // Re-enable player movement
            playerController.canMove = true;

            // Reset rigidbody constraints
            playerRigidbody.constraints = RigidbodyConstraints.FreezeRotationX |
                                         RigidbodyConstraints.FreezeRotationY |
                                         RigidbodyConstraints.FreezeRotationZ;

            // Re-enable gravity
            playerRigidbody.useGravity = true;

            // Reset levitating flag
            isLevitating = false;
        }
    }
}
