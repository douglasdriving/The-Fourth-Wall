using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPositioner : MonoBehaviour
{

    [SerializeField] float planeDistanceFromVideoToPlayer = 15f;
    [SerializeField] float videoHeightAbovePlayer = 5f;

    public void PositionVideoInFrontOfPlayer()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        Vector3 playerForwardDir = player.forward;
        playerForwardDir.y = 0;
        playerForwardDir.Normalize();
        Vector3 videoPos = player.position + (playerForwardDir * planeDistanceFromVideoToPlayer) + (Vector3.up * videoHeightAbovePlayer);
        Vector3 videoForward = (player.position - videoPos).normalized;

        transform.position = videoPos;
        transform.forward = videoForward;
    }
}
