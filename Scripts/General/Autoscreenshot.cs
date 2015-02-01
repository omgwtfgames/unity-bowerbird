using UnityEngine;
using System.Collections;

public class Autoscreenshot : MonoBehaviour {
 
    public string screenshotDirectory = "Autoscreenshots";
    string localDir = "";
    string basepath = "";
    public string suffix = "Screenshot ";
    int startNumber = 1;
    public float minDelay = 2f;
    public float maxDelay = 10f;
    float timer = 0f;
    float currentDelay = 2f;
	public KeyCode manualTriggerKey = KeyCode.BackQuote;

    // Static singleton property
    public static Autoscreenshot Instance { get; private set; }
 
    public void Awake() {
     #if !UNITY_EDITOR
     Destroy(gameObject);
     return;
     #endif
     
        /// singleton ///
        if (Instance != null && Instance != this) {
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;
		DontDestroyOnLoad(gameObject);
        localDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
        #if UNITY_EDITOR
        localDir = Application.dataPath;
        #endif
        basepath = System.IO.Path.Combine(localDir, screenshotDirectory);
        if (!System.IO.Directory.Exists(basepath)) {
            System.IO.Directory.CreateDirectory(basepath);
        }
    }
 
    void Start() { }
 
    void Update() {
        timer += Time.deltaTime;
        if (timer > currentDelay) {
            Snap();
            timer = 0f;
            currentDelay = Random.Range(minDelay, maxDelay);
        }
		if (Input.GetKeyDown(manualTriggerKey)) {
			Snap();
		}
    }
 
    public void Snap() {
        int number = startNumber;
        string date = System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-FFF ");
        string filepath = System.IO.Path.Combine(basepath, date + suffix + number.ToString() + ".png");
 
        startNumber = number + 1;
 
        Application.CaptureScreenshot(filepath);
        Debug.Log("Wrote screenshot: " + filepath);
    }
}
