using UnityEngine;

public class WalkwayGenerator : MonoBehaviour
{
    [SerializeField] GameObject piecePrefab;
    [SerializeField] GameObject wordAnchorPrefab;
    [SerializeField] float maxTurnAngle = 45;
    [SerializeField] float minLength = 3;
    [SerializeField] float maxLength = 10;
    [SerializeField] float maxPieceYRot = 90;
    [SerializeField] Transform wordAnchorParent;
    [SerializeField] Transform walkwayParent;
    [SerializeField] float pieceOverlap = 0.5f;
    [SerializeField] float wordHeightAbovePlatform = 1f;

    public GameObject GenerateNextPiece(Vector3 pointToMoveFrom, Transform pieceToMoveFrom)
    {
        GameObject piece = AddPieceToWalkway(pointToMoveFrom, pieceToMoveFrom);
        InstantiateWordAnchorAboveEndOfPiece(piece);
        return piece;
    }

    private GameObject AddPieceToWalkway(Vector3 pointToMoveFrom, Transform prevPiece)
    {

        //problem är att "point to move from" är KANTEN av biten
        //inte mitten
        //jag tänker mig kanske att vi bara vill ha mitten? kolla längs fram på biten
        Vector3 prevPiecePivot = prevPiece.position;
        Vector3 prevPieceScale = prevPiece.lossyScale;
        Vector3 endOfPrevPiece = prevPiecePivot + Vector3.forward * prevPieceScale.z;
        float halfWidthOfPrevPiece = prevPieceScale.x / 2;
        Vector3 newPiecePivot = endOfPrevPiece;
        newPiecePivot.x += Random.Range(-halfWidthOfPrevPiece, halfWidthOfPrevPiece);

        // Quaternion rot = GetRandomNewPieceRot();
        Quaternion rot = Quaternion.identity;
        // float halfHeightOfWalkwayPiece = piecePrefab.transform.lossyScale.y / 2;
        // float widthOfPiece = piecePrefab.transform.lossyScale.x;
        // pointToMoveFrom.y -= halfHeightOfWalkwayPiece;
        // pointToMoveFrom.x -= Random.Range(halfWidthOfPrevPiece, halfWidthOfPrevPiece);
        // float overlap = 1.5f;
        // Vector3 directionTowardsLastPiece = (pieceToMoveFrom.position - pointToMoveFrom).normalized;
        // pointToMoveFrom += directionTowardsLastPiece * overlap;
        GameObject piece = InstatiatePiece(newPiecePivot, rot);
        // RandomizePieceLength(piece);
        return piece;
    }

    private Quaternion GetRandomNewPieceRot()
    {
        float randomTurnAngle = Random.Range(-maxTurnAngle, maxTurnAngle);
        Vector3 rotEuler = new Vector3(0, randomTurnAngle, 0);
        Quaternion rot = Quaternion.Euler(rotEuler);
        return rot;
    }

    private GameObject InstatiatePiece(Vector3 startPos, Quaternion rot)
    {
        GameObject piece = Instantiate(piecePrefab, startPos, rot);
        piece.transform.parent = walkwayParent;
        return piece;
    }

    private void RandomizePieceLength(GameObject piece)
    {
        float length = Random.Range(minLength, maxLength);
        Vector3 scale = piece.transform.localScale;
        scale.z = length;
        piece.transform.localScale = scale;
    }

    private void InstantiateWordAnchorAboveEndOfPiece(GameObject piece)
    {
        Vector3 pos = FindObjectOfType<LevelGenerator>().GetEndPointOfPiece(piece);
        Vector3 wordPos = new Vector3(pos.x, pos.y + wordHeightAbovePlatform, pos.z);
        GameObject wordPoint = Instantiate(wordAnchorPrefab, wordPos, Quaternion.identity);
        wordPoint.transform.parent = wordAnchorParent;
    }
}
