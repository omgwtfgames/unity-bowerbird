using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class MusicManager : MonoBehaviour {
    public AudioClip[] tracks;
    public bool playOnStart = true;
    public enum CameraFollowMode {
        None,
        Follow,
        Child
    };
    public CameraFollowMode cameraFollowMode = CameraFollowMode.Follow;
    public int defaultTrack = 0;
    public string preferenceName = "music_volume";
    public bool initialVolumeFromPreference = true;
    public float _volume = 1.0f;
    public float _pitch = 1.0f;
	public bool ScaleOutputVolume = true;

    // Static singleton property
    public static MusicManager Instance { get; private set; }
 
    public float volume {
        get { return _volume; }
        set {
            if (ScaleOutputVolume) {
				audio.volume = ScaleVolume(value);
			} else {
				audio.volume = value;
			}
            _volume = value;
        }
    }
 
    public float pitch {
        get { return _pitch; }
        set {
            audio.pitch = value;
            _pitch = value;
        }
    }
 
    void Awake() {
        // First we check if there are any other instances conflicting
        if (Instance != null && Instance != this) {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
            return;
        }
 
        // Here we save our singleton instance
        Instance = this;
 
        // Furthermore we make sure that we don't destroy between scenes (this is optional)
        DontDestroyOnLoad(gameObject);
     
        if (cameraFollowMode == CameraFollowMode.Follow) {
            if (Camera.main != null)
                transform.position = Camera.main.transform.position;
        } else if (cameraFollowMode == CameraFollowMode.Child) {
            if (Camera.main != null)
                transform.parent = Camera.main.transform;
        } 
     
        audio.clip = tracks[defaultTrack];
        volume = _volume;
        if (initialVolumeFromPreference)
            volume = GetVolumePreference();
     
        AudioSource audioSrc = GetComponent<AudioSource>();
        audioSrc.rolloffMode = AudioRolloffMode.Linear;
        audioSrc.loop = true;
        audioSrc.dopplerLevel = 0f;
        audioSrc.panLevel = 0f;
    }
 
    void Start() {
        if (playOnStart) {
            Play();
        }
    }
 
    void OnLevelWasLoaded(int level) {
        if (cameraFollowMode == CameraFollowMode.Follow) {
            if (Camera.main != null)
                transform.position = Camera.main.transform.position;
        } else if (cameraFollowMode == CameraFollowMode.Child) {
            if (Camera.main != null)
                transform.parent = Camera.main.transform;
        } 
    }
 
    void Update() {
        if (cameraFollowMode == CameraFollowMode.Follow) {
            if (Camera.main != null)
                transform.position = Camera.main.transform.position;
        }
    }
 
    public void PlayTrack(int i) {
        audio.Stop();
        audio.clip = tracks[i];
        audio.Play();
    }
 
    public void Pause() {
        audio.Pause();
    }
 
    public void Play() {
        audio.Play();
    }
 
    public void Stop() {
        audio.Stop();
    }
 
    public void Fade(float targetVolume, float fadeTime) {
        LeanTween.value(gameObject, "SetVolume", volume, targetVolume, fadeTime);
    }
 
    public void FadeOut(float fadeTime) {
        StartCoroutine(FadeOutAsync(fadeTime));
    }
 
    IEnumerator FadeOutAsync(float fadeTime) {
        Fade(0f, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        Pause();
    }
 
    public void FadeIn(float fadeTime) {
        Play();
        Fade(1.0f, fadeTime);
    }
 
    public void SlidePitch(float targetPitch, float fadeTime) {
        LeanTween.value(gameObject, "SetPitch", pitch, targetPitch, fadeTime);
    }
 
    public float GetVolumePreference() {
        return PlayerPrefs.GetFloat(preferenceName, volume);
    }
 
    public void SaveCurrentVolumePreference() {
        SaveVolumePreference(volume);
    }
 
    public void SaveVolumePreference(float v) {
        PlayerPrefs.SetFloat(preferenceName, v);
        PlayerPrefs.Save();
    }
 
    public void SetPitch(float p) {
        pitch = p;
    }

	// TODO: we should consider using this dB scale as an option when porting these changes 
	//       over to unity-bowerbird: http://wiki.unity3d.com/index.php?title=Loudness
	/*
	 *   Quadratic scaling of actual volume used by AudioSource. Approximates the proper exponential.
	 */
	public float ScaleVolume(float v) {
		v = Mathf.Pow(v, 4);
		return Mathf.Clamp(v, 0f, 1f);
	}
}
