using System.Collections;
using UnityEngine;

/// <summary>
/// allows a piece to be moved to a specific pos and rot over a set time
/// </summary>
public class AnimatedPieceMover : MonoBehaviour
{

    public Vector3 targetPos;
    public Quaternion targetRot;

    public void StartMovingToPosAndRot(Vector3 pos, Quaternion rot, float time)
    {
        targetPos = pos;
        targetRot = rot;
        StartCoroutine(MovePieceToPosAndRotCoroutine(pos, rot, time));
    }

    IEnumerator MovePieceToPosAndRotCoroutine(Vector3 pos, Quaternion rot, float time)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startPos, pos, elapsedTime / time);
            transform.rotation = Quaternion.Lerp(startRot, rot, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = pos;
        transform.rotation = rot;
    }
}
