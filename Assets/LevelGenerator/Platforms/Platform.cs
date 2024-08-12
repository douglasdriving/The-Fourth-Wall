using UnityEngine;

/// <summary>
/// hides a platform, and shows it again after a player looked away
/// </summary>
public class Platform : MonoBehaviour
{
  public Transform pieces;

  float percentagePlayerMustLookAwayForActivation = 0.6f;
  LookAwayChecker playerLookAwayChecker;

  void Awake()
  {
    playerLookAwayChecker = GameObject.FindWithTag("Player").GetComponent<LookAwayChecker>();
  }

  public void HidePlatform()
  {
    pieces.gameObject.SetActive(false);
  }

  void Update()
  {
    ActivateIfPlayerIsLookingAway();
  }

  private void ActivateIfPlayerIsLookingAway()
  {
    if (pieces.gameObject.activeInHierarchy) return;
    bool isPlayerLookingAway = playerLookAwayChecker.IsPlayerLookingAway(transform, percentagePlayerMustLookAwayForActivation);
    if (isPlayerLookingAway)
    {
      pieces.gameObject.SetActive(true);
    }
  }
}