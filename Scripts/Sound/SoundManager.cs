using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SoundManager : MonoBehaviour {
	public bool soundOn = true;
	public bool parentToMainCamera = true;
	public float soundVolume = 1.0f;
	//public float musicVolume = 1.0f;
	public string[] soundNames;
	public AudioClip[] sounds;
	Dictionary<string,AudioClip> soundMap = new Dictionary<string,AudioClip>();
	
	// We essentially create a static object pool, with a set number of
	// AudioSources ('channels'). We cycle through these each time we play
	// a sound - this will reduce the change of one sample somehow clobbering
	// another, and allows one channel to play at a different pitch without
	// disrupting other playing samples.
	public int numberOfChannels;
	private AudioSource[] channels;
	private int channelIndex = 0;
		
	// Static singleton property
  public static SoundManager Instance { get; private set; }
	
	public void Awake () {
	   /// singleton stuff ///////
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
	  /////////////////////////////
		
	  if (numberOfChannels == null) {
	  	numberOfChannels = 8;
	  }
	  channels = new AudioSource[numberOfChannels];
	  for (int i = 0; i < soundNames.Length; i++) {
	  	soundMap.Add(soundNames[i], sounds[i]);
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
		a.dopplerLevel = 0f;
	  	c.audio.loop = false;
	  	channels[ii] = c.audio;
	  }
	          
	  DontDestroyOnLoad(transform.gameObject);
	}
	
	void OnLevelWasLoaded (int level) {
		if (parentToMainCamera) transform.parent = Camera.main.transform;
	}
	
	public void Start () {
	
	}
	
	public void Update () {
	
	}
	
	public void Play(string soundname) {
		Play(soundname, soundVolume, 1.0f);
	}
	
	public void Play(string soundname, float volume, float pitch) {
	  if (soundOn) {
	    while (channels[channelIndex].isPlaying) {
	      incrementChannelIndex();
	    }
	    channels[channelIndex].pitch = pitch;
	    channels[channelIndex].volume = volume;
	  	channels[channelIndex].PlayOneShot(soundMap[soundname], soundVolume);
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
}
