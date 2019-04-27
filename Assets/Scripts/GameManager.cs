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

    [Header("Debug Settings")]
    [SerializeField] private bool debugMode;

    [Header("References")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;

    // Arena variables
    public List<Gladiator> LivingGladiators { get; private set; }
    private Gladiator player;
    private int numberOfFights;


    // References
    public UnitData playerData { get; private set; }


    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
        // Generate the player
        playerData = CharacterGenerator.GenerateCharacter(0);
        numberOfFights = 0;

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    // Start is called before the first frame update
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {

        // Check if debug mode, otherwise we start on the correct scene
        if(debugMode) {
            // Start the arena if we are debugging
            if(SceneManager.GetActiveScene().name == "Arena")
                StartArena();
        }
        else {
            // Start the real game with the correct scene
            if(SceneManager.GetActiveScene().name == "Arena")
                StartArena();
            else if(SceneManager.GetActiveScene().name == "Barracks")
                StartBarracks();

        }
    }

    /// <summary>
    /// Starts the arena fight
    /// </summary>
    void StartArena() {
        // Initialize
        LivingGladiators = new List<Gladiator>();

        // Spawn the player on the map
        GameObject playerGO = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        Gladiator playerGlad = playerGO.GetComponent<Gladiator>();
        playerGlad.SetUnitData(playerData);
        playerGO.name = "Player";
        player = playerGlad;
        LivingGladiators.Add(playerGlad);

        // Spawn the enemies on the map
        for(int i = 0; i < 4; i++) {
            NavMeshHit hit;
            NavMesh.SamplePosition(Random.insideUnitSphere * 15f, out hit, Mathf.Infinity, NavMesh.AllAreas);
            GameObject enemyGO = Instantiate(enemyPrefab, hit.position, Quaternion.identity);
            enemyGO.GetComponent<Gladiator>().SetUnitData(CharacterGenerator.GenerateCharacter(numberOfFights * 3));
            enemyGO.name = string.Format("Gladiator Enemy {0}", i);
            LivingGladiators.Add(enemyGO.GetComponent<Gladiator>());
        }

        // Start the arena loop
        StartCoroutine(ArenaLoop());
    }

    void StartBarracks() {

    }

    private IEnumerator ArenaLoop() {

        // Start the loop
        while(LivingGladiators.Count > 1 && player.IsAlive) {
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        numberOfFights++;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        StartArena();
        // Stop the arena
    }
}
