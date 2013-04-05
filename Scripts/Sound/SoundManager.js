#pragma downcast
//@script RequireComponent(AudioSource)

var soundOn : boolean = true;
var soundVolume : float = 1.0;
var musicVolume : float = 1.0;
var soundNames : String[];
var sounds : AudioClip[];
static var soundMap : Hashtable = new Hashtable();

// We essentially create a static object pool, with a set number of
// AudioSources ('channels'). We cycle through these each time we play
// a sound - this will reduce the change of one sample somehow clobbering
// another, and allows one channel to play at a different pitch without
// disrupting other playing samples.
public var numberOfChannels : int;
private var channels : AudioSource[];
private var channelIndex : int = 0;

// A dedicated background music channel
// (TODO: probably really want a pool of two of these to allow cross fading between two tracks)
private var musicChannel : AudioSource;

function Awake () {
  if (numberOfChannels == null) {
  	numberOfChannels = 8;
  }
  channels = new AudioSource[numberOfChannels];
  for (var i : int = 0; i < soundNames.Length; i++) {
    try {
      soundMap.Add(soundNames[i], sounds[i]);
    } catch (err) {}
  }
  /*
  soundMap = {"Die": sounds[0],
            "BeamIn" : sounds[1],
            "PowerupPickup": sounds[2] 
            };
  */
  
  //musicChannel = transform.Find("MusicChannel").GetComponentInChildren(typeof(AudioSource));
  var musicChannelGO : GameObject = new GameObject();
  musicChannelGO.name = "MusicChannel";
  musicChannelGO.transform.parent = transform;
  musicChannel = musicChannelGO.AddComponent(AudioSource);
  musicChannel.audio.loop = true;
  
  // generate attached gameobjects "Channel1", "Channel2" with AudioSources
  // accessible in the channel array
  for (var ii : int = 0; ii < numberOfChannels; ii++) {
  	var c : GameObject = new GameObject();
  	c.name = "Channel"+ii;
  	c.transform.parent = transform;
  	c.AddComponent(AudioSource);
  	c.audio.loop = false;
  	channels[ii] = c.audio;
  }
          
  DontDestroyOnLoad(transform.gameObject);
}

function Start () {

}

function Update () {

}

function Play(soundname : String, volume : float, pitch : float) {
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

function Play(soundname : String) {
	Play(soundname, soundVolume, 1.0);
}

function Play(soundname : String, volume : float) {
	Play(soundname, volume, 1.0);
}

private function incrementChannelIndex() {
  	if (channelIndex < channels.Length - 1) {
  	   channelIndex++;
  	} else {
  	   channelIndex = 0;
  	}
}

// TODO: function to switch between two music tracks at different speed.
// Use 
// var percent_played : float = GetComponent(AudioSource).timeSamples / GetComponent(AudioSource).clip.samples
// GetComponent(AudioSource).clip.soundMap["FasterMusic"];
// GetComponent(AudioSource).timeSamples = percent_played * GetComponent(AudioSource).clip.samples


function SwitchToMusic(soundname : String) {
  if (soundOn) {
    var percent_played : float = (1.0*musicChannel.timeSamples) / (1.0*musicChannel.clip.samples);
    musicChannel.clip = soundMap[soundname];
    musicChannel.volume = musicVolume;
    musicChannel.loop = true;
  	//musicChannel.Play(percent_played * musicChannel.clip.samples);
  	musicChannel.timeSamples = parseInt(percent_played * musicChannel.clip.samples);
  	musicChannel.Play();
  	Debug.Log(soundname + ": " + percent_played);
  }
}

function PlayMusic(soundname : String) {
  if (soundOn) {
    musicChannel.volume = musicVolume;
    musicChannel.pitch = 1.0;
    musicChannel.clip = soundMap[soundname];
    musicChannel.loop = true;
  	musicChannel.Play();
  }
}

function PlayMusic(soundname : String, volume : float, pitch : float) {
  if (soundOn) {
    musicChannel.volume = Mathf.Min(musicVolume, volume);
    musicChannel.pitch = pitch;
    musicChannel.clip = soundMap[soundname];
    musicChannel.loop = true;
  	musicChannel.Play();
  }
}

function PauseMusic() {
	musicChannel.Pause();
}

function StopMusic() {
	musicChannel.Stop();
}
