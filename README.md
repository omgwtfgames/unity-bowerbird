unity-bowerbird
===============

*A Bowerbird's Nest of Unity3D scripts*

A set of resusable scripts for Unity3D. Useful for getting started quickly in gamejams such as Ludum Dare, where publically available 'basecode' is permissible. 

These are things I find myself using over and over again, so I thought I'd collect them in one place.

This collection couldn't exist without the hard work of many others freely sharing their code. What you find here is a combination of stuff I've written and code freely shared by others on the Unify Community wiki (http://wiki.unity3d.com/index.php/Main_Page), the Unity forums (http://forum.unity3d.com/forum.php) and around the web. I've linked back to the original source at the top of each script, where appropriate.

All code *should* be free for commercial use in games without attribution - but check the license or URL at the top of each individual script to be sure.

You can download the packaged version for easy import into Unity here: https://github.com/omgwtfgames/unity-bowerbird/raw/master/Packages/unity-bowerbird.unitypackage

Some parts of this package may require NGUI - if you don't have it, you'll also need the [NGUI Distribution package](http://www.tasharen.com/get.php?file=NGUIDistro) (version 2.7.0 is now [free to use with some restrictions](http://forum.unity3d.com/threads/ngui-free-edition.124032/))

Contents
--------

### Scripts

#### General
* Autoscreenshot.cs - save randomly timed screenshots when playing (at the cost of occasional framerate stuttering). Play your game in the Editor, then review what you got and pluck out the good ones.
* CombineChildren.cs - dug out from the Unity Standard Assets - combines meshes of the attached GameObject and it's children as a runtime optimization. Requires MeshCombineUtility.cs in the project (but not attached to the GameObject).
* DestroyAfter.cs - destroys or inactivates the attached GameObject a certain delay. Can optionally detach children before self destructing.
* Dictionary - contains a Serializable dictionary implementation by Calvin Rien. Unlike the standard System.Collections.Generic.Dictionary, ObjectDictionary shows up in the Inspector.
* DontGoThroughThings.cs - attempts to prevent fast moving rigidbodies from passing through things by using a Raycast to look for collisions that the Physics timestep might have stepped over. YMMV.
* GeometryUtils.cs - functions to find the mesh bounding box of a GameObject (including children), and find the geometric centre of a GameObject and it's children (ignoring mesh shape).
* LayerMaskBuilder.cs - a nicer API for manipulating LayerMasks, by Akilram Krishnan.
* Md5.cs - the hash function.
* PlayerPrefsX.js - allows additional data types to be stored in PlayerPrefs (eg Vector3, Quaternions, bool, Color, various arrays).
* Singleton_DontUseThisDirectly_Cut_n_Paste.cs - an example of the Singleton pattern for a subclass of MonoBehaviour. Use this if you have a GameObject that you only ever want one central instance of (eg a game manager script in the first scene, set to DontDestroyOnLoad).

#### Math
* Math/Random/ShuffleListExtension.cs - extension method that shuffles a list using the unbiased [Fisher-Yates algorithm](https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle).

#### Messaging and Events
* Messaging/CSharpMessengerExtended - a [useful message subscription and broadcast class](http://wiki.unity3d.com/index.php/CSharpMessenger_Extended) by by Magnus Wolffelt.

#### GUI
* FPSngui.cs - attach this to an NGUI Label (UILabel) to turn it into an FPS counter.
* ScaledRect.js - returns a rectangle scaled based on the current screen resolution. You can use this to scale the native Unity GUI across different resolutions. Or just use NGUI and not deal with all the hassle.

#### Input
* UInput.js - a wrapper of Unity's Input class that allows you to also Set and Pop axis values. This can be useful for hooking up 'virtual' touchscreen controllers since it allows you to then use the same code path for joysticks, keyboard and touch screen (see notes in script).

#### Sound
* SoundManager.cs - fill up the sounds array with AudioClips and the soundNames array with corresponding strings, and you can fire off sounds like SoundManager.Play("Powerup"); . Has a global volume setting and can play multiple sounds simultaneously at different pitches and volumes, and supports volume output scaling for more natural volume sliders. Can automatically save and retrieve global volume from PlayerPrefs.

* MusicManager.cs - a simple background music manager that allows fadein/fadeout and dynamic pitch changes.

#### Tweening
The [iTween](http://itween.pixelplacement.com/documentation.php) and [LeanTween](https://github.com/dentedpixel/LeanTween) libraries.

#### AI
* FSMSystem.cs - a finite state machine suitable for simple AI on NPCs. I've also used it for managing other high level game states (eg transitions between player turns in a turn based game). Docs: http://wiki.unity3d.com/index.php?title=Finite_State_Machine
* AStar - Simple A* pathfinding on a grid. Doesn't include any threading - if you need a more sophisticated solution see [Aron Granberg's A* Pathfinding Project](http://arongranberg.com/astar/).

#### Web
* KongregateAPI.cs - a simple class for interacting with the Kongregate stats API.
* SiteLock.cs - attempts to prevent sites stealing your Web Player game and hosting it themselves - attach it to a GameObject in your first scene and fill out the domain and redirectUrl fields. Very likely to be easily hacked.

### Editor scripts

* ScriptableObjectAssetUtil - eases the creation of ScriptableObject assets.
* Force2DSound - edit this to enable all sounds to be imported as 2D sounds (rather than the default, 3D)

### Shaders
A few useful shaders not found in Unity by default.

TODO
----

Consider allowing MusicManager to play a track using a string name (as well as track index).

Test and add someones object pooling class, eg  http://forum.unity3d.com/threads/76851-Simple-Reusable-Object-Pool-Help-limit-your-instantiations! or http://www.booncotter.com/unity-prefactory-a-better-pool-manager/ or http://vonlehecreative.wordpress.com/2010/01/06/unity-resource-gameobjectpool/ or based on http://www.third-helix.com/2011/03/unity-recycling-object-instances/ or http://unitypatterns.com/objectpool-updated-to-2-0/.

Add an example scene with some common prefabs (eg SoundManager, MusicManager, ObjectDictionary, SiteLock, FPS counter) setup ready to go.
