// Snippet from: http://fortressfiasco.wordpress.com/2013/06/03/how-to-force-all-new-sounds-to-be-2d-in-unity/
// Forces all imported sounds to be set as 2D sounds.

using UnityEditor;
using UnityEngine;

public class Force2DSound : AssetPostprocessor {	

	// Make this true to force 2D sound importing.
	// (initially set to false by default as a precaution, since it overrides expected Unity behaviour)
	bool forceSoundImportTo2D = false;

	public void OnPreprocessAudio() {
		if (forceSoundImportTo2D) {
			AudioImporter ai = assetImporter as AudioImporter;
			ai.threeD = false;
			Debug.Log("Forcing import of sound as 2D: " + ai.assetPath);
		}
	}
}