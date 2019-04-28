using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targets : MonoBehaviour
{

    public List<Gladiator> ValidTargets { get; private set; }

    private void Start() {
        ValidTargets = new List<Gladiator>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player") {
            ValidTargets.Add(other.GetComponent<Gladiator>());
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player") {
            ValidTargets.Remove(other.GetComponent<Gladiator>());
        }
    }
}
