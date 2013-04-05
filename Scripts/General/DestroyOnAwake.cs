using UnityEngine;
using System.Collections;

public class DestroyOnAwake : MonoBehaviour {
	public bool keepInEditor = false;
	private bool destroy = true;
	
	// Use this for initialization
	void Awake () {
		#if UNITY_EDITOR
		if (keepInEditor) destroy = false;
		#endif
		if (destroy) {
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
