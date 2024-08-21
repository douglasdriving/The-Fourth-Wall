using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMover : MonoBehaviour
{
    [SerializeField] float baseMoveDistance = 0.1f;
    [SerializeField] float timePerCycle = 1;
    bool isMovingUp = true;
    float initialYPos;
    Camera cam;

    void Awake()
    {
        initialYPos = transform.position.y;
        cam = Camera.main;
    }

    void Update()
    {
        float distanceToCamera = Vector3.Distance(transform.position, cam.transform.position);
        float scaledDistance = baseMoveDistance * distanceToCamera;
        float moveSpeed = scaledDistance / timePerCycle;
        float distanceToMoveThisFrame = moveSpeed * Time.deltaTime;

        if (isMovingUp)
        {
            float newYPos = transform.position.y + distanceToMoveThisFrame;
            transform.position = new Vector3(transform.position.x, newYPos, transform.position.z);
            if (newYPos >= initialYPos + scaledDistance)
            {
                isMovingUp = false;
            }
        }
        else
        {
            float newYPos = transform.position.y - distanceToMoveThisFrame;
            transform.position = new Vector3(transform.position.x, newYPos, transform.position.z);
            if (newYPos <= initialYPos)
            {
                isMovingUp = true;
            }
        }
    }
}
