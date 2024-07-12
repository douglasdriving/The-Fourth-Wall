using System.Collections.Generic;
using Narration;
using UnityEngine;
using UnityEngine.UIElements;

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


    Vector3 vectorFromStartToEnd = pathEnd - pathStart;
    float platformGapSize = FindObjectOfType<WalkwayGenerator>().platformGapSize; // this could just be in the level generator...
    Vector3 startGap = vectorFromStartToEnd.normalized * platformGapSize;
    Vector3 pathVector = vectorFromStartToEnd - startGap;
    float pathLength = pathVector.magnitude;

    Vector3 pieceForwardVector = vectorFromStartToEnd.normalized;
    Vector3 pieceUpVector = Vector3.up; //unsure if this is correct. probably not.
    Vector3 pieceRightVector = Vector3.Cross(pieceUpVector, pieceForwardVector);
    pieceRightVector.Normalize();

    //calc position for each platform piece
    float platformLength = 15f; //magic, define somewhere else
    float lengthOfPlatformWithGap = platformLength + platformGapSize;
    int numberOfPlatforms = Mathf.FloorToInt(pathLength / lengthOfPlatformWithGap);
    float averagePlatformPieceCount = numberOfPiecesToAdd / numberOfPlatforms;
    int minimumPlatformPieceCount = Mathf.FloorToInt(averagePlatformPieceCount);
    int totalPiecesIfAllPlatformsUseMinimumCount = numberOfPlatforms * minimumPlatformPieceCount;
    int leftOverPieceCount = numberOfPiecesToAdd - totalPiecesIfAllPlatformsUseMinimumCount;
    int numberOfPlatformsThatUseOneExtraPiece = leftOverPieceCount;

    float lengthOfPiecesInThePlatformsWithExtraPiece = platformLength / (minimumPlatformPieceCount + 1);
    float lengthOfPiecesInPlatformsWithMinumumPieceCount = platformLength / minimumPlatformPieceCount;
    List<LevelPiece> addedPieces = new();

    float lastPieceSideDeviation = 0;
    float maxSideDeviation = 15f; //magic!

    for (int i = 0; i < numberOfPlatforms; i++)
    {
      bool usesExtraPiece = i < numberOfPlatformsThatUseOneExtraPiece;
      float distanceFromStartToNextPiece = 0;
      distanceFromStartToNextPiece += platformGapSize; //start gap
      distanceFromStartToNextPiece += lengthOfPlatformWithGap * i; //each platform before this
      int numberOfPiecesInThisPlatform = usesExtraPiece ? minimumPlatformPieceCount + 1 : minimumPlatformPieceCount;
      for (int j = 0; j < numberOfPiecesInThisPlatform; j++)
      {

        LevelPiece piece = new();
        Vector3 vectorFromStartToPiece = vectorFromStartToEnd.normalized * distanceFromStartToNextPiece;

        //calc deviation
        int numberOfPiecesLeft = numberOfPiecesToAdd - addedPieces.Count;
        float maxSideDevationForThisPiece = numberOfPiecesLeft / 2;
        maxSideDeviation = Mathf.Clamp(maxSideDeviation, -maxSideDevationForThisPiece, maxSideDevationForThisPiece);
        float pieceSideDeviation = lastPieceSideDeviation + Random.Range(-0.5f, 0.5f);
        bool deviatedTooFar = Mathf.Abs(pieceSideDeviation) > maxSideDeviation;
        if (deviatedTooFar)
        {
          float deviationOverFlow = Mathf.Abs(pieceSideDeviation) - maxSideDeviation;
          bool isNegative = pieceSideDeviation < 0;
          pieceSideDeviation += isNegative ? deviationOverFlow : -deviationOverFlow;
        }
        lastPieceSideDeviation = pieceSideDeviation;
        Vector3 pieceDeviation = pieceRightVector * pieceSideDeviation;

        //add the piece
        piece.start = pathStart + vectorFromStartToPiece + pieceDeviation;
        piece.length = usesExtraPiece ? lengthOfPiecesInThePlatformsWithExtraPiece : lengthOfPiecesInPlatformsWithMinumumPieceCount;
        piece.forwardVector = pieceForwardVector;
        addedPieces.Add(piece);
        distanceFromStartToNextPiece += piece.length;

      }
    }
    piecesBeingGenerated = addedPieces;

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