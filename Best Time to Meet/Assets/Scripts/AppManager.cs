using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class AppManager : MonoBehaviour{

    //public static AppManager instance;

    FirebaseAuth auth;
    DatabaseReference DBref;

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
    [SerializeField] TextMeshProUGUI usernameText = null;
    string username = null;

    
    void Start(){
        //Set the authentication instance object
        //auth = FirebaseAuth.DefaultInstance;
        //DBref = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateNewCalender() {
        // Creat ID like 123-456
        int calID = Random.Range(100, 1000);
        string callID_str = "";
        calIDText.text = callID_str = calID.ToString() + "-";
        calID = Random.Range(100, 1000);
        calIDText.text += calID.ToString();
        callID_str += calID.ToString();

        // Push the cal to the DB
        FindObjectOfType<FirebaseManager>().CreateCalendar(callID_str);

        // Update Username Display
        FindObjectOfType<CalManager>().UpdateUsernameDisplay(GetUsername());
        
        // Change Canvases
        planLandingCanvas.SetActive(false);
        planCalendarCanvas.SetActive(true);

        StartCoroutine(CallMessage(planMessageText, "New Calendar created!", 3f));
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
        FindObjectOfType<FirebaseManager>().SignOut();
    }

    #endregion


    #region Getters and Setters

    public void SetUsername(string _username) { username = _username; }
    public string GetUsername() { return username; }

    #endregion
}
