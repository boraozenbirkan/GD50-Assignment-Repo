using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour{
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
    [SerializeField] TextMeshProUGUI planLandingPageMessageText = null;
    // Open field
    [SerializeField] TMP_InputField openCalID = null;
    [SerializeField] TMP_InputField openCalPassword = null;

    // Cursor Track
    public bool inPlanScene = false;
    public bool inPlanScreen = false;

    private void Update() {
        // Navigation with Tab and Enter keys
        if (Input.GetKey(KeyCode.Tab) && SceneManager.GetActiveScene().name == "PlanScene") {
            if (openCalID.isFocused) {
                // Set cursor
                openCalPassword.Select();
                openCalPassword.ActivateInputField();
            }
        }
        if (inPlanScene && SceneManager.GetActiveScene().name == "PlanScene") {
            // Set cursor
            openCalID.Select();
            openCalID.ActivateInputField();
            // Turn off the order
            inPlanScene = false;
        }
        if (Input.GetKeyDown(KeyCode.Return) && SceneManager.GetActiveScene().name == "PlanScene") {
            if (inPlanScreen) {
                OpenCalendar();
            }
        }
    }

    // Creates an call ID and opens the new calendar
    public void CreateNewCalender() {
        // Creat ID like 123-456
        int calID = Random.Range(100, 1000);
        string calID_str = "";
        calIDText.text = calID_str = calID.ToString() + "-";
        calID = Random.Range(100, 1000);
        calIDText.text += calID.ToString();
        calID_str += calID.ToString();

        // Create 6 digits password
        string calPassword = Random.Range(100000, 999999).ToString();

        // Push the callID and Password to the cal Manager
        FindObjectOfType<CalManager>().CalID = calID_str;
        FindObjectOfType<CalManager>().CalPassword = calPassword;

        // Update Cal Info Display
        FindObjectOfType<CalManager>().UpdateCalInfo();

        // Update Heat Map - Which will create completely empty one
        FindObjectOfType<CalManager>().UpdateHeatMap();

        // Change Canvases
        planLandingCanvas.SetActive(false);
        planCalendarCanvas.SetActive(true);

        StartCoroutine(CalMessage(planMessageText, "New Calendar created!", 3f));
    }

    // Handles transition to the Calendar Canvas
    public void OpenCalCanvas() {
        planLandingCanvas.active = false;
        planCalendarCanvas.active = true;
    }

    // Writes messages to the landing page of the plan scene
    public void PlanPageMessage(string _message) { StartCoroutine(PlanMessageHighlight(_message)); }
    IEnumerator PlanMessageHighlight(string _message) {
        planLandingPageMessageText.text = _message;
        yield return new WaitForSeconds(2);
        planLandingPageMessageText.text = "";
    }

    // Writes messages to the Landing page
    public void LandingPageMessage(string _message) { StartCoroutine(LandingMessageHighlight(_message)); }
    IEnumerator LandingMessageHighlight(string _message) {
        planLandingPageMessageText.text = _message;
        yield return new WaitForSeconds(2);
        planLandingPageMessageText.text = "";
    }

    // Message field for the Calendar view
    IEnumerator CalMessage(TextMeshProUGUI _textBox, string _msg, float _seconds) {
        _textBox.text = _msg;
        yield return new WaitForSeconds(_seconds);
        _textBox.text = "";
    }

    #region Buttons

    public void LandingScene(){
        loginCanvas.SetActive(true);
        registerCanvas.SetActive(false);
        // Clear the inputs
        loginUsernameInput.text = loginPasswordInput.text = messageText.text = "";
        // Set cursor
        FindObjectOfType<FirebaseManager>().loginCursor = true;
        FindObjectOfType<FirebaseManager>().inRegistrationScreen = false;
        FindObjectOfType<FirebaseManager>().inLoginScreen = true;
    }

    public void RegisterScene() {
        loginCanvas.SetActive(false);
        registerCanvas.SetActive(true);

        // Clear the inputs
        registerUsernameInput.text = registerPasswordInput.text = 
            registerConfirmationInput.text = messageText.text = "";

        // Set cursor
        FindObjectOfType<FirebaseManager>().registerCursor = true;
        FindObjectOfType<FirebaseManager>().inRegistrationScreen = true;
        FindObjectOfType<FirebaseManager>().inLoginScreen = false;
    }

    public void LogoutButton() {        
        SceneManager.LoadScene("LandingScene");
        FindObjectOfType<FirebaseManager>().SignOut();
        // Set cursor
        FindObjectOfType<FirebaseManager>().loginCursor = true;
        FindObjectOfType<FirebaseManager>().inLoginScreen = true;
        inPlanScene = false;
        inPlanScreen = false;
    }

    // Gives order to open an existing calendar
    public void OpenCalendar() {
        FindObjectOfType<FirebaseManager>().OpenCalendarAction(openCalID.text, openCalPassword.text);
    }


    #endregion

}
