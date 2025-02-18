using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class TalkingHead : MonoBehaviour
{
    [SerializeField] float distanceToWalkway = 3f;
    [SerializeField] Vector3 defaultRotation = new Vector3(0, -22.5f, 0);
    [SerializeField] float randomRotationRange = 10f;
    [SerializeField] float hideVideoUntil = -1;
    [SerializeField] GameObject videoCanvas;
    [SerializeField] GameObject talkingHeadModel;
    public Transform mouth;
    bool isShowingVideo = false;

    void Start()
    {
        if (hideVideoUntil >= 0)
        {
            StartCoroutine(StartVideoAfterSeconds(hideVideoUntil));
        }
    }

    IEnumerator StartVideoAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ShowVideoAndHideModel();
    }

    private void ShowVideoAndHideModel()
    {
        isShowingVideo = true;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        videoCanvas.SetActive(true);
        talkingHeadModel.SetActive(false);
    }

    public void MoveToEndOfWalkway(GameObject lastLevelPiece)
    {
        Vector3 finalWalkoffPoint = lastLevelPiece.GetComponent<LevelPiece.Positioner>().GetFinalWalkOffPoint();
        Vector3 targetPosition = finalWalkoffPoint + new Vector3(-1, 1, 0) * distanceToWalkway;
        transform.position = targetPosition;
        if (!isShowingVideo) SetRandomRotation();
    }

    private void SetRandomRotation()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-randomRotationRange, randomRotationRange),
            Random.Range(-randomRotationRange, randomRotationRange),
            Random.Range(-randomRotationRange, randomRotationRange)
        );
        transform.rotation = Quaternion.Euler(defaultRotation + randomOffset);
    }
}
