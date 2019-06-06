using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag.Equals ("Car")) {
			CarController cc = other.GetComponent<CarController> ();
			if (!cc.destroying) {
				cc.Destroy ();
			}
		}


	}
}
