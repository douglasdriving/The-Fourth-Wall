using UnityEngine;

public class ObjectOnLevelPieceSpawner : MonoBehaviour
{

  [SerializeField] GameObject objectToSpawn;
  [SerializeField] int minPiecesBetweenObjects = 5;
  [SerializeField] int maxPiecesBetweenObjects = 10;

  int piecesSinceLastObject = 0;
  int piecesBeforeNextObject = 6;

  public bool isSpawningRegularly = false;
  public bool isSpawningOnNextPiece = false;

  void OnEnable()
  {
    LevelGenerator.OnLevelPieceSpawned += OnLevelPieceSpawned;
  }

  void OnDisable()
  {
    LevelGenerator.OnLevelPieceSpawned -= OnLevelPieceSpawned;
  }

  void OnLevelPieceSpawned(GameObject pieceToSpawnOver)
  {
    if (isSpawningOnNextPiece)
    {
      Spawn(pieceToSpawnOver);
      isSpawningOnNextPiece = false;
    }
    else if (isSpawningRegularly)
    {
      piecesSinceLastObject++;
      if (piecesSinceLastObject >= piecesBeforeNextObject)
      {
        Spawn(pieceToSpawnOver);
        piecesSinceLastObject = 0;
        piecesBeforeNextObject = Random.Range(minPiecesBetweenObjects, maxPiecesBetweenObjects);
      }
    }
  }

  void Spawn(GameObject pieceToSpawnOver)
  {
    Vector3 pos = pieceToSpawnOver.transform.position + Vector3.up;
    Quaternion rot = pieceToSpawnOver.transform.rotation;
    Instantiate(objectToSpawn, pos, rot, pieceToSpawnOver.transform);
  }
}