using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargets : MonoBehaviour
{

    public List<Gladiator> ValidTargets { get; private set; }

    private void Start() {
        ValidTargets = new List<Gladiator>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Enemy") {
            ValidTargets.Add(other.GetComponent<Gladiator>());
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Enemy") {
            ValidTargets.Remove(other.GetComponent<Gladiator>());
        }
    }
}
