using System.Collections;
using System.Collections.Generic;
using Player;
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
    [SerializeField] float spawnHeight = 1.5f;
    [SerializeField] float spawnSpreadMultiplier = 0.3f;
    SceneRules rules;
    Transform playerCam;
    Transform talkingHeadMouth;

    private void Awake()
    {
      rules = FindObjectOfType<SceneRules>();
      playerCam = Camera.main.transform;
      TalkingHead talkingHead = FindObjectOfType<TalkingHead>();
      if (talkingHead) talkingHeadMouth = talkingHead.mouth;
    }

    public GameObject SpawnFromTalkingHead(Vector3 finalPos, Quaternion finalRot, string word)
    {
      GameObject piece = Instantiate(walkwayPiecePrefab, talkingHeadMouth.position, Quaternion.identity);
      AdjustPieceRotation(piece);
      piece.GetComponent<LevelPiece.WordSetter>().SetWord(word);
      piece.GetComponent<LevelPiece.Positioner>().MoveFromTalkingHead(GetPosAboveTarget(finalPos), finalPos, finalRot);
      return piece;
    }

    public GameObject SpawnAboveTargetAndMoveIntoPlace(Vector3 finalPos, Quaternion finalRot, string word)
    {
      Vector3 spawnPos = GetPosAboveTarget(finalPos);
      GameObject piece = Instantiate(walkwayPiecePrefab, spawnPos, Quaternion.identity);
      AdjustPieceRotation(piece);
      piece.GetComponent<LevelPiece.WordSetter>().SetWord(word);
      piece.GetComponent<LevelPiece.Positioner>().MoveWithSimpleAnimation(finalPos, finalRot);
      return piece;
    }

    private void AdjustPieceRotation(GameObject piece)
    {
      if (rules && rules.pieceSpawnSpread)
      {
        piece.transform.LookAt(playerCam.position);
        piece.transform.Rotate(-90, 0, 180);
      }
      else
      {
        piece.transform.up = Vector3.back;
      }
    }

    private Vector3 GetPosAboveTarget(Vector3 finalPos)
    {
      Vector3 spawnPos = finalPos + Vector3.up * spawnHeight;
      if (rules && rules.pieceSpawnSpread)
      {
        float distanceToPlayer = Vector3.Distance(playerCam.position, spawnPos);
        float maxUpShift = distanceToPlayer * spawnSpreadMultiplier;
        spawnPos.y += Random.Range(0, maxUpShift);
        float maxSideShift = distanceToPlayer * spawnSpreadMultiplier;
        spawnPos.x += Random.Range(-maxSideShift, maxSideShift);
      }
      return spawnPos;
    }

    public GameObject SpawnInFrontOfCameraAnMoveIntoPlace(string pieceWord, Vector3 targetPos, Quaternion targetRot)
    {
      GameObject piece = levelPieceMolds.CopyNextMold();
      piece.GetComponent<LevelPiece.WordSetter>().SetWord(pieceWord);
      piece.GetComponent<LevelPiece.Positioner>().MoveWithAnimation(targetPos, targetRot);
      return piece;
    }

    public GameObject SpawnAtFinalPosition(Vector3 pos, Quaternion rot, string pieceWord)
    {
      GameObject piece = Instantiate(walkwayPiecePrefab);
      piece.GetComponent<LevelPiece.WordSetter>().SetWord(pieceWord);
      piece.GetComponentInChildren<LevelPiece.Positioner>().SetPosition(pos, rot);
      return piece;
    }

    public float GetPieceHeight()
    {
      return walkwayPiecePrefab.transform.localScale.y;
    }
  }
}
