using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DontDestroy : MonoBehaviour {

	// make this static so it's visible across all instances
	public static DontDestroy instance = null;

	// BORA - I turned this object to an AudioManager but left the name same
	[SerializeField] AudioSource menuSound = null;
	[SerializeField] AudioSource whisperSound_1 = null;
    // + Using this obj as a game manager
    public static int numberOfMaze;
	int currentMazeNumber = 0;
	string currentScene = null;
	bool waitForMazeNum = false;
	

    // singleton pattern; make sure only one of these exists at one time, else we will
    // get an additional set of sounds with every scene reload, layering on the music
    // track indefinitely
    void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		} else if (instance != this) {
			Destroy(gameObject);
		}
	}

	// Use this for initialization	
	// BORA Sound updated
	void Start () {
		currentScene = SceneManager.GetActiveScene().name;
		Debug.Log("1 Current Scene: " + currentScene);
		ChangeSceneSound(currentScene);
		currentMazeNumber = numberOfMaze;
	}
	// BORA Sound updated
	void Update(){
		// If scene changes, then play relavent sound
		if (currentScene != SceneManager.GetActiveScene().name){
			currentScene = SceneManager.GetActiveScene().name;
			ChangeSceneSound(currentScene);
			Debug.Log("2 Current Scene: " + currentScene);
		}
		// Keeping track of maze number
		if (currentMazeNumber != numberOfMaze){
			currentMazeNumber = numberOfMaze;
			Debug.Log("Maze Number: " + numberOfMaze);
		}
	}

	// Change sound related to the Current Scene
	public void ChangeSceneSound(string currentScene){
		// Loading the new scene takes little bit more time. 
		// So we change the sound with a little delay.
		StartCoroutine(CheckSoundLater(currentScene));		
	}
	IEnumerator CheckSoundLater(string sceneName){
		yield return new WaitForSeconds(0.2f);
		
		//string sceneName = SceneManager.GetActiveScene().name;
		if (sceneName == "Title"){
			if (!menuSound.isPlaying) {menuSound.Play();}
			whisperSound_1.Stop();
		}
		else if (sceneName == "Play"){			
			menuSound.Stop();
			whisperSound_1.Play();
		}
		else {
			menuSound.Play();
			whisperSound_1.Stop();
		}
	}
	public void IncreaseMazeNumber(){
		if 	(!waitForMazeNum)
			StartCoroutine(MazeNumberDelay());
	}
	IEnumerator MazeNumberDelay(){
		waitForMazeNum = true;
		yield return new WaitForSeconds(0.3f);

		numberOfMaze++;
		waitForMazeNum = false;
		FindObjectOfType<LevelText>().updateText();
	}
}
