using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// allows a player to ride a rail between 2 points
/// </summary>
public class AndRailRider : MonoBehaviour
{
    //balance variables
    [SerializeField] float distanceFromEndPlayerWillBeDroppedAt = 0.5f;
    [SerializeField] float rideSpeed = 10;

    //player
    Transform player;
    FirstPersonController playerController;
    Rigidbody playerRb;

    // //rail positions
    Vector3 playerStartPos;
    Vector3 playerEndPos;

    //can start ride
    [SerializeField] GameObject ridePromptCanvas;
    bool playerIsInRideStartArea = false;
    bool isRiding = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<FirstPersonController>();
        playerRb = player.GetComponent<Rigidbody>();
        ridePromptCanvas.SetActive(false);
    }

    public void UpdatePlayerStartAndEnd(Vector3 _railStart, Vector3 _railEnd)
    {
        float halfPlayerHeight = player.localScale.y / 2;
        playerStartPos = _railStart + Vector3.up * halfPlayerHeight;
        playerEndPos = _railEnd + Vector3.up * halfPlayerHeight;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetRideStartEnabled(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetRideStartEnabled(false);
        }
    }

    void SetRideStartEnabled(bool shouldBeEnabled)
    {
        playerIsInRideStartArea = shouldBeEnabled;
        ridePromptCanvas.SetActive(shouldBeEnabled);
    }

    void Update()
    {
        if (playerIsInRideStartArea && Input.GetKeyDown(KeyCode.E))
        {
            StartRide();
        }
        else if (isRiding && Input.GetKeyUp(KeyCode.E))
        {
            StopRiding();
        }
        else if (isRiding)
        {
            UpdateRide();
        }
    }

    private void StopRiding()
    {
        isRiding = false;
        SetPlayerMovementAndGravityEnabled(true);
    }

    private void UpdateRide()
    {
        MovePlayerForwardAlongRail();
        bool hasReachedEnd = Vector3.Distance(player.position, playerEndPos) < 0.4f;
        if (hasReachedEnd)
        {
            EndRide();
        }
    }

    private void EndRide()
    {
        player.position = playerEndPos + Vector3.forward * distanceFromEndPlayerWillBeDroppedAt;
        SetPlayerMovementAndGravityEnabled(true);
        isRiding = false;
    }

    private void MovePlayerForwardAlongRail()
    {
        Vector3 rideDir = (playerEndPos - playerStartPos).normalized;
        player.position += rideDir * rideSpeed * Time.deltaTime;
    }

    private void StartRide()
    {
        ridePromptCanvas.SetActive(false);
        SetPlayerMovementAndGravityEnabled(false);
        isRiding = true;
        player.position = playerStartPos;
    }

    void SetPlayerMovementAndGravityEnabled(bool enabled)
    {
        if (playerController.canMove == enabled && playerRb.useGravity == enabled) return;
        playerController.canMove = enabled;
        playerRb.useGravity = enabled;
    }
}
