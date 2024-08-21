using System.Collections.Generic;
using UnityEngine;

public class PathGenerator
{
  const float gapSize = 6f;
  const float minPlatformLength = 13;
  const float minPlatformLengthWithGap = minPlatformLength + gapSize;


  const float maxSideDeviationFromPath = 15f;
  const float maxUpDeviationFromPath = 15f;
  const float maxPlatformHeightShift = 1;
  const float maxPlatformSideShift = 4;
  const float maxPieceSideShift = 0.8f;


  public static List<LevelPiece> GetPathBetweenPoints(Vector3 pathStart, Vector3 pathEnd, int numberOfPiecesToAdd)
  {
    Vector3 vectorFromStartToEnd = pathEnd - pathStart;
    Vector3 startGap = vectorFromStartToEnd.normalized * gapSize;
    Vector3 pathVectorWithoutInitialGap = vectorFromStartToEnd - startGap;
    float pathLength = pathVectorWithoutInitialGap.magnitude;
    int numberOfPlatforms = Mathf.FloorToInt(pathLength / minPlatformLengthWithGap);
    float distanceCoveredByGaps = numberOfPlatforms * gapSize;
    float distanceCoveredByPlatforms = pathLength - distanceCoveredByGaps;
    float platformLength = distanceCoveredByPlatforms / numberOfPlatforms;
    float platformPlusGapLength = platformLength + gapSize;

    //direction vectors
    Vector3 pieceForwardVector = vectorFromStartToEnd.normalized;
    Vector3 pieceUpVector = Vector3.up;
    Vector3 pieceRightVector = Vector3.Cross(pieceUpVector, pieceForwardVector);
    pieceRightVector.Normalize();
    pieceUpVector = Vector3.Cross(pieceForwardVector, pieceRightVector).normalized;

    //calc position for each platform piece
    float averagePlatformPieceCount = numberOfPiecesToAdd / numberOfPlatforms;
    int minimumPlatformPieceCount = Mathf.FloorToInt(averagePlatformPieceCount);
    int totalPiecesIfAllPlatformsUseMinimumCount = numberOfPlatforms * minimumPlatformPieceCount;
    int leftOverPieceCount = numberOfPiecesToAdd - totalPiecesIfAllPlatformsUseMinimumCount;
    int numberOfPlatformsThatUseOneExtraPiece = leftOverPieceCount;

    float lengthOfPiecesInThePlatformsWithExtraPiece = platformLength / (minimumPlatformPieceCount + 1);
    float lengthOfPiecesInPlatformsWithMinumumPieceCount = platformLength / minimumPlatformPieceCount;
    List<LevelPiece> addedPieces = new();

    float lastPieceSideDeviation = 0;
    float lastPlatformUpDeviation = 0;

    for (int i = 0; i < numberOfPlatforms; i++)
    {
      bool usesExtraPiece = i < numberOfPlatformsThatUseOneExtraPiece;
      float distanceFromStartToNextPiece = 0;
      distanceFromStartToNextPiece += gapSize;
      distanceFromStartToNextPiece += platformPlusGapLength * i;
      int numberOfPiecesInThisPlatform = usesExtraPiece ? minimumPlatformPieceCount + 1 : minimumPlatformPieceCount;


      float upShift = Random.Range(-maxPlatformHeightShift, maxPlatformHeightShift);
      float platformUpDeviation = lastPlatformUpDeviation + upShift;
      int numberOfPlatformsLeftAfterThisOne = numberOfPlatforms - i;
      float maxUpDeviationForThisPlatform = numberOfPlatformsLeftAfterThisOne * 1; //magic
      if (maxUpDeviationForThisPlatform > maxUpDeviationFromPath) maxUpDeviationForThisPlatform = maxUpDeviationFromPath;
      bool deviatedTooFarUpways = Mathf.Abs(platformUpDeviation) > maxUpDeviationForThisPlatform;
      if (deviatedTooFarUpways)
      {
        float deviationOverFlow = Mathf.Abs(platformUpDeviation) - maxUpDeviationForThisPlatform;
        bool isBelowPath = platformUpDeviation < 0;
        platformUpDeviation += isBelowPath ? deviationOverFlow : -deviationOverFlow;
      }
      lastPlatformUpDeviation = platformUpDeviation;

      for (int j = 0; j < numberOfPiecesInThisPlatform; j++)
      {

        Vector3 vectorFromStartToPiece = vectorFromStartToEnd.normalized * distanceFromStartToNextPiece;

        //calc side shift
        bool isFirstPieceInPlatform = j == 0;
        float maxSideShift = isFirstPieceInPlatform ? maxPlatformSideShift : maxPieceSideShift;
        float sideShift = Random.Range(-maxSideShift, maxSideShift);
        float pieceSideDeviation = lastPieceSideDeviation + sideShift;
        int numberOfPiecesLeftAfterThisOne = numberOfPiecesToAdd - addedPieces.Count - 1;
        float maxSideDevationForThisPiece = numberOfPiecesLeftAfterThisOne / 2;
        if (maxSideDevationForThisPiece > maxSideDeviationFromPath) maxSideDevationForThisPiece = maxSideDeviationFromPath;
        bool deviatedTooFarSideways = Mathf.Abs(pieceSideDeviation) > maxSideDevationForThisPiece;
        if (deviatedTooFarSideways)
        {
          float deviationOverFlow = Mathf.Abs(pieceSideDeviation) - maxSideDevationForThisPiece;
          bool isNegative = pieceSideDeviation < 0;
          pieceSideDeviation += isNegative ? deviationOverFlow : -deviationOverFlow;
        }
        lastPieceSideDeviation = pieceSideDeviation;
        Vector3 pieceDeviation = pieceRightVector * pieceSideDeviation + pieceUpVector * platformUpDeviation;

        //add the piece
        LevelPiece piece = new()
        {
          isPlatformStart = j == 0,
          start = pathStart + vectorFromStartToPiece + pieceDeviation,
          length = usesExtraPiece ? lengthOfPiecesInThePlatformsWithExtraPiece : lengthOfPiecesInPlatformsWithMinumumPieceCount,
          forwardVector = pieceForwardVector
        };

        addedPieces.Add(piece);
        distanceFromStartToNextPiece += piece.length;
      }
    }

    return addedPieces;
  }
}