using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(PlatformGenerator))]
[RequireComponent(typeof(WalkwayGenerator))]
public class LevelGenerator : MonoBehaviour
{

  [SerializeField] int piecesPerPlatform = 12;
  [SerializeField] int minPlatformPieces = 4;

  public static LevelPieceType pieceTypeBeingGenerated = LevelPieceType.WALKWAY;
  public GameObject lastLevelPieceAdded;
  WalkwayGenerator walkwayGenerator;
  // PlatformGenerator platformGenerator;
  int piecesOnCurrentPlatform;
  List<LevelPiece> piecesBeingGenerated = null;

  void Awake()
  {
    piecesOnCurrentPlatform = piecesPerPlatform;
    if (!lastLevelPieceAdded) Debug.LogError("please assign a last level piece to start generating from");
    walkwayGenerator = GetComponent<WalkwayGenerator>();
    // platformGenerator = GetComponent<PlatformGenerator>();
  }


  public void SetPlatformingPath(Vector3 pathStart, Vector3 pathEnd, int numberOfPiecesToAdd)
  {
    piecesBeingGenerated = PathGenerator.GetPathBetweenPoints(pathStart, pathEnd, numberOfPiecesToAdd);
    Debug.Log("set pieces being generated. count: " + piecesBeingGenerated.Count);
  }

  public GameObject SpawnCustomPlatform(GameObject platformPrefab, float gapFromWalkwayEndToPlatformPivot)
  {
    Vector3 endPointOfLastPiece = GetEndPointOfPiece(lastLevelPieceAdded);
    // GameObject platformInstance = platformGenerator.GenerateCustomPlatform(endPointOfLastPiece, platformPrefab);
    // lastLevelPieceAdded = platformInstance;
    Vector3 platformPos = endPointOfLastPiece + Vector3.forward * gapFromWalkwayEndToPlatformPivot;
    GameObject platform = Instantiate(platformPrefab, platformPos, Quaternion.identity);
    return platform;
  }

  public void SpawnNextPiece(string pieceWord, int piecesLeftToSpawnInSection)
  {
    bool piecesInList = piecesBeingGenerated != null && piecesBeingGenerated.Count > 0;

    if (piecesInList)
    {
      SpawnNextPieceFromList(pieceWord);
    }
    else if (pieceTypeBeingGenerated == LevelPieceType.WALKWAY)
    {
      SpawnWalkwayPiece(pieceWord);
    }
    else if (pieceTypeBeingGenerated == LevelPieceType.PLATFORM)
    {
      SpawnPlatformPiece(pieceWord, piecesLeftToSpawnInSection);
    }

  }

  private void SpawnPlatformPiece(string pieceWord, int piecesLeftToSpawnInSection)
  {
    // lastLevelPieceAdded = platformGenerator.GenerateNextPlatform(endPointOfLastPiece);
    bool isReachingEnd = piecesLeftToSpawnInSection < minPlatformPieces;

    if (piecesOnCurrentPlatform >= piecesPerPlatform && !isReachingEnd)
    {
      lastLevelPieceAdded = walkwayGenerator.GeneratePieceWithGap(lastLevelPieceAdded.transform, pieceWord);
      piecesOnCurrentPlatform = 1;
    }
    else
    {
      lastLevelPieceAdded = walkwayGenerator.GenerateNextPiece(lastLevelPieceAdded.transform, pieceWord, true);
      piecesOnCurrentPlatform++;
    }
  }

  private void SpawnWalkwayPiece(string pieceWord)
  {
    lastLevelPieceAdded = walkwayGenerator.GenerateNextPiece(lastLevelPieceAdded.transform, pieceWord, false);
  }

  private void SpawnNextPieceFromList(string pieceWord)
  {
    LevelPiece nextPiece = piecesBeingGenerated[0];
    walkwayGenerator.GenerateAtExactSpot(nextPiece, pieceWord);
    piecesBeingGenerated.RemoveAt(0);
  }

  public Vector3 GetEndPointOfPiece(GameObject piece)
  {
    MeshFilter meshFilter = piece.GetComponentInChildren<MeshFilter>();
    Mesh mesh = meshFilter.mesh;
    Vector3[] vertices = mesh.vertices;
    Vector3 mostUpForwardPoint = Vector3.negativeInfinity;

    foreach (Vector3 vertex in vertices)
    {
      Vector3 worldVertex = meshFilter.transform.TransformPoint(vertex);
      float forwardUpValue = worldVertex.y + worldVertex.z;
      float mostUpForwardValue = mostUpForwardPoint.y + mostUpForwardPoint.z;
      if (forwardUpValue > mostUpForwardValue)
      {
        mostUpForwardPoint = worldVertex;
      }
    }
    return mostUpForwardPoint;
  }


}