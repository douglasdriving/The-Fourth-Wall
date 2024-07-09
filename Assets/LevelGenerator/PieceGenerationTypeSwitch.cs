using UnityEngine;

public class PieceGenerationTypeSwitch : MonoBehaviour
{
    [SerializeField] LevelPieceType levelPieceTypeToSwitchTo;

    public void Switch()
    {
        LevelGenerator.pieceTypeBeingGenerated = levelPieceTypeToSwitchTo;
    }
}
