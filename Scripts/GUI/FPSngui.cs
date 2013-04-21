using UnityEngine;
using System.Collections;

public class FPSngui : MonoBehaviour {

// Modified from: http://wiki.unity3d.com/index.php?title=FramesPerSecond

// Attach this to an NGUI Label to make a frames/second indicator.
//
// It calculates frames/second over each updateInterval,
// so the display does not keep changing wildly.
//
// It is also fairly accurate at very low FPS counts (<10).
// We do this not by simply counting frames per interval, but
// by accumulating FPS for each frame. This way we end up with
// correct overall FPS even if the interval renders something like
// 5.5 frames.
 
public float updateInterval = 1f;
public bool developmentBuildsOnly = false;

private float accum = 0.0f; // FPS accumulated over the interval
private int frames = 0; // Frames drawn over the interval
private float timeleft; // Left time for current interval
private UILabel nguiLabel;

void Awake() {
    if (developmentBuildsOnly) {
        gameObject.SetActive(Debug.isDebugBuild);
    }
    nguiLabel = GetComponent<UILabel>();
}

void Start()
{
    if( !nguiLabel )
    {
        Debug.Log("FramesPerSecond needs a to be attached to an NGUI Label !");
        enabled = false;
        return;
    }
    timeleft = updateInterval;  
}
 
void Update()
{
    timeleft -= Time.deltaTime;
    accum += Time.timeScale/Time.deltaTime;
    ++frames;
 
    // Interval ended - update GUI text and start new interval
    if( timeleft <= 0.0f )
    {
        // display two fractional digits (f2 format)
        nguiLabel.text = "" + (accum/frames).ToString("f2");
        timeleft = updateInterval;
        accum = 0.0f;
        frames = 0;
    }
}
}
