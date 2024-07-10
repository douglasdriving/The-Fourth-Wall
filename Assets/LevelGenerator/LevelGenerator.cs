using UnityEngine;

// [RequireComponent(typeof(PlatformGenerator))]
[RequireComponent(typeof(WalkwayGenerator))]
public class LevelGenerator : MonoBehaviour
{
  public static LevelPieceType pieceTypeBeingGenerated = LevelPieceType.WALKWAY;
  public GameObject lastLevelPieceAdded;
  WalkwayGenerator walkwayGenerator;
  // PlatformGenerator platformGenerator;
  [SerializeField] int piecesPerPlatform = 12;
  int piecesOnCurrentPlatform;
  [SerializeField] int minPlatformPieces = 4;

  void Awake()
  {
    piecesOnCurrentPlatform = piecesPerPlatform;
    if (!lastLevelPieceAdded) Debug.LogError("please assign a last level piece to start generating from");
    walkwayGenerator = GetComponent<WalkwayGenerator>();
    // platformGenerator = GetComponent<PlatformGenerator>();
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

  public void SpawnNextLevelPiece(string pieceWord, int piecesLeftToSpawnInSection)
  {
    // Vector3 endPointOfLastPiece = GetEndPointOfPiece(lastLevelPieceAdded);

    if (pieceTypeBeingGenerated == LevelPieceType.WALKWAY)
    {
      lastLevelPieceAdded = walkwayGenerator.GenerateNextPiece(lastLevelPieceAdded.transform, pieceWord, false);
    }
    else if (pieceTypeBeingGenerated == LevelPieceType.PLATFORM)
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