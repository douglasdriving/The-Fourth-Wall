using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Narration
{
  /// <summary>
  /// spawns trigger volumes into the world that unpauses the current narration track
  /// </summary>
  public class UnpauseTriggerActivator : MonoBehaviour
  {
    public static void ActivateUnpauseTriggerOnLastPieces(int numberOfPieces)
    {

      List<GameObject> levelPieces = FindObjectOfType<LevelGenerator>().levelPiecesSpawned;
      List<GameObject> piecesToUnpauseOn = levelPieces.Skip(Math.Max(0, levelPieces.Count - numberOfPieces)).ToList();
      List<UnpauseTrigger> unpauseTriggersToActivate = new();

      foreach (GameObject pieceToUnpauseOn in piecesToUnpauseOn)
      {
        unpauseTriggersToActivate.Add(pieceToUnpauseOn.GetComponentInChildren<UnpauseTrigger>());
      }

      foreach (UnpauseTrigger unpauseTrigger in unpauseTriggersToActivate)
      {
        List<UnpauseTrigger> otherTriggers = unpauseTriggersToActivate.Where(u => u != unpauseTrigger).ToList();
        unpauseTrigger.SetActiveAndLink(otherTriggers);
      }

    }
  }
}
