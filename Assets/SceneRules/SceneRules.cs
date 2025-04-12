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

    public enum PortalType
    {
        NORMAL,
        QUIZ,
        VOTING_PATHS,
    }
    public PortalType portalType = PortalType.NORMAL;
    public bool pauseBetweenSentences = false;
}
