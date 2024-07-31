using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlatformToggler : MonoBehaviour
{

  [SerializeField] Transform platformParent;
  KeyControl activationKey;

  void Awake()
  {
    SetActivationKey(CurrentGameRules.rules.GetPlatformSpawnKey());
  }

  void OnEnable()
  {
    FirstPersonController.OnJumped += PopPlatformsIfRuleEnabled;
    CurrentGameRules.rules.OnPlatformSpawnKeySet += SetActivationKey;
  }

  void OnDisable()
  {
    FirstPersonController.OnJumped -= PopPlatformsIfRuleEnabled;
    CurrentGameRules.rules.OnPlatformSpawnKeySet -= SetActivationKey;
  }

  void Update()
  {
    if (activationKey.wasPressedThisFrame)
    {
      ActivateAllPlatforms();
    }
  }

  private void ActivateAllPlatforms()
  {
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

  void SetActivationKey(KeyControl newKey)
  {
    activationKey = newKey;
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