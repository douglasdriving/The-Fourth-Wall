using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FirstPersonController))]
public class PlayerMoveHolder : MonoBehaviour
{
  [SerializeField] float maxTimeBetweenSpacePresses = 0.2f;
  [SerializeField] int spacePressesRequired = 10;
  private FirstPersonController firstPersonController;
  private float timeSinceSpacePressed = 0;
  private int spacePressedCount = 0;
  bool isHoldingPlayer = false;

  private void Awake()
  {
    firstPersonController = GetComponent<FirstPersonController>();
  }

  private void Update()
  {
    if (!isHoldingPlayer) return;
    timeSinceSpacePressed += Time.deltaTime;

    if (Keyboard.current.spaceKey.wasPressedThisFrame)
    {
      spacePressedCount++;
      timeSinceSpacePressed = 0;
    }

    if (timeSinceSpacePressed > maxTimeBetweenSpacePresses)
    {
      spacePressedCount = 0;
    }

    if (spacePressedCount >= spacePressesRequired)
    {
      ReleasePlayer();
    }
  }

  private void ReleasePlayer()
  {
    firstPersonController.canMove = true;
    firstPersonController.enableJump = true;
    isHoldingPlayer = false;
  }

  public void HoldPlayer()
  {
    firstPersonController.canMove = false;
    firstPersonController.enableJump = false;
    isHoldingPlayer = true;
  }
}