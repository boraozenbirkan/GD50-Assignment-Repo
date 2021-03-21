using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelText : MonoBehaviour{
    [SerializeField] Text playText = null;
    [SerializeField] Text gameOverText = null;

    // Start is called before the first frame update
    void Start()    {
        updateText();
    }
    public void updateText(){
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Play"){
				playText.text = "Level: " + DontDestroy.numberOfMaze.ToString();
		}
		else {
				gameOverText.text = "You've reach level " + DontDestroy.numberOfMaze.ToString();
		}
    }
}
