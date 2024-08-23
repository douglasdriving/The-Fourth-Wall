using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(WalkwayGenerator))]
public class LevelGenerator : MonoBehaviour
{

  [SerializeField] int piecesPerPlatform = 12;
  [SerializeField] int minPlatformPieces = 4;

  [SerializeField] Transform ground;
  [SerializeField] Transform platformsParent;
  [SerializeField] GameObject platformsPrefab;

  public static LevelPieceType pieceTypeBeingGenerated = LevelPieceType.WALKWAY;
  WalkwayGenerator walkwayGenerator;

  List<LevelPiece> piecesBeingGenerated = null;
  public List<GameObject> levelPiecesSpawned = new();
  // Platform lastPlatformStarted;
  // int piecesOnCurrentPlatform;
  public delegate void LevelPieceSpawned(GameObject piece);
  public static event LevelPieceSpawned OnLevelPieceSpawned;

  void Awake()
  {
    // piecesOnCurrentPlatform = piecesPerPlatform;
    if (levelPiecesSpawned.Count <= 0) Debug.LogError("no spawned pieces in list. please assign at least one level piece to start generating from");
    walkwayGenerator = GetComponent<WalkwayGenerator>();
  }

  public GameObject SpawnNextPiece(string pieceWord, int piecesLeftToSpawnInSection)
  {
    bool piecesInList = piecesBeingGenerated != null && piecesBeingGenerated.Count > 0;
    GameObject piece = null;
    piece = SpawnWalkwayPiece(pieceWord);

    // if (piecesInList)
    // {
    //   piece = SpawnNextPieceFromList(pieceWord);
    // }
    // else if (pieceTypeBeingGenerated == LevelPieceType.WALKWAY)
    // {
    //   piece = SpawnWalkwayPiece(pieceWord);
    // }
    // else if (pieceTypeBeingGenerated == LevelPieceType.PLATFORM)
    // {
    //   piece = SpawnPlatformPiece(pieceWord, piecesLeftToSpawnInSection);
    // }

    levelPiecesSpawned.Add(piece);
    OnLevelPieceSpawned?.Invoke(piece);

    return piece;
  }

  private GameObject SpawnWalkwayPiece(string pieceWord)
  {
    GameObject piece = walkwayGenerator.AddPieceToWalkway(levelPiecesSpawned.Last().transform, pieceWord);
    // piece.transform.parent = ground; 
    return piece;
  }

  // public void SetPlatformingPath(Vector3 pathStart, Vector3 pathEnd, int numberOfPiecesToAdd)
  // {
  //   piecesBeingGenerated = PathGenerator.GetPathBetweenPoints(pathStart, pathEnd, numberOfPiecesToAdd);
  // }

  // public Vector3 GetEndPointOfPiece(GameObject piece)
  // {
  //   MeshFilter meshFilter = piece.GetComponentInChildren<MeshFilter>();
  //   Mesh mesh = meshFilter.mesh;
  //   Vector3[] vertices = mesh.vertices;
  //   Vector3 mostUpForwardPoint = Vector3.negativeInfinity;

  //   foreach (Vector3 vertex in vertices)
  //   {
  //     Vector3 worldVertex = meshFilter.transform.TransformPoint(vertex);
  //     float forwardUpValue = worldVertex.y + worldVertex.z;
  //     float mostUpForwardValue = mostUpForwardPoint.y + mostUpForwardPoint.z;
  //     if (forwardUpValue > mostUpForwardValue)
  //     {
  //       mostUpForwardPoint = worldVertex;
  //     }
  //   }
  //   return mostUpForwardPoint;
  // }

  // public void DestroyAllPiecesAboveIndex(int indexToDestroyAbove)
  // {
  //   for (int i = levelPiecesSpawned.Count - 1; i > indexToDestroyAbove; i--)
  //   {
  //     GameObject piece = levelPiecesSpawned[i];
  //     levelPiecesSpawned.RemoveAt(i); // Remove from list
  //     Destroy(piece); // Destroy the GameObject
  //   }
  // }


  // public GameObject SpawnCustomPlatform(GameObject platformPrefab, float gapFromWalkwayEndToPlatformPivot)
  // {
  //   Vector3 endPointOfLastPiece = GetEndPointOfPiece(levelPiecesSpawned.Last());
  //   Vector3 platformPos = endPointOfLastPiece + Vector3.forward * gapFromWalkwayEndToPlatformPivot;
  //   GameObject platform = Instantiate(platformPrefab, platformPos, Quaternion.identity);
  //   return platform;
  // }

  // private GameObject SpawnPlatformPiece(string pieceWord, int piecesLeftToSpawnInSection)
  // {
  //   Transform lastLevelPieceAdded = levelPiecesSpawned.Last().transform;
  //   bool isReachingEnd = piecesLeftToSpawnInSection < minPlatformPieces;

  //   if (piecesOnCurrentPlatform >= piecesPerPlatform && !isReachingEnd)
  //   {
  //     GameObject piece = walkwayGenerator.GeneratePieceWithGap(lastLevelPieceAdded.transform, pieceWord);
  //     piecesOnCurrentPlatform = 1;
  //     StartNewPlatform(piece.transform.position);
  //     piece.transform.parent = lastPlatformStarted.pieces;
  //     return piece;
  //   }
  //   else
  //   {
  //     GameObject piece = walkwayGenerator.GenerateNextPiece(lastLevelPieceAdded.transform, pieceWord, true);
  //     piecesOnCurrentPlatform++;
  //     if (!lastPlatformStarted)
  //     {
  //       StartNewPlatform(piece.transform.position);
  //     }
  //     piece.transform.parent = lastPlatformStarted.pieces;
  //     return piece;
  //   }
  // }


  // private Platform StartNewPlatform(Vector3 pos)
  // {
  //   Platform newPlatform = Instantiate(platformsPrefab).GetComponent<Platform>();
  //   newPlatform.transform.position = pos;
  //   newPlatform.transform.parent = platformsParent;
  //   lastPlatformStarted = newPlatform;
  //   return newPlatform;
  // }



  // private GameObject SpawnNextPieceFromList(string pieceWord)
  // {
  //   LevelPiece nextPiece = piecesBeingGenerated[0];
  //   GameObject piece = walkwayGenerator.InstatiatePiece(nextPiece, pieceWord);
  //   piecesBeingGenerated.RemoveAt(0);
  //   if (nextPiece.isPlatformStart || !lastPlatformStarted)
  //   {
  //     StartNewPlatform(piece.transform.position);
  //   }
  //   piece.transform.parent = lastPlatformStarted.pieces;
  //   return piece;
  // }
}