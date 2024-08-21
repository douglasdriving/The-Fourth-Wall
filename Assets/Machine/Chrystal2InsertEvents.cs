using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class Chrystal2InsertEvents : MonoBehaviour
{
  [SerializeField] GameObject floatingVideo;
  [SerializeField] GameObject roadToChrystal3;
  VideoPlayer videoPlayer;

  void Awake()
  {
    videoPlayer = floatingVideo.GetComponentInChildren<VideoPlayer>();
  }

  public void StartEvents()
  {
    floatingVideo.SetActive(true);
    floatingVideo.GetComponent<VideoPositioner>().PositionVideoInFrontOfPlayer();
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
}