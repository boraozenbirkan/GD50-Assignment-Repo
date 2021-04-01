using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour{

    public static AppManager instance;

    [Header ("Landing Scene Objects")]
    [SerializeField] GameObject loginCanvas = null;
    [SerializeField] GameObject registerCanvas = null;
    [SerializeField] TextMeshProUGUI loginUsernameInput = null;
    [SerializeField] TextMeshProUGUI loginPasswordInput = null;
    [SerializeField] TextMeshProUGUI registerUsernameInput = null;
    [SerializeField] TextMeshProUGUI registerPasswordInput = null;
    [SerializeField] TextMeshProUGUI registerConfirmationInput = null;
    [SerializeField] TextMeshProUGUI messageText = null;

    [Header("Plan Scene Objects")]
    [SerializeField] GameObject planLandingCanvas = null;
    [SerializeField] GameObject planCalendarCanvas = null;
    [SerializeField] TextMeshProUGUI calIDText = null;
    [SerializeField] TextMeshProUGUI planMessageText = null;

    /*
    void Awake(){
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }*/

    public void SetCalanderID(string calID) {

    }

    public void CreateNewCalender() {
        // Creat ID like 123-456
        int calID = Random.Range(0, 1000);
        calIDText.text = calID.ToString() + "-";
        calID = Random.Range(0, 1000);
        calIDText.text += calID.ToString();

        planLandingCanvas.SetActive(false);
        planCalendarCanvas.SetActive(true);

        StartCoroutine(CallMessage(planMessageText, "New Calendar has been created!", 3f));
    }

    IEnumerator CallMessage(TextMeshProUGUI textBox, string msg, float seconds) {
        textBox.text = msg;
        yield return new WaitForSeconds(seconds);
        textBox.text = "";
    }

    #region Buttons

    public void LandingScene(){
        loginCanvas.SetActive(true);
        registerCanvas.SetActive(false);
        // Clear the inputs
        loginUsernameInput.text = loginPasswordInput.text = messageText.text = "";
    }

    public void RegisterScene() {
        loginCanvas.SetActive(false);
        registerCanvas.SetActive(true);

        // Clear the inputs
        registerUsernameInput.text = registerPasswordInput.text = 
            registerConfirmationInput.text = messageText.text = "";
    }

    public void LogoutButton() {
        SceneManager.LoadScene("LandingScene");
    }

    #endregion

}
