using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeometryUtils {

	public static Bounds getBounds(Transform g) {
		Bounds b = new Bounds(Vector3.zero, Vector3.zero);
		foreach (MeshFilter m in g.GetComponentsInChildren<MeshFilter>()) {
			Bounds mb = m.mesh.bounds;
			mb.center = m.transform.position; // position bounds globally
			if (b.size.x == 0 && b.size.y == 0) {
				b = mb;
			} else {
    			b.Encapsulate(mb);
			}
		}
		b.center = g.position;
		return b;
	}
	
	public static Bounds getBounds(GameObject g) {
		return getBounds(g.transform);
	}
	
	public static Vector3 getCenter(Transform g) {
		Vector3 vsum = Vector3.zero;
		int n_obj = 0;
		foreach (Transform m in g) {
			if (vsum == Vector3.zero) {
				vsum = m.position;
			} else {
				vsum += m.position;
			}
			n_obj++;
		}
		Vector3 cog = vsum / n_obj;
		return cog;
	}
	
	public static Vector3 getCenter(List<GameObject> g) {
		Vector3 vsum = Vector3.zero;
		int n_obj = 0;
		foreach (GameObject m in g) {
			if (vsum == Vector3.zero) {
				vsum = m.transform.position;
			} else {
				vsum += m.transform.position;
			}
			n_obj++;
		}
		Vector3 cog = vsum / n_obj;
		return cog;
	}
}
