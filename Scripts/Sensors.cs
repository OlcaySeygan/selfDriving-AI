using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensors : MonoBehaviour {

	public GameObject target;

	public LayerMask targetLayer;

	public float maxDistanceOfSensor;
	public float[] distanceOfSensor;

	public GameObject prefabsOfSensor;
	public int amountOfSensor;
	private GameObject[] gameObjectOfSensor;
	private LineRenderer[] lineRendererOfSensor;
	public Color[] colorOfSensor;
	public Vector2[] directionOfSensor;

	public float[] outOfSensors;

	public Quaternion rotation;

	void Start() {
		gameObjectOfSensor = new GameObject[amountOfSensor];
		lineRendererOfSensor = new LineRenderer[amountOfSensor];
		distanceOfSensor = new float[amountOfSensor];
		colorOfSensor = new Color[amountOfSensor];
		directionOfSensor = new Vector2[amountOfSensor];
		outOfSensors = new float[amountOfSensor];



		for (int i = 0; i < amountOfSensor; i++) {
			gameObjectOfSensor [i] = Instantiate (prefabsOfSensor, transform.position, Quaternion.Euler (0f, 0f, 0f), transform);
			gameObjectOfSensor [i].name = "Sensor <" + i.ToString () + ">";
			lineRendererOfSensor [i] = gameObjectOfSensor [i].GetComponent<LineRenderer> ();
		}
	}

	public float startAngle = 90f;
	void Update() {
		Vector3 relativePosition = target.transform.position - transform.position;
		float degree = Degrees (relativePosition.x, relativePosition.y);
		for (int i = 0; i < amountOfSensor; i++) {
			GameObject s = gameObjectOfSensor [i];
			LineRenderer lr = lineRendererOfSensor [i];
			s.transform.position = transform.position;
			float angle = ((degree - startAngle) + ((((180f / amountOfSensor) / 2f)) + ((180f / amountOfSensor) * (i))));
			directionOfSensor[i] = new Vector3 (Mathf.Sin (Mathf.Deg2Rad * (angle)), Mathf.Cos (Mathf.Deg2Rad * (angle)));

			lr.SetPosition (0, s.transform.position);
			lr.SetPosition (1, s.transform.position);

			distanceOfSensor [i] = maxDistanceOfSensor;

			RaycastHit2D hit = Physics2D.Raycast (transform.position, directionOfSensor[i], maxDistanceOfSensor, targetLayer);
			if (hit.collider != null) {
				distanceOfSensor [i] = Vector2.Distance (transform.position, hit.point);
				if (GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().drawOnlySelectedCar && GetComponent<CarController> ().selected) {
					lr.SetPosition (1, hit.point);
				} else if (!GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().drawOnlySelectedCar) {
					lr.SetPosition (1, hit.point);
				}
			}

			float per = distanceOfSensor [i] * 1f / maxDistanceOfSensor;
			float r = 1f; float g = per; float b = per;
			colorOfSensor [i] = new Color (r, per, per);
			lr.materials [0].color = colorOfSensor [i];
		}
	}

	public float[] Outputs() {
		List<float> o = new List<float> ();
		for (int i = 0; i < amountOfSensor; i++) 
			o.Add (distanceOfSensor [i] * 1f / maxDistanceOfSensor);
		outOfSensors = o.ToArray ();
		return o.ToArray ();
	}

	private Vector2 f(float degrees, Vector2 start, float distance) {
		float radians = degrees * Mathf.Deg2Rad;

		float x = start.x + (distance * (float)(Mathf.Cos (radians)));
		float y = start.y + (distance * (float)(Mathf.Sin (radians)));

		Debug.DrawLine (start, new Vector3 (x, y, 0f), Color.blue, 0.1f, false);
		Debug.Log (x + " " + y);
		return new Vector2 (x, y);
	}

	public float Degrees(float x, float y) {
		float value = (float)(Mathf.Atan2(x,y) / Mathf.PI * 180f);
		if (value < 0)
			value += 360f;
		return value;
	}
}
