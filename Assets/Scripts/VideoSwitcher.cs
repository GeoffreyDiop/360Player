using System.Collections;
using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class VideoSwitcher : MonoBehaviour
{
    [SerializeField] private MediaPlayer mediaPlayer;
    [SerializeField] private Text videoName;
    [SerializeField] private Text NextText;
    [Space]
    [SerializeField] private List<Video> videoList;

    protected Image BackgroundTextName;
    protected int index = -1;
    protected bool errorCached = false;
    // Start is called before the first frame update
    void Start()
    {
        BackgroundTextName = videoName.transform.parent.GetComponent<Image>();
        BackgroundTextName.gameObject.SetActive(false);

        mediaPlayer.Events.AddListener(OnMediaPlayerEvent);
        LoadNextVideo();
    }

    public void LoadNextVideo()
    {
        index++;
        if (index >= videoList.Count)
        {
            index = 0;
        }
        NextText.text = (index + 1) + "/" + videoList.Count + " >>";
        StopAllCoroutines();
        mediaPlayer.CloseVideo();

        if (videoList[index].streaming)
        {
            mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, videoList[index].path, true);
        }
        else
        {
            mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToPersistentDataFolder, videoList[index].path, true);
        }
        BackgroundTextName.gameObject.SetActive(true);
        BackgroundTextName.color = Color.grey;
        errorCached = false;
        StartCoroutine(LoadingAnimation());
    }

    IEnumerator LoadingAnimation()
    {
        while (true)
        {
            videoName.text = videoList[index].vName + " Loading .";
            yield return new WaitForSeconds(0.3f);
            videoName.text = videoList[index].vName + " Loading ..";
            yield return new WaitForSeconds(0.3f);
            videoName.text = videoList[index].vName + " Loading ...";
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void OnMediaPlayerEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
    {
        switch (et)
        {
            case MediaPlayerEvent.EventType.FirstFrameReady:
                StopAllCoroutines();
                BackgroundTextName.color = Color.green;
                videoName.text = videoList[index].vName;
                break;
            //case MediaPlayerEvent.EventType.FinishedPlaying:
            //    BackgroundTextName.gameObject.SetActive(false);
            //    LoadNextVideo();
            //    break;
            case MediaPlayerEvent.EventType.Error:
                StopAllCoroutines();
                BackgroundTextName.color = Color.red;
                videoName.text = videoList[index].vName + " - [AVProVideo] Error: " + Helper.GetErrorMessage(errorCode);
                break;
        }

    }
}
