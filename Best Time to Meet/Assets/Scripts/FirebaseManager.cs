using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine.SceneManagement;

public class FirebaseManager : MonoBehaviour{

    // Singleton
    private static FirebaseManager _instance;
    public static FirebaseManager Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<FirebaseManager>();
            }

            return _instance;
        }
    }

    // Firebase variables
    [Header("Firebase")]
    [SerializeField] DependencyStatus dependencyStatus;
    [SerializeField] FirebaseAuth auth;
    [SerializeField] FirebaseUser User;
    [SerializeField] DatabaseReference DBref;
    Firebase.FirebaseApp firebaseApp;

    // Login variables
    [Header("Login")]
    [SerializeField] TMP_InputField emailLoginField = null;
    [SerializeField] TMP_InputField passwordLoginField = null;
    [SerializeField] TMP_Text messageText = null;

    // Register variables
    [Header("Register")]
    [SerializeField] TMP_InputField usernameRegisterField = null;
    [SerializeField] TMP_InputField emailRegisterField = null;
    [SerializeField] TMP_InputField passwordRegisterField = null;
    [SerializeField] TMP_InputField passwordRegisterVerifyField = null;

    // Calendar Infos
    string calID = "";
    string calPassword = "";

    // Cursor Track
    public bool loginCursor = false;
    public bool registerCursor = false;
    public bool inLoginScreen = false;
    public bool inRegistrationScreen = false;

    void Awake() {
        // Singleton
        DontDestroyOnLoad(gameObject);

        // Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available) {
                // If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void Start() {
        // Set cursor
        emailLoginField.Select();
        emailLoginField.ActivateInputField();
        inLoginScreen = true;
        inRegistrationScreen = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (emailLoginField.isFocused) {
                passwordLoginField.Select();
                passwordLoginField.ActivateInputField();
            }
            if (usernameRegisterField.isFocused) {
                emailRegisterField.Select();
                emailRegisterField.ActivateInputField();
            }
            if (emailRegisterField.isFocused) {
                passwordRegisterField.Select();
                passwordRegisterField.ActivateInputField();
            }
            if (passwordRegisterField.isFocused) {
                passwordRegisterVerifyField.Select();
                passwordRegisterVerifyField.ActivateInputField();
            }
        }
        if (registerCursor) {
            // Set cursor
            usernameRegisterField.ActivateInputField();
            // Turn off the order
            registerCursor = false;
        }
        if (loginCursor) {
            // Set cursor
            emailLoginField.ActivateInputField();
            // Turn off the order
            loginCursor = false;
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            if (inLoginScreen) {
                LoginButton();
            }
            if (inRegistrationScreen) {
                RegisterButton();
            }
        }
    }
    //  -------------------------------------------  //
    //  ---------------   BUTTONS   ---------------  //
    //  -------------------------------------------  //

    // Function for the login button
    public void LoginButton() {
        // Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    //Function for the register button
    public void RegisterButton() {
        // Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }
    

    //  -------------------------------------------  //
    //  -----------   PUBLIC METHODS   ------------  //
    //  -------------------------------------------  //


    public void SignOut() { auth.SignOut(); }

    // Starts operations to add saved data to the datbase
    public void SaveDataAction(string _username, int[] _userData, string _calID, string _calPassword) {
        StartCoroutine(SaveData(_username, _userData, _calID, _calPassword));
    }
        
    // Starts opening operations
    public void OpenCalendarAction(string _calID, string _calPassword) {
        StartCoroutine(OpenCalendar(_calID, _calPassword));
    }

    //  -------------------------------------------  //
    //  -----------   PRIVATE METHODS   -----------  //
    //  -------------------------------------------  //

    // Setting up the Firebase Connections
    void InitializeFirebase() {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBref = FirebaseDatabase.DefaultInstance.RootReference;
        // Update Google Services 
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                firebaseApp = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

    }


    // This passes the username to the Cal Manager after scene loaded
    IEnumerator PassUsername(string _username) {
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<CalManager>().Username = _username;

        // Keep track the screen
        inLoginScreen = false;
        FindObjectOfType<AppManager>().inPlanScene = true;
        FindObjectOfType<AppManager>().inPlanScreen = true;
    }

    #region Updates

    // Handles opening an existing calendar
    IEnumerator OpenCalendar(string _calID, string _calPassword) {

        // Get the calendar data
        var DBTask = DBref.Child("calendars").Child(_calID).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        // Check is there any problem?
        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            FindObjectOfType<AppManager>().PlanPageMessage("Error: Invalid Inputs!");
        } // If system can'f find any data
        else if (DBTask.Result.Value == null) {
            // No data exists yet
            FindObjectOfType<AppManager>().PlanPageMessage("Wrong Calendar ID!");
        }
        else {
            // Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;
            
            // Check if the passowrd is correct or not!
            if (snapshot.Child("cal_password").Value.ToString() != _calPassword) {
                FindObjectOfType<AppManager>().PlanPageMessage("Wrong Password!");
            }
            else { // Send the dt to the Cal Manager
                FindObjectOfType<CalManager>().OpenCalendar(
                    snapshot, User.DisplayName, _calID, _calPassword);
            }
        }
    }

    // Handles saving data to the database
    IEnumerator SaveData(string _username, int[] _userData, string _calID, string _calPassword) {
        // Store the password first
        var DBTask = DBref.Child("calendars").Child(_calID).Child("cal_password").SetValueAsync(_calPassword);

        // Check all days from user data
        for (int i = 0; i < 31; i++) {
            // If in that day, user marked as "In", then write that day to the related cal ID
            // calendars > call ID (543-123) > username (test15-1) > 0 (day index) : 1 (1 - in, 0 - out)
            if (_userData[i] == 1) {
                DBTask = DBref.Child("calendars").Child(_calID).
                    Child(_username).Child(i.ToString()).SetValueAsync("1");
            } // If user didn't select the day, then make it 0. Therefore if user has selected the same day before, we update now.
            else {
                DBTask = DBref.Child("calendars").Child(_calID).
                    Child(_username).Child(i.ToString()).SetValueAsync("0");
            }
        }

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else {
            Debug.Log("Database updated!");
            FindObjectOfType<AppManager>().PlanPageMessage("Saved !");
        }
    }

    IEnumerator Login(string _email, string _password) {
        // Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        // Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null) {
            // If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode) {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
                default:
                    messageText.text = message;
                    break;
            }
            messageText.text = message;
        }
        else {
            // User is now logged in
            // Now get the result
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            messageText.text = "";
            messageText.text = "Logged In";
            SceneManager.LoadScene("PlanScene");

            // Keep it in the AppManager
            StartCoroutine(PassUsername(User.DisplayName));
        }
    }

    IEnumerator Register(string _email, string _password, string _username) {
        if (_username == "") {
            // If the username field is blank show a warning
            messageText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text) {
            // If the password does not match show a warning
            messageText.text = "Password Does Not Match!";
        }
        else {
            // Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            // Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null) {
                // If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode) {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                messageText.text = message;
            }
            else {
                // User has now been created
                // Now get the result
                User = RegisterTask.Result;

                // Keep it in the AppManager
                StartCoroutine(PassUsername(User.DisplayName));

                if (User != null) {
                    // Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    // Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    // Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null) {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        messageText.text = "Username Set Failed!";
                    }
                    else {
                        // Write username to the database
                        StartCoroutine(UpdateUsernameAuth(_username));
                        StartCoroutine(UpdateUsernameDatabase(_username));
                        // Username is now set
                        // Now return to login screen
                        FindObjectOfType<AppManager>().LandingScene();
                        messageText.text = "";
                        FindObjectOfType<AppManager>().LandingPageMessage("Registration is succesful!");

                        // Set cursor
                        loginCursor = true;
                        inLoginScreen = true;
                        inRegistrationScreen = false;
                    }
                }
            }
        }
    }

    IEnumerator UpdateUsernameAuth(string _username) {
        // Create a user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };

        // Call the Firebase auth update user profile function passing the profile with the username
        var ProfileTask = User.UpdateUserProfileAsync(profile);
        // Wait until the task completes
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else {
            // Auth username is now updated
        }
    }

    IEnumerator UpdateUsernameDatabase(string _username) {
        // Set the currently logged in user username in the database
        var DBTask = DBref.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else {
            // Database username is now updated
        }
    }

    #endregion
}
