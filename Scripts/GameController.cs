using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public bool drawOnlySelectedCar = false;

	public int[] layers;
	public List<NeuralNetwork> network = new List<NeuralNetwork>();
	private int generation = 0;

	public int amountOfCar;
	public GameObject prefabsOfCar;
	public GameObject startPoint;

	public bool isTraining = false;
	private bool spawnEnd = false;

	void Start() {
		Time.timeScale = 10f;
	}

	void Update() {
		if (!isTraining) {
			if (generation == 0) {
				InitNeuralNetworks ();
			} else {
				network.Sort ();
				for (int i = 0; i < amountOfCar / 2; i++) {
					network [i] = new NeuralNetwork (network [i + (amountOfCar / 2)]);
					network [i].MUTASYON ();
					network [i + (amountOfCar / 2)] = new NeuralNetwork (network [i + (amountOfCar / 2)]);
					network [i].Basari = 0f;
					network [i + (amountOfCar / 2)].Basari = 0f;
				}
			}

			generation++;
			StartCoroutine(Spawn ());
			isTraining = true;
		}

		if (spawnEnd) {
			Move ();
			spawnEnd = false;
		}

		if (GameObject.FindGameObjectsWithTag ("Car").Length == 0)
			isTraining = false;
	}

	IEnumerator Spawn() {
		if (!isTraining) {
			Debug.Log ("Spawned");
			for (int i = 0; i < amountOfCar; i++) {
				GameObject go = Instantiate (prefabsOfCar, new Vector3(startPoint.transform.position.x, startPoint.transform.position.y, 0f), Quaternion.Euler (startPoint.transform.localRotation.eulerAngles));
				CarController cc = go.GetComponent<CarController> ();
				cc.network = new NeuralNetwork (network[i]);
				cc.moveable = false;
				cc.gameController = GetComponent<GameController> ();
				Sensors s = cc.GetComponent<Sensors> ();
				s.amountOfSensor = layers [0];
				yield return new WaitForSeconds (0.01f);
			}

			spawnEnd = true;
		}
	}

	void Move() {
		foreach (var item in GameObject.FindGameObjectsWithTag("Car")) {
			CarController cc = item.GetComponent<CarController> ();
			cc.moveable = true;
		}
	}

	void InitNeuralNetworks()
	{
		if (amountOfCar % 2 != 0) {
			amountOfCar += 1; 
		}

		network = new List<NeuralNetwork>();

		for (int i = 0; i < amountOfCar; i++) {
			NeuralNetwork net = new NeuralNetwork(layers);
			net.MUTASYON ();
			network.Add(net);
		}
	}
}
