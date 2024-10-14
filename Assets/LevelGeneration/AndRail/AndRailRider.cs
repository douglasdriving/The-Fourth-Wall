using System;
using System.Collections;
using System.Collections.Generic;
using LevelGeneration;
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
        SetPlayerControllEnabled(true);
    }

    private void UpdateRide()
    {
        MovePlayerForwardAlongRail();
        bool hasReachedEnd = player.position.z >= walkoffPoint.position.z;
        if (hasReachedEnd)
        {
            EndRide();
        }
    }

    private void EndRide()
    {
        player.position = playerEndPos + Vector3.forward * distanceFromEndPlayerWillBeDroppedAt;
        SetPlayerControllEnabled(true);
        isRiding = false;
        Destroy(this);
    }

    private void MovePlayerForwardAlongRail()
    {
        Vector3 rideDir = (playerEndPos - playerStartPos).normalized;
        player.position += rideDir * rideSpeed * Time.deltaTime;
    }

    private void StartRide()
    {
        ridePromptCanvas.SetActive(false);
        SetPlayerControllEnabled(false);
        isRiding = true;
        player.position = playerStartPos;
    }

    void SetPlayerControllEnabled(bool enabled)
    {
        if (playerController.canMove == enabled && playerRb.useGravity == enabled) return;
        playerController.canMove = enabled;
        playerRb.useGravity = enabled;
        playerCollider.enabled = enabled;
    }
}
