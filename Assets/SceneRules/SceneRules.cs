using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class SceneRules : MonoBehaviour
{
    public bool freezePiecesOnSpawn = false;
    public bool pieceSpawnSpread = false;
    public bool colorPieces = false;

    public enum EndSpawnObject
    {
        PORTAL,
        PORTAL_QUIZ,
        VOTING_PATHS,
        THOUGHT_LEVITATOR,
    }
    public EndSpawnObject portalType = EndSpawnObject.PORTAL;
    public bool pauseBetweenSentences = false;
}
