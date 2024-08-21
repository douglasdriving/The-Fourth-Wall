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

    public GameObject GenerateAtExactSpot(LevelPiece piece, string pieceWord)
    {
        GameObject pieceGO = InstatiatePiece(piece.start, Quaternion.identity);

        Transform pieceT = pieceGO.transform;

        pieceT.forward = piece.forwardVector;

        Vector3 pieceLocalScale = pieceT.localScale;
        pieceLocalScale.z = piece.length;
        pieceT.localScale = pieceLocalScale;

        pieceGO.GetComponentInChildren<TMP_Text>().text = pieceWord;
        return pieceGO;
    }

    public GameObject GenerateNextPiece(Transform pieceToMoveFrom, string pieceWord, bool isPartOfPlatform)
    {
        GameObject piece = AddPieceToWalkway(pieceToMoveFrom);
        piece.GetComponentInChildren<TMP_Text>().text = pieceWord;
        if (isPartOfPlatform)
        {
            ScaleToPlatformPieceLength(piece);
        }
        return piece;
    }

    public GameObject GeneratePieceWithGap(Transform prevPiece, string pieceWord)
    {
        Vector3 prevPiecePivot = prevPiece.position;
        Vector3 prevPieceScale = prevPiece.lossyScale;
        Vector3 endOfPrevPiece = prevPiecePivot + Vector3.forward * prevPieceScale.z;

        Vector3 gap = Vector3.one;
        gap.x = Random.Range(-5, 5);
        gap.y = Random.Range(-3, 3);
        gap.z = Random.Range(4, 10);
        gap = gap.normalized * platformGapSize;

        Vector3 newPiecePivot = endOfPrevPiece + gap;

        Quaternion rot = Quaternion.identity;
        GameObject piece = InstatiatePiece(newPiecePivot, rot);

        ScaleToPlatformPieceLength(piece);

        piece.GetComponentInChildren<TMP_Text>().text = pieceWord;

        return piece;
    }

    private void ScaleToPlatformPieceLength(GameObject piece)
    {
        Vector3 shorterPieceScale = piece.transform.localScale;
        shorterPieceScale.z *= zScalePercentageOfPlatformPieces;
        piece.transform.localScale = shorterPieceScale;
    }

    private GameObject AddPieceToWalkway(Transform prevPiece)
    {
        Vector3 prevPiecePivot = prevPiece.position;
        Vector3 prevPieceScale = prevPiece.lossyScale;
        Vector3 endOfPrevPiece = prevPiecePivot + Vector3.forward * prevPieceScale.z;
        float halfWidthOfPrevPiece = prevPieceScale.x / 2;
        Vector3 newPiecePivot = endOfPrevPiece;
        newPiecePivot.x += Random.Range(-halfWidthOfPrevPiece, halfWidthOfPrevPiece);

        Quaternion rot = Quaternion.identity;
        GameObject piece = InstatiatePiece(newPiecePivot, rot);
        return piece;
    }


    private GameObject InstatiatePiece(Vector3 startPos, Quaternion rot)
    {
        GameObject piece = Instantiate(piecePrefab, startPos, rot);
        return piece;
    }
}
