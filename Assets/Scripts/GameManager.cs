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


    // References
    public UnitData PlayerData { get; private set; }


    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Generate the player
            PlayerData = CharacterGenerator.GenerateCharacter(0);
            numberOfFights = 0;

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
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

        // Spawn the player on the map
        GameObject playerGO = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        Gladiator playerGlad = playerGO.GetComponent<Gladiator>();
        playerGlad.SetUnitData(PlayerData);
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

        UIManager.instance.EnableStats(false);
        // Start the arena loop
        StartCoroutine(ArenaLoop());
    }

    void StartBarracks() {
        UIManager.instance.EnableStats(true);
    }

    private IEnumerator ArenaLoop() {

        // Start the loop
        while(LivingGladiators.Count > 1 && player.IsAlive) {
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        numberOfFights++;

        SceneManager.LoadScene("Barracks");
        // Stop the arena
    }
}
