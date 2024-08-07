using Narration;
using UnityEngine;

/// <summary>
/// sets a level generator path when the player enters the trigger volume
/// </summary>
public class TransformToTaggedPathTrigger : MonoBehaviour
{
  [SerializeField] Transform pathStart = null;
  [SerializeField] string pathEndObjectTag;
  [SerializeField] TextAsset subtitleToGeneratePathFor;
  private bool triggered = false;

  void OnTriggerEnter(Collider other)
  {
    if (!other.CompareTag("Player")) return;
    if (triggered) return;
    SavePathInLevelGenerator();
    triggered = true;
  }

  private void SavePathInLevelGenerator()
  {
    Vector3 startPoint = pathStart.position;
    Vector3 pathEnd = GameObject.FindWithTag(pathEndObjectTag).transform.position;
    int totalPlatformPieceCount = SubtitleJsonReader.CountWordsInSubtitleFile(subtitleToGeneratePathFor);
    FindObjectOfType<LevelGenerator>().SetPlatformingPath(startPoint, pathEnd, totalPlatformPieceCount);
  }
}