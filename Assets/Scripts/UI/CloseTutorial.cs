using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTutorial : MonoBehaviour
{
    public void Close() {
        GameManager.instance.tutorial = false;
        gameObject.SetActive(false);
    }
}
