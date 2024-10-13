using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelGeneration;

/// <summary>
/// moves, scales, and positions the rail
/// </summary>
[RequireComponent(typeof(LevelPiecePositioner))]
public class AndRailPositioner : MonoBehaviour
{

    // rail objects
    [SerializeField] Transform mesh;

    //positioner
    LevelPiecePositioner positioner;

    //rail positions
    Vector3 finalRailStartPoint;
    float railLength;
    Quaternion finalRotation;

    void Awake()
    {
        positioner = GetComponent<LevelPiecePositioner>();
    }

    public void SetupRailPositioning(Vector3 start, Vector3 end)
    {
        SetFinalRailPositions(start, end);
        TeleportToStartPos();
        MoveRailWithAnimation();
    }

    private void SetFinalRailPositions(Vector3 finalRailStart, Vector3 finalRailEnd)
    {
        finalRailStartPoint = finalRailStart;

        Vector3 direction = finalRailEnd - finalRailStart;
        railLength = direction.magnitude;
        transform.position = finalRailStart;

        mesh.localScale = new Vector3(0.1f, 0.1f, railLength);
        transform.LookAt(finalRailEnd);
        finalRotation = transform.rotation;
        mesh.position = transform.position + mesh.forward * railLength / 2;

        GetComponent<AndRailRider>().UpdatePlayerStartAndEnd(finalRailStart, finalRailEnd);
    }

    private void TeleportToStartPos()
    {
        //start pos is above walkway
        transform.position = finalRailStartPoint + Vector3.up * 1.5f; //magic number that we need to grab from somewhere...
        transform.position += Vector3.right * railLength / 2;

        //with the word rotated towards the player
        transform.rotation = Quaternion.Euler(0, 270, 90);
    }

    private void MoveRailWithAnimation()
    {
        positioner.MoveWithSimpleAnimation(finalRailStartPoint, finalRotation);
    }

}
