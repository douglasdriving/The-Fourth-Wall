using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlatformToggler : MonoBehaviour
{

  [SerializeField] Transform platformParent;
  KeyControl activationKey = null;

  void Awake()
  {
    SetActivationKey(CurrentGameRules.rules.platformRespawnKey);
  }

  void OnEnable()
  {
    FirstPersonController.OnJumped += PopPlatformsIfRuleEnabled;
  }

  void OnDisable()
  {
    FirstPersonController.OnJumped -= PopPlatformsIfRuleEnabled;
  }

  void Update()
  {
    if (activationKey != null && activationKey.wasPressedThisFrame)
    {
      ActivateAllPlatforms();
    }
  }

  private void ActivateAllPlatforms()
  {
    Debug.Log("activating all platforms!");
    List<Transform> platforms = GetAllPlatforms();
    foreach (Transform platform in platforms)
    {
      platform.gameObject.SetActive(true);
    }
  }

  void PopPlatformsIfRuleEnabled(GameObject groundJumpedFrom)
  {
    if (!CurrentGameRules.rules.platformPopOn) return;

    List<Transform> platforms = GetAllPlatforms();
    Transform platformJumpedFrom = groundJumpedFrom.transform.parent.parent;

    DeactivateAllPLatformsExceptOne(platforms, platformJumpedFrom);
  }

  void DeactivateAllPLatformsExceptOne(List<Transform> platforms, Transform platformToKeep)
  {
    foreach (Transform platform in platforms)
    {
      if (platform == platformToKeep) continue;
      platform.gameObject.SetActive(false);
    }
  }

  public void SetActivationKey(KeyControl newKey)
  {
    activationKey = newKey;
    Debug.Log("activation key set to " + activationKey);
  }

  List<Transform> GetAllPlatforms()
  {
    List<Transform> directChildren = new List<Transform>();

    for (int i = 0; i < platformParent.childCount; i++)
    {
      directChildren.Add(platformParent.GetChild(i));
    }

    return directChildren;
  }

}