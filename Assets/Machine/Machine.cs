using UnityEngine;
using UnityEngine.Video;

public class Machine : MonoBehaviour
{
  [SerializeField] Chrystal2InsertEvents chrystal2InsertEvents;
  [SerializeField] VideoClip demoEndVideo;
  [SerializeField] GameObject floatingVideoPlayer;
  int chrystalsInserted = 0;

  public void AddChrystal()
  {
    chrystalsInserted++;
    if (chrystalsInserted == 2)
    {
      chrystal2InsertEvents.StartEvents();
    }
    else if (chrystalsInserted == 3)
    {
      PlayDemoEndVideo();
    }
  }

  private void PlayDemoEndVideo()
  {
    floatingVideoPlayer.SetActive(true);
    floatingVideoPlayer.GetComponent<VideoPositioner>().PositionVideoInFrontOfPlayer();
    VideoPlayer videoPlayer = floatingVideoPlayer.GetComponentInChildren<VideoPlayer>();
    videoPlayer.clip = demoEndVideo;
    videoPlayer.Play();
  }
}