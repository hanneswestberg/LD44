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

    [Space(10)]
    [SerializeField] private GameObject playerStats;

    [Space(10)]
    [SerializeField] private Text playerStatStrength;
    [SerializeField] private Text playerStatHealth;
    [SerializeField] private Text playerStatSpeed;

    [Space(10)]
    [SerializeField] private Text playerWeaponName;
    [SerializeField] private Text playerWeaponStrength;
    [SerializeField] private Text playerWeaponHealth;
    [SerializeField] private Text playerWeaponSpeed;

    [Space(10)]
    [SerializeField] private Text playerArmorName;
    [SerializeField] private Text playerArmorStrength;
    [SerializeField] private Text playerArmorHealth;
    [SerializeField] private Text playerArmorSpeed;

    // Internal variables:
    private bool isMusicOn = true;
    private bool isSoundOn = true;
    private UnitData playerData;

    private Color[] rarityColors = new Color[] {
        new Color(0.80f, 0.50f, 0.20f), // bronze
        new Color(0.75f, 0.75f, 0.75f), // silver
        new Color(1.0f, 0.84f, 0.0f), // gold
        new Color(0.44f, 0.82f, 0.88f) // diamond blue
    };

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
    }

    private void Start() {
        playerData = GameManager.instance.PlayerData;

        if(playerData != null) {
            UpdateUI();
        }
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
    }

    public void UpdateUI()
    {
        playerValue.text = "Value " + playerData.LifeValue + " denarii";
        playerHype.text = "Hype " + playerData.Hype + "x";

        // stats
        playerStatStrength.text = "Strength " + playerData.Strength;
        playerStatHealth.text = "Health " + playerData.Health;
        playerStatSpeed.text = "Speed " + playerData.Speed;

        // weapon
        playerWeaponName.text = playerData.Weapon.Name;
        playerWeaponName.color = rarityColors[playerData.Weapon.Rarity];
        playerWeaponStrength.text = "Str " + playerData.Weapon.Strength;
        playerWeaponHealth.text = "Hp " + playerData.Weapon.Health;
        playerWeaponSpeed.text = "Spd " + playerData.Weapon.Speed;
        // armor
        playerArmorName.text = playerData.Armor.Name;
        playerArmorName.color = rarityColors[playerData.Armor.Rarity];
        playerArmorStrength.text = "Str " + playerData.Armor.Strength;
        playerArmorHealth.text = "Hp " + playerData.Armor.Health;
        playerArmorSpeed.text = "Spd " + playerData.Armor.Speed;
    }
}
