using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// allows a player to ride a rail between 2 points
/// </summary>
public class Rail : MonoBehaviour
{
    //rail objects
    [SerializeField] Transform mesh;

    //balance variables
    [SerializeField] float totalRideTime = 0.5f;
    [SerializeField] float distanceFromEndPlayerWillBeDroppedAt = 0.5f;

    //player
    Transform player;
    FirstPersonController playerController;
    Rigidbody playerRb;

    //rail positions
    Vector3 startPoint;
    Vector3 endPoint;
    Vector3 playerStartPos;
    Vector3 playerEndPos;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<FirstPersonController>();
        playerRb = player.GetComponent<Rigidbody>();
    }

    public void SetStartAndEnd(Vector3 start, Vector3 end)
    {
        startPoint = start;
        endPoint = end;

        Vector3 direction = end - start;
        float length = direction.magnitude;
        transform.position = start;

        mesh.localScale = new Vector3(0.1f, 0.1f, length);
        mesh.LookAt(end);
        mesh.position += mesh.forward * length / 2;

        UpdatePlayerStartAndEnd();
    }

    private void UpdatePlayerStartAndEnd()
    {
        float halfPlayerHeight = player.localScale.y / 2;
        playerStartPos = startPoint + Vector3.up * halfPlayerHeight;
        playerEndPos = endPoint + Vector3.up * halfPlayerHeight;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Ride());
        }
    }

    IEnumerator Ride()
    {
        SetPlayerMovementAndGravityEnabled(false);
        yield return MoveAlongRail();
        SetPlayerMovementAndGravityEnabled(true);
    }

    IEnumerator MoveAlongRail()
    {
        player.position = playerStartPos;
        float timeOnRail = 0;
        while (timeOnRail < totalRideTime)
        {
            float progress = timeOnRail / totalRideTime;
            player.position = Vector3.Lerp(playerStartPos, playerEndPos, progress);
            timeOnRail += Time.deltaTime;
            yield return null;
        }
        player.position = playerEndPos + Vector3.forward * distanceFromEndPlayerWillBeDroppedAt;
    }

    void SetPlayerMovementAndGravityEnabled(bool enabled)
    {
        playerController.canMove = enabled;
        playerRb.useGravity = enabled;
    }
}
