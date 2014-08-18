using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SoundManager : MonoBehaviour {
	public bool soundOn = true;
	public enum CameraFollowMode { None, Follow, Child };
	public CameraFollowMode cameraFollowMode = CameraFollowMode.Follow;
	public float volume = 1.0f;
	public string preferenceName = "sfx_volume";
	public bool initialVolumeFromPreference = true;
	public bool useFilenameAsSoundName = false;
    public List<string> soundNames;
	public AudioClip[] sounds;
	
	Dictionary<string,AudioClip> soundMap = new Dictionary<string,AudioClip>();
	
	// we keep tally of how many sfx of a particular name are playing simultaneously
	// and don't play more than a certain number at a time (for efficiency doesn't 
	// account for sounds that are manually stopped, assumes they play for the full duration
	// after being triggered)
	public int maxSimlutSfxOfOneType = 4;
	Dictionary<string,int> currentlyPlayingCount = new Dictionary<string,int>();
	
	// We essentially create a static object pool, with a set number of
	// AudioSources ('channels'). We cycle through these each time we play
	// a sound - this will reduce the change of one sample somehow clobbering
	// another, and allows one channel to play at a different pitch without
	// disrupting other playing samples.
	public int numberOfChannels = 2;
	private AudioSource[] channels;
	private int channelIndex = 0;

	// Static singleton property
  	public static SoundManager Instance { get; private set; }
	
	public void Awake () {
	    /// singleton stuff ///////
	    // First we check if there are any other instances conflicting
	    if (Instance != null && Instance != this) {
	        // If that is the case, we destroy this instance
	        Destroy(gameObject);
			return;
	    }
	
	    // Here we save our singleton instance
	    Instance = this;
	
	    // Furthermore we make sure that we don't destroy between scenes (this is optional)
	    DontDestroyOnLoad(gameObject);
	  /////////////////////////////
		
	  if (numberOfChannels == null) {
	  	numberOfChannels = 8;
	  }
	  channels = new AudioSource[numberOfChannels];
	  for (int i = 0; i < sounds.Length; i++) {
	  	if (useFilenameAsSoundName) {
			soundNames.Add(sounds[i].name);
			soundMap.Add(sounds[i].name, sounds[i]);
		} else {
			soundMap.Add(soundNames[i], sounds[i]);
		}
		currentlyPlayingCount.Add(soundNames[i], 0);
	  }
	  /*
	  soundMap = {"Die": sounds[0],
	            "BeamIn" : sounds[1],
	            "PowerupPickup": sounds[2] 
	            };
	  */
	  		
	  // generate attached gameobjects "Channel1", "Channel2" with AudioSources
	  // accessible in the channel array
	  for (int ii = 0; ii < numberOfChannels; ii++) {
	    	GameObject c = new GameObject();
	    	c.name = "Channel"+ii;
	    	c.transform.parent = transform;
	    	AudioSource a = c.AddComponent<AudioSource>();
	    	a.rolloffMode = AudioRolloffMode.Linear;
		  a.panLevel = 0f;
		  a.dopplerLevel = 0f;
	    	c.audio.loop = false;
	    	channels[ii] = c.audio;
	  }
			
	  DontDestroyOnLoad(transform.gameObject);
	
		if (cameraFollowMode == CameraFollowMode.Follow) {
			transform.position = Camera.main.transform.position;
		}
		else if (cameraFollowMode == CameraFollowMode.Child) {
			transform.parent = Camera.main.transform;
		} 
		
	  if (initialVolumeFromPreference) volume = GetVolumePreference();
	}
	
	void OnLevelWasLoaded (int level) {
		if (cameraFollowMode == CameraFollowMode.Follow) {
			transform.position = Camera.main.transform.position;
		}
		else if (cameraFollowMode == CameraFollowMode.Child) {
			transform.parent = Camera.main.transform;
		} 
	}
	
	public void Start () {
	
	}
	
	public void Update () {
		if (cameraFollowMode == CameraFollowMode.Follow) {
			transform.position = Camera.main.transform.position;
		}
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
	
	public void Play(string soundname) {
		Play(soundname, volume, 1.0f);
	}
	
	public void Play(string soundname, float volume, float pitch) {
	  if (!soundMap.ContainsKey(soundname)) {
		  Debug.LogWarning("SoundManager: Tried to play undefined sound: " + soundname);
		  return;
	  }
	  if (soundOn) {
		if (currentlyPlayingCount[soundname] > maxSimlutSfxOfOneType) {
			return;
		}
		int channelsPlayingCount = 0;
	    while (channels[channelIndex].isPlaying) {
	      incrementChannelIndex();
		  channelsPlayingCount++;
	    }
	    // skip playing this effect if all channels are in use
		if (channelsPlayingCount >= numberOfChannels) {
			return;
		}
	    channels[channelIndex].pitch = pitch;
	    channels[channelIndex].volume = volume;
	  	channels[channelIndex].PlayOneShot(soundMap[soundname], volume);
		currentlyPlayingCount[soundname] += 1;
		StartCoroutine("decrementPlayCountInFuture", soundname);
	  	incrementChannelIndex();
	  }
	}
	
	public void Play(string soundname, float volume) {
		Play(soundname, volume, 1.0f);
	}
	
	private void incrementChannelIndex() {
	  	if (channelIndex < channels.Length - 1) {
	  	   channelIndex++;
	  	} else {
	  	   channelIndex = 0;
	  	}
	}
	
	private IEnumerator decrementPlayCountInFuture(string soundname) {
		if (Time.timeScale > 0) {
			yield return new WaitForSeconds(soundMap[soundname].length);
		}
		if (currentlyPlayingCount[soundname] > 0) currentlyPlayingCount[soundname] -= 1;
	}
}
