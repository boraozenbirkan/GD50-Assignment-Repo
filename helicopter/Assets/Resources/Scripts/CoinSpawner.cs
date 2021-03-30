using UnityEngine;
using System.Collections;

public class CoinSpawner : MonoBehaviour {

	public GameObject[] prefabs;

	// Use this for initialization
	void Start () {

		// infinite coin spawning function, asynchronous
		StartCoroutine(SpawnCoins());
	}

	// Update is called once per frame
	void Update () {

	}

	IEnumerator SpawnCoins() {
		while (true) {

			// number of coins we could spawn vertically
			int coinsThisRow = Random.Range(1, 4);

			// instantiate all coins in this row separated by some random amount of space
			for (int i = 0; i < coinsThisRow; i++) {

				// BORA.1 I give 1 in 5 chance to spawn a gem
				int coinType;
				if (Random.Range(0, 5) == 0) { coinType = 1; } else { coinType = 0; }

				// Updated below code
				Instantiate(prefabs[coinType], new Vector3(26, Random.Range(-10, 10), 10), Quaternion.identity);
			}

			// pause 1-5 seconds until the next coin spawns
			yield return new WaitForSeconds(Random.Range(1, 5));
		}
	}
}
