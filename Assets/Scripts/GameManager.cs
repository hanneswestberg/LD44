using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // This script handles:
    // Initialization of the game
    // Player generation and storing of the stats
    // Scene loading
    // Keeping track of progress

    public static GameManager instance;

    [Header("References")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;

    // Arena variables
    public List<Gladiator> LivingGladiators { get; private set; }
    private Gladiator player;
    private int numberOfFights;
    private int numberOfPlayerKills;

    // References
    public UnitData PlayerData { get; private set; }

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Generate the player
            PlayerData = CharacterGenerator.GenerateCharacter(5, 0);
            PlayerData.Name = CharacterGenerator.GenerateName();
            numberOfFights = 0;

            // Subscribes to the scene loaded event
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Called on the load of the scene
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        // Start the real game with the correct scene
        if(SceneManager.GetActiveScene().name == "Arena")
            StartArena();
        else if(SceneManager.GetActiveScene().name == "Barracks")
            StartBarracks();
    }

    /// <summary>
    /// Starts the arena fight
    /// </summary>
    void StartArena() {
        // Initialize
        LivingGladiators = new List<Gladiator>();
        UIManager.instance.EnableStats(false);
        numberOfPlayerKills = 0;

        // Spawn the player on the map
        NavMeshHit playerhit;
        NavMesh.SamplePosition(Random.insideUnitSphere * 15f, out playerhit, Mathf.Infinity, NavMesh.AllAreas);
        GameObject playerGO = Instantiate(playerPrefab, playerhit.position, Quaternion.identity);
        Gladiator playerGlad = playerGO.GetComponent<Gladiator>();
        playerGlad.SetUnitData(PlayerData);
        playerGO.name = "Player";
        player = playerGlad;
        playerGlad.OnUnitKilled += () => { numberOfPlayerKills++; };
        LivingGladiators.Add(playerGlad);

        // Spawn the enemies on the map
        for(int i = 0; i < 4; i++) {
            NavMeshHit hit;
            NavMesh.SamplePosition(Random.insideUnitSphere * 15f, out hit, Mathf.Infinity, NavMesh.AllAreas);
            GameObject enemyGO = Instantiate(enemyPrefab, hit.position, Quaternion.identity);
            UnitData enemyData = CharacterGenerator.GenerateCharacter(1, 6 + numberOfFights * 3);
            enemyData.Name = CharacterGenerator.GenerateName();
            enemyGO.GetComponent<Gladiator>().SetUnitData(enemyData);
            enemyGO.name = string.Format("Gladiator Enemy {0}", i);
            LivingGladiators.Add(enemyGO.GetComponent<Gladiator>());
        }

        // Start the arena loop
        StartCoroutine(ArenaLoop());
    }

    /// <summary>
    /// Starts the baracks
    /// </summary>
    void StartBarracks() {
        UIManager.instance.EnableStats(true);

        StartCoroutine(BaracksLoop());
    }

    void StartMiniGameStrength() {
        StartCoroutine(MiniGameStrengthLoop());
    }

    void StartMiniGameHealth() {
        StartCoroutine(MiniGameHealthLoop());
    }

    void StartMiniGameSpeed() {
        StartCoroutine(MiniGameSpeedLoop());
    }

    private IEnumerator ArenaLoop() {
        // Start the loop
        while(LivingGladiators.Count > 1 && player.IsAlive) {
            yield return null;
        }

        numberOfFights++;
        // Give hype to the player depending on the fight
        PlayerData.Hype += numberOfPlayerKills;
        // Calculate life value
        PlayerData.LifeValue = PlayerData.CombinedStats() * PlayerData.Hype;

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("Barracks");
    }

    private IEnumerator BaracksLoop() {

        // Here we wait for the player to choose offer and minigame

        yield return null;
    }

    private IEnumerator MiniGameStrengthLoop() {
        var success = false;
        var onGoing = false;

        // While the minigame is ongoing
        while(onGoing) {
            // Calculate success here
            success = true;

            yield return null;
        }

        // Give reward to player if won;
        PlayerData.Strength += (success) ? 1 : 0;
    }

    private IEnumerator MiniGameHealthLoop() {
        var success = false;
        var onGoing = false;

        // While the minigame is ongoing
        while(onGoing) {
            // Calculate success here
            success = true;

            yield return null;
        }

        // Give reward to player if won;
        PlayerData.Health += (success) ? 1 : 0;
    }

    private IEnumerator MiniGameSpeedLoop() {
        var success = false;
        var onGoing = false;

        // While the minigame is ongoing
        while(onGoing) {
            // Calculate success here
            success = true;

            yield return null;
        }

        // Give reward to player if won;
        PlayerData.Speed += (success) ? 1 : 0;
    }
}
