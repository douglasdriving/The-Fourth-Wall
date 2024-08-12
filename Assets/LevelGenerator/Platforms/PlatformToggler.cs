using UnityEngine;

public class PlatformToggler : MonoBehaviour
{
  [SerializeField] Transform platformParent;

  public void PopAllPlatformsExceptTheOnePlayerIsOn()
  {
    GameObject lastGroundPlayerWasOn = FindObjectOfType<FirstPersonController>().lastGround;
    Platform lastPlatformPlayerWasOn = lastGroundPlayerWasOn.transform.GetComponentInParent<Platform>();
    DeactivateAllPLatformsExceptOne(lastPlatformPlayerWasOn);
  }

  void DeactivateAllPLatformsExceptOne(Platform platformToKeep)
  {
    Platform[] platforms = FindObjectsOfType<Platform>();
    foreach (Platform platform in platforms)
    {
      if (platform == platformToKeep) continue;
      platform.HidePlatform();
    }
  }
}