using UnityEngine;

namespace LevelGeneration
{
  /// <summary>
  /// Destroys the level piece after a set amount of time after it has been positioned
  /// </summary>
  [RequireComponent(typeof(LevelPiecePositioner))]
  public class LevelPieceDestroyTimer : MonoBehaviour
  {
    [SerializeField] float destroyTimer = 3f;
    public bool startDestroyTimerWhenPositioned = false;
    bool timerIsRunning = false;
    LevelPiecePositioner positioner;

    private void Awake()
    {
      positioner = GetComponent<LevelPiecePositioner>();
    }

    private void Update()
    {
      if (startDestroyTimerWhenPositioned)
      {
        StartTimerIfPositioned();
      }
      if (timerIsRunning)
      {
        UpdateRunningTimer();
      }
    }

    void StartTimerIfPositioned()
    {
      if (positioner.reachedFinalPosition)
      {
        timerIsRunning = true;
        startDestroyTimerWhenPositioned = false;
      }
    }

    private void UpdateRunningTimer()
    {
      destroyTimer -= Time.deltaTime;
      if (destroyTimer <= 0)
      {
        Destroy(gameObject);
      }
    }
  }
}