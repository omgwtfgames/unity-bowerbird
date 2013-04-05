using UnityEngine;
using System.Collections;

public class ActivatedInEditorOnly : MonoBehaviour {
	
	public bool showLabels = false;
	
	void Awake () {
     #if UNITY_EDITOR
	 showLabels = true;
     #endif
		if (showLabels) {
           gameObject.SetActive(true);
		} else {
		   gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
