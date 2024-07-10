using UnityEngine;

[RequireComponent(typeof(PlatformGenerator))]
[RequireComponent(typeof(WalkwayGenerator))]
public class LevelGenerator : MonoBehaviour
{
  public static LevelPieceType pieceTypeBeingGenerated = LevelPieceType.WALKWAY;
  public GameObject lastLevelPieceAdded;
  WalkwayGenerator walkwayGenerator;
  PlatformGenerator platformGenerator;
  int piecesPerPlatform = 8;
  int piecesOnCurrentPlatform = 8;

  void Awake()
  {
    if (!lastLevelPieceAdded) Debug.LogError("please assign a last level piece to start generating from");
    walkwayGenerator = GetComponent<WalkwayGenerator>();
    platformGenerator = GetComponent<PlatformGenerator>();
  }
  public GameObject SpawnCustomPlatform(GameObject platformPrefab)
  {
    Vector3 endPointOfLastPiece = GetEndPointOfPiece(lastLevelPieceAdded);
    GameObject platformInstance = platformGenerator.GenerateCustomPlatform(endPointOfLastPiece, platformPrefab);
    // lastLevelPieceAdded = platformInstance;
    return platformInstance;
  }

  public void SpawnNextLevelPiece(string pieceWord = "")
  {
    // Vector3 endPointOfLastPiece = GetEndPointOfPiece(lastLevelPieceAdded);
    if (pieceTypeBeingGenerated == LevelPieceType.WALKWAY)
    {
      lastLevelPieceAdded = walkwayGenerator.GenerateNextPiece(lastLevelPieceAdded.transform, pieceWord);
    }
    else if (pieceTypeBeingGenerated == LevelPieceType.PLATFORM)
    {
      //use gaps!
      // lastLevelPieceAdded = platformGenerator.GenerateNextPlatform(endPointOfLastPiece);
      if (piecesOnCurrentPlatform >= piecesPerPlatform)
      {
        lastLevelPieceAdded = walkwayGenerator.GeneratePieceWithGap(lastLevelPieceAdded.transform, pieceWord);
        piecesOnCurrentPlatform = 1;
      }
      else
      {
        lastLevelPieceAdded = walkwayGenerator.GenerateNextPiece(lastLevelPieceAdded.transform, pieceWord);
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