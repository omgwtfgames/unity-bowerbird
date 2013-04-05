using UnityEngine;
using System.Collections;

public class DestroyOnAwakeExceptInEditor : MonoBehaviour {
	
	void Awake () {
	#if !UNITY_EDITOR
		Destroy(gameObject);
	#endif
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
