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
