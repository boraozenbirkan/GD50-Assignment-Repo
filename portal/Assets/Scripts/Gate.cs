using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour{

    [SerializeField] Canvas canvas = null;

    private void OnTriggerEnter(Collider col){
        canvas.enabled = true;
        Time.timeScale = 0;
    }
    
}
