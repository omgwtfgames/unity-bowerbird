unity-bowerbird
===============

*A Bowerbird's Nest of Unity3D scripts*

A set of resusable scripts and simple assets for Unity3D.

These are things I find myself using over and over again, so I thought I'd collect them in one place. I hope to continually evolve this into what I would regard as "the missing Standard Assets".

This collection couldn't exist without the hard work of many others freely sharing their code. What you find here is a combination of stuff I've written and code freely shared by others on the Unify Community wiki (http://wiki.unity3d.com/index.php/Main_Page), the Unity forums (http://forum.unity3d.com/forum.php) and around the web. I've linked back to the original source at the top of each script, where appropriate. 

All code *should* be free for commercial use in games without attribution - but check the license or URL at the top individual script to be sure.

Contents
--------

### Scripts

#### General

* ActivatedInEditorOnly.cs - inactivates the attached GameObject unless we are running in the Editor. Good for debugging overlays and things.
* DestroyOnAwakeExceptInEditor.cs - useful for the situation where you have a DontDestroyOnLoad object in your first scene  but need to maintain a copy of it in your second scene for testing. This ensures in real builds that test object won't be around. A using the Singleton pattern (below) for your DontDestroyOnLoad object is probably a better option.
* DestroyAfter.cs - destroys or inactivates the attached GameObject a certain delay. Can optionally detach children before self destructing.

* Dictionary - contains some serializable dictionary implementations. I haven't really used these, but I believe the idea is that they might show up in the Inspector unlike System.Collections.Generic.Dictionary.
* CombineChildren.cs - dug out from the Unity Standard Assets - combines meshes of the attached GameObject and it's children as a runtime optimization. Requires MeshCombineUtility.cs in the project (but not attached to the GameObject).
* DontGoThroughThings.cs - attempts to prevent fast moving rigidbodies from passing through things by using a Raycast to look for collisions that the Physics timestep might have stepped over. YMMV.
* GeometryUtils.cs - functions to find the mesh bounding box of a GameObject (including children), and find the geometric centre of a GameObject and it's children (ignoring mesh shape).
* Md5.js - the hash function.
* PlayerPrefsX.js - allows additional data types to be stored in PlayerPrefs (eg Vector3, Quaternions, bool, Color, various arrays).
* Singleton_DontUseThisDirectly_Cut_n_Paste.cs - an example of the Singleton pattern for a subclass of MonoBehaviour. Use this if you have a GameObject that you only ever want one central instance of (eg a game manager script in the first scene, set to DontDestroyOnLoad).

#### GUI
* FPSngui.cs - attach this to an NGUI Label (UILabel) to turn it into an FPS counter.
* ScaledRect.js - returns a rectangle scaled based on the current screen resolution. You can use this to scale the native Unity GUI across different resolutions. Or just use NGUI and not deal with all the hassle.

#### Input
* UInput.js - a wrapper of Unity's Input class that allows you to also Set and Pop axis values. This can be useful for hooking up 'virtual' touchscreen controllers since it allows you to then use the same code path for joysticks, keyboard and touch screen (see notes in script).

#### Sound
* SoundManager.js - fill up the sounds array with AudioClips and the soundNames array with corresponding strings, and you can fire off sounds like SoundManager.Play("Powerup"); . Has a global volume setting and can play multiple sounds simultaneously at different pitches and volumes.

* MusicManager.cs - a simple background music manager that allows fadein/fadeout and dynamic pitch changes.

#### Tweening
The iTween (http://itween.pixelplacement.com/documentation.php) and LeanTween (https://github.com/dentedpixel/LeanTween) libraries.

#### AI
* FSMSystem.cs - a finite state machine suitable for simple AI on NPCs. I've also used it for managing other high level game states (eg transitions between player turns in a turn based game). Docs: http://wiki.unity3d.com/index.php?title=Finite_State_Machine

#### Web
* KongregateAPI.cs - a simple class for interacting with the Kongregate stats API.
* SiteLock.cs - attempts to prevent sites stealing your Web Player game and hosting it themselves - attach it to a GameObject in your first scene and fill out the domain and redirectUrl fields. Very likely to be easily hacked.

### Meshes

Some primitive meshes not part of the standard Unity set.


### Shaders
A few useful shaders not found in Unity by default.
Also contains Madfingers mobile-optimized Shadowgun shaders which they kindly released to the community.

TODO
----

There is lots of redundency in Scripts/General. Merge ActivatedInEditorOnly, DestroyOnAwakeExceptInEditor, DestroyOnAwakwe and DestroyAfter, into one all-singing-and-dancing script and also allow actions based on Debug.isDebugBuild. Consider that the EditorOnly tag makes some of the functionality here redundant. Should probably use OnEnable instead of Awake, so a disabled script can be enabled to start timers and destroy.

Consider allowing MusicManager to play a track using a string name (as well as track index).

Make SiteLock.cs work with a list of domains, not just one.

Test and add someones object pooling class, eg  http://forum.unity3d.com/threads/76851-Simple-Reusable-Object-Pool-Help-limit-your-instantiations! or http://www.booncotter.com/unity-prefactory-a-better-pool-manager/ or http://vonlehecreative.wordpress.com/2010/01/06/unity-resource-gameobjectpool/ or based on http://www.third-helix.com/2011/03/unity-recycling-object-instances/ .

Add an example scene with some common prefabs (eg SoundManager, MusicManager, FPS counter) setup ready to go.
