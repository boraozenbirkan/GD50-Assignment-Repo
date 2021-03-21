using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnInput : MonoBehaviour {	
	// Update is called once per frame
	void Update () {
		// BORA.2 Updated Scene Transition
		if (Input.GetAxis("Submit") == 1) {
			if (SceneManager.GetActiveScene().name == "Title"){
				SceneManager.LoadScene("Play");
				DontDestroy.numberOfMaze = 1;
			}
			else{
				SceneManager.LoadScene("Title");
			}				
		}
	}
}
