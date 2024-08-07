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
    PositionVideoInFrontOfPlayer();
    videoPlayer.Play();
    ActivateRoadAfterVideoTime();
  }

  void ActivateRoadAfterVideoTime()
  {
    float videoLength = (float)videoPlayer.length;
    StartCoroutine(ActivateRoadToChrystal3AfterDelay(videoLength));

    IEnumerator ActivateRoadToChrystal3AfterDelay(float delay)
    {
      yield return new WaitForSeconds(delay);
      roadToChrystal3.SetActive(true);
      //does this need to also be saved in the level generator, so that we can continue from there?
      //maybe when we hit the narration trigger we can do that
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