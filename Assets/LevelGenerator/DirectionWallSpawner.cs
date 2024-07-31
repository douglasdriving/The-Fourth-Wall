using UnityEngine;

public class DirectionWallSpawner : MonoBehaviour
{

  [SerializeField] GameObject wallPrefab;
  [SerializeField] int minPiecesBetweenWalls = 5;
  [SerializeField] int maxPiecesBetweenWalls = 10;

  int piecesSinceLastWall = 0;
  int piecesBeforeNextWall = 6;

  void OnEnable()
  {
    LevelGenerator.OnLevelPieceSpawned += OnPieceSpawned;
  }

  void OnDisable()
  {
    LevelGenerator.OnLevelPieceSpawned -= OnPieceSpawned;
  }

  void OnPieceSpawned(GameObject pieceToSpawnOver)
  {
    if (!CurrentGameRules.rules.directionWallsOn) return;
    piecesSinceLastWall++;
    if (piecesSinceLastWall >= piecesBeforeNextWall)
    {
      SpawnWall(pieceToSpawnOver);
      piecesSinceLastWall = 0;
      piecesBeforeNextWall = Random.Range(minPiecesBetweenWalls, maxPiecesBetweenWalls);
    }
  }

  void SpawnWall(GameObject pieceToSpawnOver)
  {
    Vector3 pos = pieceToSpawnOver.transform.position + Vector3.up;
    Quaternion rot = pieceToSpawnOver.transform.rotation;
    Instantiate(wallPrefab, pos, rot, pieceToSpawnOver.transform);
  }
}