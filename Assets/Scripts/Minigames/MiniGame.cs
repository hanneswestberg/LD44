using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;
using UnityEngine.UI;

public class MiniGame : MonoBehaviour
{
    public Action<int, int, int> OnMiniGameFinish;

    [Header("Panels:")]
    [SerializeField] protected GameObject miniGameParent;
    [SerializeField] protected GameObject tutorialPanel;
    [SerializeField] protected GameObject resultPanel;

    [Space(10)]
    [SerializeField] protected Text resultHeadingText;
    [SerializeField] protected Text resultDescriptionText;

    [Space(10)]
    [SerializeField] private RectTransform marker;
    [SerializeField] private RectTransform greenZone;

    [Space(10)]
    [SerializeField] private Text countText;
    [SerializeField] private Text statusText;

    [Space(10)]
    [SerializeField] private SimpleAudioEvent uiSound; 
    [SerializeField] private SimpleAudioEvent succeedSound;
    [SerializeField] private SimpleAudioEvent failSound;
    [SerializeField] private AudioSource soundScource;

    // Internal variables
    protected int results;
    protected int resultsStrength;
    protected int resultsHealth;
    protected int resultsSpeed;
    protected bool finished;
    protected GladiatorBarrack gladiator;

    public virtual void StartMiniGame(GladiatorBarrack glad) {
        tutorialPanel.SetActive(true);
        miniGameParent.SetActive(false);
        resultPanel.SetActive(false);
        gladiator = glad;
        gladiator.GetComponent<Animator>().SetBool("Training", true);
    }

    public virtual void ButtonStart() {
        tutorialPanel.SetActive(false);
        miniGameParent.SetActive(true);
        uiSound.Play(soundScource);

        StartCoroutine(MiniGameLoop());
    }

    public virtual void ButtonResults() {
        if(!finished)
            Finish(resultsStrength, resultsHealth, resultsSpeed);

        finished = true;
        uiSound.Play(UIManager.instance.GetComponent<AudioSource>());
        gladiator.Anim.SetBool("Training", false);
    }

    protected virtual void Finish(int strength, int health, int speed) {
        if(OnMiniGameFinish != null)
            OnMiniGameFinish.Invoke(strength, health, speed);

        Destroy(gameObject);
    }

    private IEnumerator MiniGameLoop() {
        var onGoing = true;
        var parentWidth = marker.parent.GetComponent<RectTransform>().sizeDelta.x / 2f;
        var inGreenZone = false;
        var placedGreenZone = false;
        var placeLeft = true;
        var totalCount = 0;
        var successCount = 0;
        var hasTouchedEnd = false;

        var difficulty = GameManager.instance.PlayerData.CombinedStats();

        countText.text = "Count: " + 0;
        statusText.text = "";

        var currentSpeed = 1f;

        // While the minigame is ongoing
        while(onGoing) {

            // Place the green zone
            if(!placedGreenZone && hasTouchedEnd) {

                var minSize = Mathf.Clamp(100 - difficulty * 1.5f, 35, 80);
                var maxSize = Mathf.Clamp(140 - difficulty * 1.5f, 50, 120);

                greenZone.sizeDelta = new Vector2(Random.Range(minSize, maxSize), greenZone.sizeDelta.y);
                greenZone.anchoredPosition = new Vector2(Random.Range((placeLeft) ? -greenZone.sizeDelta.x : greenZone.sizeDelta.x, (placeLeft) ? -maxSize + greenZone.sizeDelta.x / 2 : maxSize - greenZone.sizeDelta.x / 2), greenZone.anchoredPosition.y);

                placedGreenZone = true;
                placeLeft = !placeLeft;
            }

            currentSpeed = Mathf.Lerp(currentSpeed, currentSpeed + (0.01f * totalCount), 0.1f);

            // Move the marker
            marker.anchoredPosition = new Vector2(Mathf.Sin((Time.time * 1.6f + (0.05f * difficulty)) + currentSpeed) * parentWidth, marker.anchoredPosition.y);

            // Wait for the marker to touch an end before starting
            if(!hasTouchedEnd) {
                greenZone.sizeDelta = new Vector2(0f, greenZone.sizeDelta.y);

                if(Mathf.Abs(marker.anchoredPosition.x) > parentWidth - 30f) {
                    hasTouchedEnd = true;
                    placeLeft = (marker.anchoredPosition.x > 0);
                }
            }


            // See if we are in the green zone
            if(hasTouchedEnd 
                &&!inGreenZone
                && marker.anchoredPosition.x < (greenZone.anchoredPosition.x + greenZone.sizeDelta.x / 2)
                && marker.anchoredPosition.x > greenZone.anchoredPosition.x - greenZone.sizeDelta.x / 2) {
                inGreenZone = true;
            }

            if(hasTouchedEnd
                && inGreenZone
                && (marker.anchoredPosition.x > (greenZone.anchoredPosition.x + greenZone.sizeDelta.x / 2)
                || marker.anchoredPosition.x < greenZone.anchoredPosition.x - greenZone.sizeDelta.x / 2)) {
                inGreenZone = false;
                totalCount++;
                placedGreenZone = !placedGreenZone;
                countText.text = "Count: " + totalCount;
                statusText.text = "Fail!";
                gladiator.Anim.SetTrigger("Fail");
                gladiator.PlayLiftSound(false);

                failSound.Play(soundScource);

                statusText.color = Color.red;
            }

            if((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) && inGreenZone && hasTouchedEnd) {
                placedGreenZone = !placedGreenZone;
                inGreenZone = false;
                successCount++;
                totalCount++;
                countText.text = "Count: " + totalCount;
                statusText.text = "Success!";
                gladiator.Anim.SetTrigger("Success");
                gladiator.PlayLiftSound(true);

                succeedSound.Play(soundScource);

                statusText.color = Color.green;
            }

            if(totalCount >= 10) {
                onGoing = false;
            }
            
            yield return null;
        }

        // calculate results
        if(successCount < 3)
            results = -3;
        else if(successCount < 5)
            results = -2;
        else if(successCount < 7)
            results = -1;
        else if(successCount <= 8)
            results = 1;
        else if(successCount <= 9)
            results = 2;
        else if(successCount <= 10)
            results = 3;

        // Add stats
        if(results > 0) {
            for(int i = 0; i < results; i++) {
                float rand = Random.Range(0, 1f);

                if(rand < 0.33f)
                    resultsStrength++;
                else if(rand < 0.66f)
                    resultsHealth++;
                else {
                    resultsSpeed++;
                }
            }
        } else {
            // Remove stats
            for(int i = 0; i < Mathf.Abs(results); i++) {
                float rand = Random.Range(0f, 1f);

                if(rand < 0.33f)
                    resultsStrength--;
                else if(rand < 0.66f)
                    resultsHealth--;
                else {
                    resultsSpeed--;
                }
            }
        }

        // Set statistics
        resultPanel.SetActive(true);
        resultHeadingText.text = "The training was a " + ((results > 0) ? "Success" : "Failure");
        resultHeadingText.color = (results > 0) ? Color.green : Color.red;

        string descText = (resultsStrength != 0) ? (resultsStrength > 0) ? "Gain " + "+" + resultsStrength + " Strength \n" : "Lose " + resultsStrength + " Strength \n" : "";
        descText += (resultsHealth != 0) ? (resultsHealth > 0) ? "Gain " + "+" + resultsHealth + " Health \n" : "Lose " + resultsHealth + " Health \n" : "";
        descText += (resultsSpeed != 0) ?  (resultsSpeed > 0) ? "Gain " + "+" + resultsSpeed + " Speed \n" : "Lose " + resultsSpeed + " Speed \n" : "";

        resultDescriptionText.text = descText;
        resultDescriptionText.color = (results > 0) ? Color.green : Color.red;
    }
}
