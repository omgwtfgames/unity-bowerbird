using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class MusicManager : MonoBehaviour {
  public AudioClip[] tracks;
	public bool playOnStart = true;
	public bool parentToMainCamera = true;
	public int defaultTrack = 0;
	public float _volume = 1.0f;
	public float _pitch = 1.0f;
	
	// Static singleton property
    public static MusicManager Instance { get; private set; }
 
	public float volume {
		get { return _volume; }
		set { audio.volume = value;
			  _volume = value;
		    }
	}
	
	public float pitch {
		get { return _pitch; }
		set { audio.pitch = value;
			  _pitch = value;
		    }
	}

    void Awake() {
        // First we check if there are any other instances conflicting
        if(Instance != null && Instance != this) {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
			return;
        }
 
        // Here we save our singleton instance
        Instance = this;
 
        // Furthermore we make sure that we don't destroy between scenes (this is optional)
        DontDestroyOnLoad(gameObject);
			
		audio.clip = tracks[defaultTrack];
		
		GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
		GetComponent<AudioSource>().loop = true;
		GetComponent<AudioSource>().dopplerLevel = 0f;
    }
	
	void Start() {
		if (playOnStart) {
			Play();
		}
	}
	
	void OnLevelWasLoaded (int level) {
		if (parentToMainCamera) transform.parent = Camera.main.transform;
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
	
	public void SetVolume(float v) {
		volume = v;
	}
	
	public void SetPitch(float p) {
		pitch = p;
	}
}
