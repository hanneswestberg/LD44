using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Settings:")]
    [SerializeField] private Button toggleMusicButton;
    [SerializeField] private Button toggleSoundButton;

    [Header("Player Stats:")]
    [SerializeField] private Text playerValue;
    [SerializeField] private Text playerHype;
    [SerializeField] private Text playerStatStrength;
    [SerializeField] private Text playerStatHealth;
    [SerializeField] private Text playerStatSpeed;

    // Internal variables:
    private bool isMusicOn = true;
    private bool isSoundOn = true;


    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
    }

    public void ToggleMusic() {
        isMusicOn = !isMusicOn;
        // Do stuff here
    }

    public void ToggleSounds() {
        isSoundOn = !isSoundOn;
        // Do stuff here
    }
}
