using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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


    void Awake(){
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    #region Buttons

    public void LandingScene(){
        loginCanvas.SetActive(true);
        registerCanvas.SetActive(false);
        // Clear the inputs
        loginUsernameInput.text = loginPasswordInput.text = "";
    }

    public void RegisterScene() {
        loginCanvas.SetActive(false);
        registerCanvas.SetActive(true);

        // Clear the inputs
        registerUsernameInput.text = registerPasswordInput.text = registerConfirmationInput.text = "";
    }

    #endregion

}
