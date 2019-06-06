using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

	public GameController gameController;

	public NeuralNetwork network;
	public float[] inputs;
	public float[] outputs;
	Sensors sensors;

	[Range(-1f, 1f)]
	public float steeringWheelRange = 0f;
	[Range(0.25f, 1f)]
	public float powerRange = 0f;
	public float power = 3f;
	public float maxSpeed = 5f;
	public float turnPower = 2f;
	public float friction = 3f;
	public Vector2 curSpeed;
	Rigidbody2D rb2d;

	public bool moveable = false;
	public bool destroying = false;
	public bool selected = false;

	void Start() {
		if (network == null)
			Destroy (gameObject);

		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		rb2d = GetComponent<Rigidbody2D> ();
		sensors = GetComponent<Sensors> ();
	}

	void Update() {
		if (!moveable)
			return;

		inputs = sensors.Outputs ();
		outputs = network.ıLERı_HESAPLAMA (inputs);

		steeringWheelRange = outputs [0];
		powerRange = outputs [1];
		if (powerRange < 0.25f)
			powerRange = 0.25f;

		curSpeed = new Vector2 (rb2d.velocity.x, rb2d.velocity.y);

		if (curSpeed.magnitude > maxSpeed) {
			curSpeed = curSpeed.normalized;
			curSpeed *= maxSpeed;
		}

		rb2d.drag = friction;
		if (powerRange > 0f) {
			rb2d.AddForce (transform.up * (powerRange * power));

		} else {
			rb2d.AddForce (transform.up * ((powerRange * power) / 2f));
		}

		transform.Rotate (Vector3.forward * turnPower * -steeringWheelRange);

		noGas (powerRange > 0f);

		network.Basari += Time.deltaTime;
	}

	public void Destroy() {
		Destroy (gameObject);
	}

	void noGas(bool gas) {
		if (!gas)
			rb2d.drag = friction * 2f;
	}
}
