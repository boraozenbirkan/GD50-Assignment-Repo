using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using UnityEngine.UI;
using TMPro;

public class CalManager : MonoBehaviour{

    [SerializeField] TextMeshProUGUI usernameText = null;
    [SerializeField] TextMeshProUGUI calIDText = null;
    [SerializeField] TextMeshProUGUI calPasswordText = null;
    [SerializeField] Button[] buttons = null;
    [SerializeField] Image[] heatMap = null;

    ColorBlock refColor;
    ColorBlock currentColor;
    Color heatColor;

    int[] usersDays = new int[31];
    int[] DBdays = new int[31];
    int[] heatMapData = new int[31];

    public string CalID { get; set; }
    public string Username { get; set; }
    public string CalPassword { get; set; }

    private void Start() {
        // Make internal data stroge empty
        for (int i = 0; i < 31; i++) {
            usersDays[i] = 0;
            DBdays[i] = 0;
        }

        // First get the normal color as a referance color
        refColor = buttons[0].colors;
        refColor.normalColor = refColor.selectedColor = Color.white;
        currentColor = refColor;

        heatColor = Color.red;
    }

    //  -------------------------------------------  //
    //  -----------   PUBLİC METHODS   ------------  //
    //  -------------------------------------------  //
    
    // Updates Calendar info and Displays
    public void UpdateCalInfo() {
        usernameText.text = Username;
        calIDText.text = CalID;
        calPasswordText.text = CalPassword;
    }

    // Opens an existing calender with the given information
    public void OpenCalendar(DataSnapshot _snapshot, string _username, string _calID, string _calPassword) {
        // Display cal info
        Username = _username;   
        CalID = _calID;
        CalPassword = _calPassword;
        UpdateCalInfo();

        // Get the DB 
        DataSnapshot snapshot = _snapshot;
        foreach (DataSnapshot user in snapshot.Children) {
            // skip the password key
            if (user.Key != "cal_password") {
                // check all the values
                for (int i = 0; i < 31; i++) {
                    string value = user.Child(i.ToString()).Value.ToString();
                    // if the value is equal 1, store it
                    if (value == "1") {
                        DBdays[i] += 1;
                        // if the data is about the current user, then store it seperately
                        if (user.Key == Username) { usersDays[i] = 1;  }
                    }
                }
            }
        }
        // Write the number of attendants to the buttons
        // Create the biggest num variable
        int theBiggestAttendant = 1; 
        for (int i = 0; i < 31; i++) {
            TextMeshProUGUI[] texts = buttons[i].GetComponentsInChildren<TextMeshProUGUI>();
            texts[1].text = "Attendants: " + DBdays[i].ToString();

            // Store value to the heatMapData as well
            heatMapData[i] = DBdays[i];
            // Keep the bigget number
            if (DBdays[i] > theBiggestAttendant) { theBiggestAttendant = DBdays[i]; }
        }
        // Create heatIndex
        float heatIndex = 1 / (float)theBiggestAttendant;

        // Color the heatMap
        for (int i = 0; i < heatMap.Length; i++) {
            heatColor.a = heatIndex * (float)heatMapData[i];
            heatMap[i].color = heatColor;
        }
        // Turn on the ones user already selected
        // Then turn on the ones who already selected by the user
        for (int i = 0; i < usersDays.Length; i++) {
            // Make all the buttons uncliked at first to avoid buggy behaviour
            buttons[i].GetComponent<ButtonBehaviour>().clicked = false;
            buttons[i].colors = refColor;

            if (usersDays[i] == 1) {
                currentColor.normalColor = Color.green;
                currentColor.selectedColor = Color.green;
                buttons[i].colors = currentColor;
                // make them cliked
                buttons[i].GetComponent<ButtonBehaviour>().clicked = true;
            }
        }
        // Open the calendar
        FindObjectOfType<AppManager>().OpenCalCanvas();
    }

    // Updates Heat Map
    public void UpdateHeatMap() {
        // Update heatmap according to number of attendants
        // Create the biggest num variable
        int theBiggestAttendant = 1;
        for (int i = 0; i < buttons.Length; i++) {
            // Get buttons texts
            TextMeshProUGUI[] texts = buttons[i].GetComponentsInChildren<TextMeshProUGUI>();
            // Take the second one and split
            string[] strSplitted = texts[1].text.Split(' ');
            // Get the number of attendants as int
            int attendentNum; int.TryParse(strSplitted[1], out attendentNum);
            // Store the heatMapData
            heatMapData[i] = attendentNum;
            // Keep the bigget number
            if (attendentNum > theBiggestAttendant) { theBiggestAttendant = attendentNum; }
        }
        // Create heatIndex
        float heatIndex = 1 / (float)theBiggestAttendant;
        
        // Color the heatMap
        for (int i = 0; i < heatMap.Length; i++) {
            heatColor.a = heatIndex * (float)heatMapData[i];
            heatMap[i].color = heatColor;
        }
    }

    //  -------------------------------------------  //
    //  --------------   BUTTONS   ----------------  //
    //  -------------------------------------------  //

    // Handles with buttun actions
    public void ButtonEvent(string buttonName, TextMeshProUGUI buttonText) {
        // Toggle the day index in the usersDays
        string[] strSplitted = buttonName.Split('_');
        int buttonIndex;
        // Get button index and decrease 1 because it is 1 more in the editor
        int.TryParse(strSplitted[1], out buttonIndex); buttonIndex--; 

        if (usersDays[buttonIndex] == 0) { 
            usersDays[buttonIndex] = 1;

            // Increase the buttonText
            string[] tmpSplitted = buttonText.text.Split(' ');
            int attendentNum; int.TryParse(tmpSplitted[1], out attendentNum);
            attendentNum++;
            buttonText.text = "Attendants: " + attendentNum.ToString();
        }
        else {
            usersDays[buttonIndex] = 0;

            // Decrease the buttonText
            string[] tmpSplitted = buttonText.text.Split(' ');
            int attendentNum; int.TryParse(tmpSplitted[1], out attendentNum);
            attendentNum--;
            buttonText.text = attendentNum.ToString();
            buttonText.text = "Attendants: " + attendentNum.ToString();
        }
        // Update Heat Map
        UpdateHeatMap();
    }

    public void SaveButton() {
        FindObjectOfType<FirebaseManager>().SaveDataAction(Username, usersDays, CalID, CalPassword);
    }

}
