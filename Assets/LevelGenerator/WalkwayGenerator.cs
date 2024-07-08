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

    public GameObject GenerateNextPiece(Vector3 pointToMoveFrom)
    {
        GameObject piece = AddPieceToWalkway(pointToMoveFrom);
        InstantiateWordAnchorAboveEndOfPiece(piece); //do something with this too?
        return piece;
    }

    private GameObject AddPieceToWalkway(Vector3 pointToMoveFrom)
    {
        Quaternion rot = GetRandomNewPieceRot();
        float halfHeightOfWalkwayPiece = piecePrefab.transform.lossyScale.y / 2;
        pointToMoveFrom.y -= halfHeightOfWalkwayPiece;
        GameObject piece = InstatiatePiece(pointToMoveFrom, rot);
        RandomizePieceLength(piece);
        return piece;
    }

    private Quaternion GetRandomNewPieceRot()
    {

        // float lastPieceRotAngle = lastAddedPiece.transform.rotation.eulerAngles.y;
        // if (lastPieceRotAngle > 180) lastPieceRotAngle -= 360;
        // float newPieceRotAngle = lastPieceRotAngle + randomTurnAngle;
        // float newPieceRotAngleClamped = Mathf.Clamp(newPieceRotAngle, -maxPieceYRot, maxPieceYRot);
        // Vector3 rotEuler = new Vector3(0, newPieceRotAngleClamped, 0);

        //if we want this more sophisticated, it can take the rotation of the last piece into account

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
        float heightAbovePlatform = 2f;
        Vector3 wordPos = new Vector3(pos.x, pos.y + heightAbovePlatform, pos.z);
        GameObject wordPoint = Instantiate(wordAnchorPrefab, wordPos, Quaternion.identity);
        wordPoint.transform.parent = wordAnchorParent;
    }
}
