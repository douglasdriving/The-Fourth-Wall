using UnityEngine;

public class WalkwayGenerator : MonoBehaviour
{
    [SerializeField] GameObject lastInstatiatedWalkwayPiece;
    [SerializeField] GameObject piecePrefab;
    [SerializeField] GameObject wordAnchorPrefab;
    [SerializeField] float maxTurnAngle = 45;
    [SerializeField] float minLength = 3;
    [SerializeField] float maxLength = 10;
    [SerializeField] Transform wordAnchorParent;
    [SerializeField] Transform walkwayParent;
    [SerializeField] float pieceOverlap = 0.5f;

    public void GenerateNextSection()
    {
        AddPieceToWalkway();
        InstantiateWordAnchorAboveLastPieceEndPoint();
    }

    private Vector3 GetNextPieceStartPoint()
    {
        Vector3 startOfCurrentPiece = lastInstatiatedWalkwayPiece.transform.position;
        Vector3 currentPieceDir = lastInstatiatedWalkwayPiece.transform.forward;
        float currentPieceLength = lastInstatiatedWalkwayPiece.transform.lossyScale.z;
        Vector3 endOfCurrentPiece = startOfCurrentPiece + (currentPieceDir * currentPieceLength);
        Vector3 startOfNextPiece = endOfCurrentPiece - (currentPieceDir * pieceOverlap);
        return startOfNextPiece;
    }

    private void AddPieceToWalkway()
    {
        Vector3 startPos = GetNextPieceStartPoint();
        Quaternion rot = GetRandomTurnRot();
        GameObject piece = InstatiatePiece(startPos, rot);
        RandomizePieceLength(piece);
    }

    private Quaternion GetRandomTurnRot()
    {
        float turnAngle = Random.Range(-maxTurnAngle, maxTurnAngle);
        Vector3 rotVector = new Vector3(0, turnAngle, 0);
        Quaternion rot = Quaternion.Euler(rotVector);
        return rot;
    }

    private GameObject InstatiatePiece(Vector3 startPos, Quaternion rot)
    {
        GameObject piece = Instantiate(piecePrefab, startPos, rot);
        piece.transform.parent = walkwayParent;
        lastInstatiatedWalkwayPiece = piece;
        return piece;
    }

    private void RandomizePieceLength(GameObject piece)
    {
        float length = Random.Range(minLength, maxLength);
        Vector3 scale = piece.transform.localScale;
        scale.z = length;
        piece.transform.localScale = scale;
    }

    private void InstantiateWordAnchorAboveLastPieceEndPoint()
    {
        Vector3 pos = GetNextPieceStartPoint();
        float heightAbovePlatform = 2f;
        Vector3 wordPos = new Vector3(pos.x, pos.y + heightAbovePlatform, pos.z);
        GameObject wordPoint = Instantiate(wordAnchorPrefab, wordPos, Quaternion.identity);
        wordPoint.transform.parent = wordAnchorParent;
    }
}
