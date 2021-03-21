using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrabPickups : MonoBehaviour {

	private AudioSource pickupSoundSource;

	void Awake() {
		pickupSoundSource = DontDestroy.instance.GetComponents<AudioSource>()[1];
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		if (hit.gameObject.tag == "Pickup") {
			// BORA.3 Increse number of Maza by 1
			FindObjectOfType<DontDestroy>().IncreaseMazeNumber();
			pickupSoundSource.Play();
			SceneManager.LoadScene("Play");
		}
	}
}
