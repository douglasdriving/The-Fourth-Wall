using UnityEngine;

[RequireComponent(typeof(PlatformGenerator))]
[RequireComponent(typeof(WalkwayGenerator))]
public class LevelGenerator : MonoBehaviour
{
  public GameObject lastLevelPieceAdded;
  WalkwayGenerator walkwayGenerator;
  PlatformGenerator platformGenerator;

  void Awake()
  {
    if (!lastLevelPieceAdded) Debug.LogError("please assign a last level piece to start generating from");
    walkwayGenerator = GetComponent<WalkwayGenerator>();
    platformGenerator = GetComponent<PlatformGenerator>();
  }

  public void SpawnNextLevelPiece(LevelPieceType type)
  {
    Vector3 endPointOfLastPiece = GetEndPointOfPiece(lastLevelPieceAdded);
    if (type == LevelPieceType.WALKWAY)
    {
      lastLevelPieceAdded = walkwayGenerator.GenerateNextPiece(endPointOfLastPiece);
    }
    else if (type == LevelPieceType.PLATFORM)
    {
      lastLevelPieceAdded = platformGenerator.GenerateNextPlatform(endPointOfLastPiece);
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