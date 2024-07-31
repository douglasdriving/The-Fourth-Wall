using System.Collections.Generic;
using UnityEngine;

public class PlatformPopper : MonoBehaviour
{

  [SerializeField] Transform platformParent;

  void OnEnable()
  {
    FirstPersonController.OnJumped += PopPlatformsIfRuleEnabled;
  }

  void OnDisable()
  {
    FirstPersonController.OnJumped -= PopPlatformsIfRuleEnabled;
  }

  void PopPlatformsIfRuleEnabled(GameObject groundJumpedFrom)
  {
    if (!CurrentGameRules.rules.platformPopOn) return;

    List<Transform> platforms = GetAllPlatforms();
    Transform platformJumpedFrom = groundJumpedFrom.transform.parent.parent;

    foreach (Transform platform in platforms)
    {
      if (platform == platformJumpedFrom) continue;
      platform.gameObject.SetActive(false);
    }
  }

  public List<Transform> GetAllPlatforms()
  {
    List<Transform> directChildren = new List<Transform>();

    for (int i = 0; i < platformParent.childCount; i++)
    {
      directChildren.Add(platformParent.GetChild(i));
    }

    return directChildren;
  }
}