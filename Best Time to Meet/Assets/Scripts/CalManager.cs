using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CalManager : MonoBehaviour{

    [SerializeField] TextMeshProUGUI usernameText = null;

    int[] usersDays = new int[32];
    int[] DBdays = new int[32];

    [SerializeField] Day[] daysInput;

    void Start()
    {
        // Set default value for the dayCounter
        for (int i = 0; i < 32; i++) { usersDays[i] = 0; }

        // Fetch data from Database
        for (int i = 1; i < 32; i++) {
            // Store them in DBdays

            // Write them in buttons

        }
    }

    private void Update() {
        
    }


    //      ----    Buttons     ----        //

    public void ButtonEvent(string buttonName, TextMeshProUGUI buttonText) {
        // Toggle the day index in the usersDays
        string[] strSplitted = buttonName.Split('_');
        int buttonIndex;
        int.TryParse(strSplitted[1], out buttonIndex);  Debug.Log("button Index: " + buttonIndex);

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
    }

    public void UpdateUsernameDisplay(string _username) {
        usernameText.text = _username;
        Debug.Log("Username name in CalManager: " + _username);
    }
}
