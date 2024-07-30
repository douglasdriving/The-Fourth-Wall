using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(WalkwayGenerator))]
public class LevelGenerator : MonoBehaviour
{

  [SerializeField] int piecesPerPlatform = 12;
  [SerializeField] int minPlatformPieces = 4;

  public static LevelPieceType pieceTypeBeingGenerated = LevelPieceType.WALKWAY;
  WalkwayGenerator walkwayGenerator;
  int piecesOnCurrentPlatform;
  List<LevelPiece> piecesBeingGenerated = null;
  public List<GameObject> levelPiecesSpawned = new();
  void Awake()
  {
    piecesOnCurrentPlatform = piecesPerPlatform;
    if (levelPiecesSpawned.Count <= 0) Debug.LogError("no spawned pieces in list. please assign at least one level piece to start generating from");
    walkwayGenerator = GetComponent<WalkwayGenerator>();
  }

  public void DestroyAllPiecesAboveIndex(int indexToDestroyAbove)
  {
    for (int i = levelPiecesSpawned.Count - 1; i > indexToDestroyAbove; i--)
    {
      GameObject piece = levelPiecesSpawned[i];
      levelPiecesSpawned.RemoveAt(i); // Remove from list
      Destroy(piece); // Destroy the GameObject
    }
  }


  public void SetPlatformingPath(Vector3 pathStart, Vector3 pathEnd, int numberOfPiecesToAdd)
  {
    piecesBeingGenerated = PathGenerator.GetPathBetweenPoints(pathStart, pathEnd, numberOfPiecesToAdd);
  }

  public GameObject SpawnCustomPlatform(GameObject platformPrefab, float gapFromWalkwayEndToPlatformPivot)
  {
    Vector3 endPointOfLastPiece = GetEndPointOfPiece(levelPiecesSpawned.Last());
    Vector3 platformPos = endPointOfLastPiece + Vector3.forward * gapFromWalkwayEndToPlatformPivot;
    GameObject platform = Instantiate(platformPrefab, platformPos, Quaternion.identity);
    return platform;
  }

  public void SpawnNextPiece(string pieceWord, int piecesLeftToSpawnInSection)
  {
    bool piecesInList = piecesBeingGenerated != null && piecesBeingGenerated.Count > 0;
    GameObject piece = null;

    if (piecesInList)
    {
      piece = SpawnNextPieceFromList(pieceWord);
    }
    else if (pieceTypeBeingGenerated == LevelPieceType.WALKWAY)
    {
      piece = SpawnWalkwayPiece(pieceWord);
    }
    else if (pieceTypeBeingGenerated == LevelPieceType.PLATFORM)
    {
      piece = SpawnPlatformPiece(pieceWord, piecesLeftToSpawnInSection);
    }

    levelPiecesSpawned.Add(piece);

  }

  private GameObject SpawnPlatformPiece(string pieceWord, int piecesLeftToSpawnInSection)
  {
    Transform lastLevelPieceAdded = levelPiecesSpawned.Last().transform;
    bool isReachingEnd = piecesLeftToSpawnInSection < minPlatformPieces;

    if (piecesOnCurrentPlatform >= piecesPerPlatform && !isReachingEnd)
    {
      GameObject piece = walkwayGenerator.GeneratePieceWithGap(lastLevelPieceAdded.transform, pieceWord);
      piecesOnCurrentPlatform = 1;
      return piece;
    }
    else
    {
      GameObject piece = walkwayGenerator.GenerateNextPiece(lastLevelPieceAdded.transform, pieceWord, true);
      piecesOnCurrentPlatform++;
      return piece;
    }
  }

  private GameObject SpawnWalkwayPiece(string pieceWord)
  {
    GameObject piece = walkwayGenerator.GenerateNextPiece(levelPiecesSpawned.Last().transform, pieceWord, false);
    return piece;
  }

  private GameObject SpawnNextPieceFromList(string pieceWord)
  {
    LevelPiece nextPiece = piecesBeingGenerated[0];
    GameObject piece = walkwayGenerator.GenerateAtExactSpot(nextPiece, pieceWord);
    piecesBeingGenerated.RemoveAt(0);
    return piece;
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