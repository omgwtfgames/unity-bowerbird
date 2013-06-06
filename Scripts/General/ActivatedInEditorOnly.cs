// NOTE: similar functionality is built into Unity - 
//       just tag any GameObject as "EditorOnly" and it
//       won't be included in published builds but will
//       be available in the Editor.

using UnityEngine;
using System.Collections;

public class ActivatedInEditorOnly : MonoBehaviour {
	
	void Awake () {
     gameObject.SetActive(false);
     #if UNITY_EDITOR
	   gameObject.SetActive(true);
     #endif
	}
}
