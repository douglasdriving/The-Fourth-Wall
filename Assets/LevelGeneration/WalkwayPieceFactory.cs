using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;

namespace LevelGeneration
{
  /// <summary>
  /// instantiates walkway pieces into the world
  /// </summary>
  public class WalkwayPieceFactory : MonoBehaviour
  {
    [SerializeField] GameObject walkwayPiecePrefab;
    [SerializeField] LevelPieceMolds levelPieceMolds;

    /// okay so this was a little bit wierd, it just keeps going up and up <summary>
    /// instead we could create an "ideal height" for the piece, and then pick the point that is as close to that as possible but still not blocked by the other piece
    public GameObject InstantiatePieceAbovePos(Vector3 finalPos, Quaternion finalRot, Transform lastPiece, string pieceWord)
    {

      float idealHeightAboveFinalPos = 1f;
      float idealHeight = finalPos.y + idealHeightAboveFinalPos;
      Vector2 ideal2DPos = new Vector2(finalPos.x, idealHeight);
      float minDiffFromPreviousPos = 1f;
      Vector2 previousPiece2DPos = new Vector2(lastPiece.position.x, lastPiece.position.y);
      float distanceFromPrevious = Vector2.Distance(ideal2DPos, previousPiece2DPos);
      bool isBlockedByPrevious = distanceFromPrevious < minDiffFromPreviousPos;
      Vector2 spawnPos2D = ideal2DPos;
      if (isBlockedByPrevious)
      {
        Vector2 directionFromPreviousPosToIdealPos2D = (ideal2DPos - previousPiece2DPos).normalized;
        spawnPos2D = previousPiece2DPos + directionFromPreviousPosToIdealPos2D * minDiffFromPreviousPos;
        Debug.Log("blocked by previous, moving by vector from last 2d pos " + directionFromPreviousPosToIdealPos2D * minDiffFromPreviousPos);
      }
      Vector3 spawnPos = new Vector3(spawnPos2D.x, spawnPos2D.y, finalPos.z);

      // float heightAbovePrevious = 0.3f; //magic?
      // Vector3 spawnPos = finalPos;
      // spawnPos.y = lastPiece.position.y + heightAbovePrevious;

      GameObject piece = Instantiate(walkwayPiecePrefab, spawnPos, Quaternion.identity);
      piece.transform.up = Vector3.back;
      piece.GetComponentInChildren<TMP_Text>().text = pieceWord;
      piece.GetComponent<LevelPiecePositioner>().MoveWithSimpleAnimation(finalPos, finalRot);
      return piece;
    }

    public GameObject InstantiateInFrontOfCamera(string pieceWord)
    {
      GameObject piece = levelPieceMolds.CopyNextMold();
      piece.GetComponentInChildren<TMP_Text>().text = pieceWord;
      return piece;
    }

    public GameObject InstantiateAtFinalPosition(Vector3 pos, Quaternion rot, string pieceWord)
    {
      GameObject piece = GameObject.Instantiate(walkwayPiecePrefab);
      piece.GetComponentInChildren<LevelPiecePositioner>().SetPosition(pos, rot);
      piece.GetComponentInChildren<TMP_Text>().text = pieceWord;
      return piece;
    }
  }
}
