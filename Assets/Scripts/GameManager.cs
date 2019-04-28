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
    [SerializeField] private GameObject barrackGladiatorPrefab;
    [SerializeField] private GameObject miniGamePrefab;

    [Space(20)]
    [SerializeField] private AudioSource arenaSource;
    [SerializeField] private AudioSource barracksSource;
    [SerializeField] private SimpleAudioEvent offerSound;

    // Arena variables
    public List<Gladiator> LivingGladiators { get; private set; }
    private Gladiator player;
    [HideInInspector] public GladiatorBarrack barrackGladiator;
    private int numberOfFights;
    private int numberOfPlayerKills;
    private float oldValue;
    private bool lostLastFight;
    private bool firstStart = true;
    public bool tutorial = true;

    // References
    public UnitData PlayerData { get; private set; }

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Generate the player
            PlayerData = CharacterGenerator.GenerateCharacter(3, 3);
            PlayerData.Name = CharacterGenerator.GenerateName();
            PlayerData.SkinColor = Color.Lerp(new Color(53f, 36f, 21f) / 255f, new Color(231f, 178f, 132f) / 255f, Random.Range(0f, 1f));

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
        if(SceneManager.GetActiveScene().name == "Arena") {
            arenaSource.volume = 0.1f;
            barracksSource.volume = 0;
            StartArena();
        }
        else if(SceneManager.GetActiveScene().name == "Barracks") {
            arenaSource.volume = 0f;
            barracksSource.volume = 0.1f;
            StartBarracks();
        }
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
        playerGlad.CanMove = false;
        playerGlad.SetUnitData(PlayerData);
        playerGO.name = "Player";
        player = playerGlad;
        playerGlad.OnUnitKilled += () => { numberOfPlayerKills++; };
        LivingGladiators.Add(playerGlad);

        // Spawn the enemies on the map
        for(int i = 0; i < (4 + numberOfFights); i++) {
            NavMeshHit hit;
            NavMesh.SamplePosition(Random.insideUnitSphere * 15f, out hit, Mathf.Infinity, NavMesh.AllAreas);
            GameObject enemyGO = Instantiate(enemyPrefab, hit.position, Quaternion.identity);
            UnitData enemyData = CharacterGenerator.GenerateCharacter(1, 6 + numberOfFights * 3);
            enemyData.SkinColor = Color.Lerp(new Color(53f, 36f, 21f) / 255f, new Color(231f, 178f, 132f) / 255f, Random.Range(0f, 1f));
            enemyData.Name = CharacterGenerator.GenerateName();
            enemyGO.GetComponent<Gladiator>().SetUnitData(enemyData);
            enemyGO.name = string.Format("Gladiator Enemy {0}", i);
            LivingGladiators.Add(enemyGO.GetComponent<Gladiator>());
            enemyGO.GetComponent<Gladiator>().CanMove = false;

        }

        if (firstStart)
        {
            UIManager.instance.ShowArenaTutorial();
            StartCoroutine(TutorialPause());
        }
        else
        {
            // Start the arena loop
            StartCoroutine(ArenaLoop());
        }
        firstStart = false;
    }

    /// <summary>
    /// Starts the baracks
    /// </summary>
    void StartBarracks() {
        UIManager.instance.EnableStats(true);

        GameObject gladGO = Instantiate(barrackGladiatorPrefab, new Vector3(0.75f, 0, -0.9f), Quaternion.Euler(new Vector3(0, -140f, 0)));
        barrackGladiator = gladGO.GetComponent<GladiatorBarrack>();

        if(lostLastFight) {
            var rand = Random.Range(0f, 1f);
            if(rand <= 0.33f) {
                UIManager.instance.NewInjury("Strength", Random.Range(1, 3));
            }
            else if(rand <= 0.66f) {
                UIManager.instance.NewInjury("Health", Random.Range(1, 3));
            }
            else {
                UIManager.instance.NewInjury("Speed", Random.Range(1, 3));
            }
        }

        if(!firstStart) {
            bool hasOffer = CheckOffers(PlayerData.LifeValue, oldValue);
            if(!hasOffer) StartMiniGame();
        }
        else {
            UIManager.instance.ShowCharacterCreator();
        }

    }

    IEnumerator TutorialPause() {
        while(tutorial)
        {
            yield return null;
        }
        StartCoroutine(ArenaLoop());
    }

    public void CharacterDone() {
        StartCoroutine(LoadSceneDelay("Arena"));
    }

    public void StartMiniGame() {
        GameObject miniGameGO = Instantiate(miniGamePrefab, Vector3.zero, Quaternion.identity);
        MiniGame miniGame = miniGameGO.GetComponent<MiniGame>();
        miniGame.StartMiniGame(barrackGladiator);

        miniGame.OnMiniGameFinish += (srt, hp, spd) => {
            PlayerData.Strength += srt;
            PlayerData.Health += hp;
            PlayerData.Speed += spd;
            UIManager.instance.UpdateUI();
            StartCoroutine(LoadSceneDelay("Arena"));
        };
    }

    private IEnumerator ArenaLoop() {

        yield return new WaitForSeconds(1f);
        foreach(var gladiator in LivingGladiators) {
            gladiator.CanMove = true;
        }

        // Start the loop
        while(LivingGladiators.Count > 1 && player.IsAlive) {
            yield return null;
        }

        float newValue = 0;
        oldValue = PlayerData.LifeValue;

        if (player.IsAlive)
        {
            newValue = oldValue + (PlayerData.CombinedStats() * 2 * PlayerData.Hype);
            PlayerData.Hype += numberOfPlayerKills;
            lostLastFight = false;
        }
        else
        {
            newValue =  oldValue - (LivingGladiators.Count * 10);
            PlayerData.Hype = 1;
            lostLastFight = true;
        }


        numberOfFights++;
        PlayerData.LifeValue = newValue;

        UIManager.instance.UpdateUI();

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("Barracks");
    }

    private IEnumerator LoadSceneDelay(string scene) {
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(scene);
    }

    public void TakeOffer(OfferData data)
    {
        PlayerData.Weapon = data.Weapon;
        PlayerData.Armor = data.Armor;
        UIManager.instance.UpdateUI();
        StartMiniGame();
    }

    private bool CheckOffers(float newValue, float oldValue)
    {
        float rng = Random.Range(0.0f, 1.0f);
        if (rng < 0.5) return false; // no offers today

        if (newValue >= oldValue)
        {
            offerSound.Play(UIManager.instance.GetComponent<AudioSource>());
            OfferData off0 = OfferGenerator.GenerateOffer(newValue);
            OfferData off1 = OfferGenerator.GenerateOffer(newValue);
            UIManager.instance.NewOffers(off0, off1);
        }
        else
        {
            OfferData off0 = OfferGenerator.GenerateOffer(newValue);
            UIManager.instance.Sold(off0);
        }
        return true;
    }

}
