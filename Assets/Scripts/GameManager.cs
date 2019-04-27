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

    // References
    private UnitData playerData;


    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {
        // Generate the player
        playerData = CharacterGenerator.GenerateCharacter();

        // Check if debug mode, otherwise we start on the correct scene
        if(debugMode) {

            // Start the arena if we are debugging
            if(SceneManager.GetActiveScene().name == "Arena")
                StartArena();
        }
        else {
            // Start the real game with the correct scene
        }
    }

    /// <summary>
    /// Starts the arena fight
    /// </summary>
    void StartArena() {
        // Initialize
        LivingGladiators = new List<Gladiator>();

        // Spawn the player on the map
        GameObject playerGO = Instantiate(playerPrefab, Random.insideUnitSphere * 15f, Quaternion.identity);
        playerGO.GetComponent<Gladiator>().SetUnitData(playerData);
        playerGO.name = "Player";
        LivingGladiators.Add(playerGO.GetComponent<Gladiator>());

        // Spawn the enemies on the map
        for(int i = 0; i < 4; i++) {
            NavMeshHit hit;
            NavMesh.SamplePosition(Random.insideUnitSphere * 15f, out hit, Mathf.Infinity, NavMesh.AllAreas);
            GameObject enemyGO = Instantiate(enemyPrefab, hit.position, Quaternion.identity);
            enemyGO.GetComponent<Gladiator>().SetUnitData(CharacterGenerator.GenerateCharacter());
            enemyGO.name = string.Format("Gladiator Enemy {0}", i);
            LivingGladiators.Add(enemyGO.GetComponent<Gladiator>());
        }

        // Start the arena loop
        StartCoroutine(ArenaLoop());
    }

    private IEnumerator ArenaLoop() {

        // Start the loop
        while(LivingGladiators.Count > 1) {

            yield return null;
        }

        // Stop the arena
    }
}
