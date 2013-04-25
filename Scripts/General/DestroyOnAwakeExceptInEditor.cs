using UnityEngine;
using System.Collections;

public class DestroyOnAwakeExceptInEditor : MonoBehaviour {
	
	void Awake () {
	#if !UNITY_EDITOR
		Destroy(gameObject);
	#endif
	}
}
