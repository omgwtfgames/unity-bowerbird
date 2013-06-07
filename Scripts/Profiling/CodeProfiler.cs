using UnityEngine;
using System.Collections.Generic;
 
// Simple code profiler class for Unity projects
// @robotduck 2011
// http://robotduck.wordpress.com/2011/08/05/code-optimization-in-unity-part-2/
//
// usage: place on an empty gameobject in your scene
// then insert calls to CodeProfiler.Begin(id) and
// CodeProfiler.End(id) around the section you want to profile
//
// "id" should be string, unique to each code portion that you're timing
// for example, in your enemy update function, you might have:
//
//     function Update {
//         CodeProfiler.Begin("Enemy:Update");
//         <the rest of your enemy update code here>
//         CodeProfiler.End("Enemy:Update");
//     }
//
// the Begin id and the End id must match exactly.
 
public class CodeProfiler : MonoBehaviour
{
    float startTime = 0;
    float nextOutputTime = 5;
    int numFrames = 0;
    static Dictionary<string, ProfilerRecording> recordings = new Dictionary<string, ProfilerRecording>();
    string displayText;
    Rect displayRect = new Rect(10,10,460,300);
     
    void Awake() {
        startTime = Time.time;  
        displayText = "\n\nTaking initial readings...";
    }
     
    void OnGUI() {
        GUI.Box(displayRect,"Code Profiler");
        GUI.Label(displayRect, displayText);
    }
     
    public static void Begin(string id) {
         
        // create a new recording if not present in the list
        if (!recordings.ContainsKey(id)) {
            recordings[id] = new ProfilerRecording(id);
        }
                 
        recordings[id].Start();
    }
     
    public static void End(string id) {
        recordings[id].Stop();
    }
     
    void Update() {
         
        numFrames++;
         
        if (Time.time > nextOutputTime)
        {
            // time to display the results      
             
             
            // column width for text display
            int colWidth = 10;
             
            // the overall frame time and frames per second:
            displayText = "\n\n";
            float totalMS = (Time.time-startTime)*1000;
            float avgMS = (totalMS/numFrames);
            float fps = (1000/(totalMS/numFrames));
            displayText += "Avg frame time: ";
            displayText += avgMS.ToString("0.#")+"ms, ";
            displayText += fps.ToString("0.#")+" fps \n";
 
            // the column titles for the individual recordings:
            displayText += "Total".PadRight(colWidth);
            displayText += "MS/frame".PadRight(colWidth);
            displayText += "Calls/fra".PadRight(colWidth);
            displayText += "MS/call".PadRight(colWidth);
            displayText += "Label";
            displayText += "\n";
                 
            // now we loop through each individual recording
            foreach(var entry in recordings)
            {
                // Each "entry" is a key-value pair where the string ID
                // is the key, and the recording instance is the value:
                ProfilerRecording recording = entry.Value;
                 
                // calculate the statistics for this recording:
                float recordedMS = (recording.Seconds * 1000);
                float percent = (recordedMS*100) / totalMS;
                float msPerFrame = recordedMS / numFrames;
                float msPerCall = recordedMS / recording.Count;
                float timesPerFrame = recording.Count / (float)numFrames;
                 
                // add the stats to the display text
                displayText += (percent.ToString("0.000")+"%").PadRight(colWidth);
                displayText += (msPerFrame.ToString("0.000")+"ms").PadRight(colWidth);
                displayText += (timesPerFrame.ToString("0.000")).PadRight(colWidth);
                displayText += (msPerCall.ToString("0.0000")+"ms").PadRight(colWidth);
                displayText += (recording.id);
                displayText += "\n";
 
                // and reset the recording
                recording.Reset();
            }
            Debug.Log(displayText); 
                 
            // reset & schedule the next time to display results:
            numFrames = 0;
            startTime = Time.time;
            nextOutputTime = Time.time + 5;
             
        }   
    }
}
 
 
// this is the ProfileRecording class which is simply included
// directly after the CodeProfiler class in the same file.
// The ProfileRecording class is basically for "internal use
// only" - you don't need to place it on a gameobject or interact
// with it in any way yourself, it's purely used by the
// CodeProfiler to do its job.
 
class ProfilerRecording
{
    // this class accumulates time for a single recording
     
    int count = 0;
    float startTime = 0;
    float accumulatedTime = 0;
    bool started = false;
    public string id;
     
    public ProfilerRecording(string id)
    {
        this.id = id;
    }
     
    public void Start() {
        if (started) { BalanceError(); }
        count++;
        started = true;
        startTime = Time.realtimeSinceStartup; // done last
    }
     
    public void Stop() {
        float endTime = Time.realtimeSinceStartup; // done first
        if (!started) { BalanceError(); }
        started = false;
        float elapsedTime = (endTime-startTime);
        accumulatedTime += elapsedTime;
    }
     
    public void Reset() {
        accumulatedTime = 0;
        count = 0;
        started = false;
    }
     
    void BalanceError() {
        // this lets you know if you've accidentally
        // used the begin/end functions out of order
        Debug.LogError("ProfilerRecording start/stops not balanced for '"+id+"'");  
    }
     
    public float Seconds {
        get { return accumulatedTime; }
    }
     
    public int Count {
        get { return count; }
    }
 
}