using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public UIOfferGenerator offerGenerator;

    [SerializeField] private GameObject enterScreen;
    [SerializeField] private GameObject arenaTutorial;

    [Header("Settings:")]
    [SerializeField] private Button toggleMusicButton;
    [SerializeField] private Button toggleSoundButton;

    [Header("Player Stats:")]
    [SerializeField] private Text playerValue;
    [SerializeField] private Text playerHype;

    [Space(10)]
    [SerializeField] private GameObject playerStats;

    [Space(10)]
    [SerializeField] private Text playerStatStrength;
    [SerializeField] private Text playerStatHealth;
    [SerializeField] private Text playerStatSpeed;

    [SerializeField] private GameObject itemPrefab;

    [Header("Injury")]
    [SerializeField] private GameObject injuryPanel;
    [SerializeField] private Text injuryText;

    [Header("Character Creator")]
    [SerializeField] private GameObject characterCreatorPanel;
    [SerializeField] private Text characterNameText;

    [SerializeField] private SimpleAudioEvent uiSound;


    private ItemUI weapon;
    private ItemUI armor;
    private AudioSource audioSource;

    // Internal variables:
    private bool isMusicOn = true;
    private bool isSoundOn = true;
    private UnitData playerData;

    private string currentInjury;
    private int currentAmount;

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            injuryPanel.SetActive(false);
            characterCreatorPanel.SetActive(false);
            audioSource = GetComponent<AudioSource>();

        }
        else {
            Destroy(this.gameObject);
        }
    }

    private void Start() {
        playerData = GameManager.instance.PlayerData;
        characterNameText.text = "Name: <color=green>" + playerData.Name + "</color>";

        GameObject weapon_inst = Instantiate(itemPrefab, playerStats.transform);
        GameObject armor_inst = Instantiate(itemPrefab, playerStats.transform);

        weapon = weapon_inst.GetComponent<ItemUI>();
        armor = armor_inst.GetComponent<ItemUI>();

        if (playerData != null) {
            UpdateUI();
        }

        Instantiate(enterScreen);
    }

    public void ShowArenaTutorial()
    {
        Instantiate(arenaTutorial);
    }

    public void ShowCharacterCreator() {
        characterCreatorPanel.SetActive(true);
    }

    public void NewNameButton() {
        playerData.Name = CharacterGenerator.GenerateName();
        characterNameText.text = "Name: <color=green>" + playerData.Name + "</color>";
        uiSound.Play(audioSource);
    }

    public void NewSkinButton() {
        playerData.SkinColor = Color.Lerp(new Color(53f, 36f, 21f) / 255f, new Color(231f, 178f, 132f) / 255f, Random.Range(0f, 1f));
        GameManager.instance.barrackGladiator.UpdateMaterials();
        uiSound.Play(audioSource);
    }

    public void CharacterDone() {
        GameManager.instance.CharacterDone();
        characterCreatorPanel.SetActive(false);
        uiSound.Play(audioSource);
    }

    public void EnableStats(bool enable) 
    {
        playerStats.SetActive(enable);
    }

    public void ToggleMusic() {
        isMusicOn = !isMusicOn;
        // Do stuff here
    }

    public void ToggleSounds() {
        isSoundOn = !isSoundOn;
        // Do stuff here
        AudioListener.pause = !isSoundOn;
    }

    public void NewOffers(OfferData off0, OfferData off1)
    {
        offerGenerator.CreateOffer(off0);
        offerGenerator.CreateOffer(off1);
    }

    public void InjuryButton() {
        if(currentInjury != "") {
            injuryPanel.SetActive(false);

            if(currentInjury == "Strength") {
                playerData.Strength -= currentAmount;
            }
            else if(currentInjury == "Health") {
                playerData.Health -= currentAmount;
            }
            else if(currentInjury == "Speed") {
                playerData.Speed -= currentAmount;
            }
            currentInjury = "";
            uiSound.Play(audioSource);
            UpdateUI();
        }
    }

    public void NewInjury(string injury, int amount) {
        injuryPanel.SetActive(true);
        injuryText.text = amount + " " + injury;
        currentInjury = injury;
        currentAmount = amount;
    }

    public void Sold(OfferData offer)
    {
        offerGenerator.CreateSold(offer);
    }

    public void UpdateUI()
    {
        playerValue.text = "Value " + playerData.LifeValue + " denarii";
        playerHype.text = "Hype " + playerData.Hype + "x";

        // stats
        playerStatStrength.text = "Strength " + playerData.Strength;
        playerStatHealth.text = "Health " + playerData.Health;
        playerStatSpeed.text = "Speed " + playerData.Speed;

        weapon.NewValues(playerData.Weapon);
        armor.NewValues(playerData.Armor, -70);
    }

}
