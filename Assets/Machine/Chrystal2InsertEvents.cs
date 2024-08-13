using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class Chrystal2InsertEvents : MonoBehaviour
{
  [SerializeField] GameObject floatingVideo;
  [SerializeField] GameObject roadToChrystal3;
  [SerializeField] float planeDistanceFromVideoToPlayer = 15f;
  [SerializeField] float videoHeightAbovePlayer = 5f;
  VideoPlayer videoPlayer;

  void Awake()
  {
    videoPlayer = floatingVideo.GetComponentInChildren<VideoPlayer>();
  }

  public void StartEvents()
  {
    floatingVideo.SetActive(true);
    PositionVideoInFrontOfPlayer();
    videoPlayer.Play();
    ActivateRoadAndHideVideoAfterVideoTime();
  }

  void ActivateRoadAndHideVideoAfterVideoTime()
  {
    float videoLength = (float)videoPlayer.length;
    StartCoroutine(ActivateRoadToChrystal3AndHideVideoAfterDelay(videoLength));

    IEnumerator ActivateRoadToChrystal3AndHideVideoAfterDelay(float delay)
    {
      yield return new WaitForSeconds(delay);
      roadToChrystal3.SetActive(true);
      floatingVideo.gameObject.SetActive(false);
    }
  }

  void PositionVideoInFrontOfPlayer()
  {
    Transform player = GameObject.FindWithTag("Player").transform;
    Vector3 playerForwardDir = player.forward;
    playerForwardDir.y = 0;
    playerForwardDir.Normalize();
    Vector3 videoPos = player.position + (playerForwardDir * planeDistanceFromVideoToPlayer) + (Vector3.up * videoHeightAbovePlayer);
    Vector3 videoForward = (player.position - videoPos).normalized;

    floatingVideo.transform.position = videoPos;
    floatingVideo.transform.forward = videoForward;
  }
}