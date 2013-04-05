using UnityEngine;
using System.Collections;

public class DestroyAfter : MonoBehaviour {
	public float time;
	public bool justSetInactive = false;
  public bool detachChildren = false;
	//private float counter = 0f;
  //private bool done = false;

  void Awake() {
    Invoke("DestroyOrWhatever", time);
  }

  /*
	void Update () {
		if (!done && counter >= time) {
      DestroyOrWhatever();
      done = true;
		}
		counter += Time.deltaTime;
	}
  */
  
  void DestroyOrWhatever() {
    if (detachChildren) {
        transform.DetachChildren();
    }

    if (justSetInactive) {
      gameObject.SetActive(false);
    } else {
      Destroy(gameObject);
    }
  }
}
