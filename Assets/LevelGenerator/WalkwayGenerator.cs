using TMPro;
using UnityEngine;

public class WalkwayGenerator : MonoBehaviour
{
    // [SerializeField] GameObject piecePrefab;
    // [SerializeField] GameObject wordAnchorPrefab;
    // [SerializeField] Transform wordAnchorParent;
    // [SerializeField] Transform walkwayParent;
    // public float platformGapSize = 12f;
    // [SerializeField] float zScalePercentageOfPlatformPieces = 0.7f;
    LevelPieceMolds levelPieceMolds;
    [SerializeField] float sentanceGapSize = 3f;
    public bool isWordAnimationActive = false;
    public bool isSeparatingSentences = false;

    private void Start()
    {
        levelPieceMolds = FindAnyObjectByType<LevelPieceMolds>();
    }

    public GameObject AddPieceToWalkway(Transform pieceToMoveFrom, string pieceWord)
    {
        Vector3 newPiecePivot = GetNextPiecePivot(pieceToMoveFrom);
        GameObject piece = InstatiatePiece(newPiecePivot, Quaternion.identity, pieceWord);

        // if (isPartOfPlatform)
        // {
        //     ScaleToPlatformPieceLength(piece);
        // }

        return piece;
    }

    private Vector3 GetNextPiecePivot(Transform pieceToMoveFrom)
    {
        Vector3 prevPieceFinalPivot = pieceToMoveFrom.position;
        Vector3 prevPieceFinalScale = pieceToMoveFrom.lossyScale;

        if (isWordAnimationActive)
        {
            LevelPiecePositioner prevPiecePositioner = pieceToMoveFrom.GetComponent<LevelPiecePositioner>();
            prevPieceFinalPivot = prevPiecePositioner.targetPosition;
            prevPieceFinalScale = prevPiecePositioner.targetPieceScale;
        }

        Vector3 finalEndOfPrevPiece = prevPieceFinalPivot + Vector3.forward * prevPieceFinalScale.z;
        float halfWidthOfPrevPiece = prevPieceFinalScale.x / 2;

        Vector3 newPiecePivot = finalEndOfPrevPiece;
        newPiecePivot.x += Random.Range(-halfWidthOfPrevPiece, halfWidthOfPrevPiece);

        if (isSeparatingSentences)
        {
            newPiecePivot = ExtendWithSentenceGapIfLastPieceEndedSentence(pieceToMoveFrom, newPiecePivot);
        }

        return newPiecePivot;
    }

    private Vector3 ExtendWithSentenceGapIfLastPieceEndedSentence(Transform lastPiece, Vector3 piecePivotToExtend)
    {
        bool lastPieceEndedSentence = false;
        if (lastPiece != null)
        {
            lastPieceEndedSentence = IsPieceWordEndingSentence(lastPiece);
        }
        if (lastPieceEndedSentence)
        {
            piecePivotToExtend.z += sentanceGapSize;
        }

        return piecePivotToExtend;
    }

    private static bool IsPieceWordEndingSentence(Transform pieceToMoveFrom)
    {
        bool startsNewSentece = false;
        TMP_Text prevPieceTextElement = pieceToMoveFrom.GetComponentInChildren<TMP_Text>();
        if (prevPieceTextElement != null)
        {
            startsNewSentece = prevPieceTextElement.text.EndsWith(".");
        }

        return startsNewSentece;
    }

    public GameObject InstatiatePiece(Vector3 pivot, Quaternion rot, string pieceWord)
    {
        GameObject piece = levelPieceMolds.CopyNextMold();
        piece.GetComponent<LevelPiecePositioner>().MoveToPosition(pivot, rot, isWordAnimationActive);
        piece.GetComponentInChildren<TMP_Text>().text = pieceWord;
        return piece;
    }

    // private void ScaleToPlatformPieceLength(GameObject piece)
    // {
    //     Vector3 shorterPieceScale = piece.transform.localScale;
    //     shorterPieceScale.z *= zScalePercentageOfPlatformPieces;
    //     piece.transform.localScale = shorterPieceScale;
    // }

    // public GameObject GeneratePieceWithGap(Transform prevPiece, string pieceWord)
    // {
    //     Vector3 prevPiecePivot = prevPiece.position;
    //     Vector3 prevPieceScale = prevPiece.lossyScale;
    //     Vector3 endOfPrevPiece = prevPiecePivot + Vector3.forward * prevPieceScale.z;

    //     Vector3 gap = Vector3.one;
    //     gap.x = Random.Range(-5, 5);
    //     gap.y = Random.Range(-3, 3);
    //     gap.z = Random.Range(4, 10);
    //     gap = gap.normalized * platformGapSize;

    //     Vector3 newPiecePivot = endOfPrevPiece + gap;

    //     Quaternion rot = Quaternion.identity;

    //     LevelPiece pieceInfo = new LevelPiece();
    //     pieceInfo.start = newPiecePivot;
    //     pieceInfo.forwardVector = prevPiece.forward;
    //     pieceInfo.length = prevPieceScale.z;

    //     GameObject piece = InstatiatePiece(newPiecePivot, rot);

    //     ScaleToPlatformPieceLength(piece);

    //     piece.GetComponentInChildren<TMP_Text>().text = pieceWord;

    //     return piece;
    // }
}
