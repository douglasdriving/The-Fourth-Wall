using System.Collections.Generic;
using Narration;
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
  List<Piece> piecesBeingGenerated;

  void Awake()
  {
    piecesOnCurrentPlatform = piecesPerPlatform;
    if (!lastLevelPieceAdded) Debug.LogError("please assign a last level piece to start generating from");
    walkwayGenerator = GetComponent<WalkwayGenerator>();
    // platformGenerator = GetComponent<PlatformGenerator>();
  }


  public void SetPlatformingPath(Vector3 pathStart, Vector3 pathEnd, int numberOfPieces)
  {
    Vector3 vectorFromStartToEnd = pathEnd - pathStart;
    float platformGapSize = FindObjectOfType<WalkwayGenerator>().platformGapSize;
    Vector3 startGap = vectorFromStartToEnd.normalized * platformGapSize;
    Vector3 pathVector = vectorFromStartToEnd - startGap;
    float pathLength = pathVector.magnitude;

    //calc position for each platform piece
    float platformLength = 15f; //magic, define somewhere else
    float lengthOfPlatformWithGap = platformLength + platformGapSize;
    int numberOfPlatforms = Mathf.FloorToInt(pathLength / lengthOfPlatformWithGap);
    float piecesPerPlatform = numberOfPieces / numberOfPlatforms;
    int piecesPerPlatformFloored = Mathf.FloorToInt(piecesPerPlatform);
    int totalPiecesIfAllPlatformsUseFlooredCount = numberOfPlatforms * piecesPerPlatformFloored;
    int leftOverPieceCount = numberOfPieces - totalPiecesIfAllPlatformsUseFlooredCount;
    int numberOfPlatformsThatUseOneExtraPiece = leftOverPieceCount;
    float lengthOfPiecesInTheFirstPlatforms = platformLength / (piecesPerPlatformFloored + 1);
    float lengthOfPiecesInTheLatterPlatforms = platformLength / piecesPerPlatformFloored;
    List<Piece> pieceStartPositions = new();
    for (int i = 0; i < numberOfPlatforms; i++)
    {
      bool usesExtraPiece = i < numberOfPlatformsThatUseOneExtraPiece;
      float distanceFromStartToNextPiece = 0;
      distanceFromStartToNextPiece += platformGapSize; //start gap
      distanceFromStartToNextPiece += platformLength * i; //each platform before this
      distanceFromStartToNextPiece += platformGapSize * i; //each gap before this
      for (int j = 0; j < piecesPerPlatformFloored + 1; j++)
      {
        Piece piece = new();
        Vector3 vectorFromStartToPiece = vectorFromStartToEnd.normalized * distanceFromStartToNextPiece;
        piece.start = pathStart + vectorFromStartToPiece;
        piece.length = usesExtraPiece ? lengthOfPiecesInTheFirstPlatforms : lengthOfPiecesInTheLatterPlatforms;
        pieceStartPositions.Add(piece);
        distanceFromStartToNextPiece += piece.length;
      }
    }
    piecesBeingGenerated = pieceStartPositions;
  }

  struct Piece
  {
    public Vector3 start;
    public float length;
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

    //!!! if there is a list of pieces being generated defined, we should follow that list.


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