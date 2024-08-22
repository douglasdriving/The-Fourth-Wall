using TMPro;
using UnityEngine;

public class WalkwayGenerator : MonoBehaviour
{
    [SerializeField] GameObject piecePrefab;
    [SerializeField] GameObject wordAnchorPrefab;
    [SerializeField] Transform wordAnchorParent;
    [SerializeField] Transform walkwayParent;
    public float platformGapSize = 12f;
    [SerializeField] float zScalePercentageOfPlatformPieces = 0.7f;

    //ok so, should we have a bool that determines if the word animation is active or not?
    public bool isWordAnimationActive = false;

    public GameObject AddPieceToWalkway(Transform pieceToMoveFrom, string pieceWord)
    {
        Vector3 prevPiecePivot = pieceToMoveFrom.position;
        Vector3 prevPieceScale = pieceToMoveFrom.lossyScale;
        Vector3 endOfPrevPiece = prevPiecePivot + Vector3.forward * prevPieceScale.z;
        float halfWidthOfPrevPiece = prevPieceScale.x / 2;
        Vector3 newPiecePivot = endOfPrevPiece;
        newPiecePivot.x += Random.Range(-halfWidthOfPrevPiece, halfWidthOfPrevPiece);

        Quaternion rot = Quaternion.identity;
        GameObject piece = InstatiatePiece(newPiecePivot, rot, pieceWord);

        // if (isPartOfPlatform)
        // {
        //     ScaleToPlatformPieceLength(piece);
        // }

        return piece;
    }

    public GameObject InstatiatePiece(Vector3 pivot, Quaternion rot, string pieceWord)
    {

        GameObject piece;

        if (isWordAnimationActive)
        {
            piece = FindAnyObjectByType<LevelPieceMolds>().CopyMold(); //does not have to find it every time
            piece.GetComponent<LevelPiecePositioner>().MoveToPosition(pivot, rot);
        }
        else
        {
            piece = Instantiate(piecePrefab, pivot, rot);
        }

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
