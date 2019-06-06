using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour {

	public GameObject camera;
	public GameObject target;
	public GameObject startPoint;

	void Update() {
		float fitness = float.MinValue;
		foreach (var item in GameObject.FindGameObjectsWithTag("Car")) {
			if (item.GetComponent<CarController> ().network.Basari > fitness) {
				fitness = item.GetComponent<CarController> ().network.Basari;
				item.GetComponent<CarController> ().selected = true;
				target = item;
			}
		}

		if (target == null) {
			camera.transform.position = new Vector3 (startPoint.transform.position.x, startPoint.transform.position.y, -10f);
			return;
		}
		camera.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, -10f);
	}
}
