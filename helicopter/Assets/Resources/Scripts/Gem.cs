using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour {

    void Update() {
        if (transform.position.x < -25)
            Destroy(gameObject);
        else
            transform.Translate(-SkyscraperSpawner.speed * Time.deltaTime, 0, 0, Space.World);
        // BORA.Note: I didn't add rotation feature for gems 
        // because they a too smaal to notice that they're rotating
    }

    void OnTriggerEnter(Collider other) {
        // BORA.1 Give 5 coins to heli for a gem
        other.transform.parent.GetComponent<HeliController>().PickupCoin(5);
        Destroy(gameObject);
    }
}

