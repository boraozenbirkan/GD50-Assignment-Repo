using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonBehaviour : MonoBehaviour{

    public bool clicked = false;
    Button button = null;
    ColorBlock refColor;
    ColorBlock currentColor;

    private void Awake() {
        button = GetComponent<Button>();
        refColor = button.colors;
        refColor.normalColor = refColor.selectedColor = Color.white;
        currentColor = refColor;
    }

    public void ButtonClicked() {
        // Send the name of the button and the Attendats Text
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        FindObjectOfType<CalManager>().ButtonEvent(name, texts[1]);

        // Toggle the color of the button
        if (!clicked) {
            currentColor.normalColor = Color.green;
            currentColor.selectedColor = Color.green;
            button.colors = currentColor;
            clicked = true;
        }
        else {
            button.colors = refColor;
            clicked = false;
        }
    }

}
