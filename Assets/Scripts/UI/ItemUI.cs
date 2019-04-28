using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Text Name;
    public Text Strength;
    public Text Health;
    public Text Speed;

    private RectTransform pos;

    private Color[] rarityColors = new Color[] {
        new Color(0.80f, 0.50f, 0.20f), // bronze
        new Color(0.75f, 0.75f, 0.75f), // silver
        new Color(1.0f, 0.84f, 0.0f), // gold
        new Color(0.44f, 0.82f, 0.88f) // diamond blue
    };

    public void NewValues(ItemData data, float offset = 0)
    {
        if (pos == null) pos = GetComponent<RectTransform>();

        Name.text = data.Name;
        Name.color = rarityColors[data.Rarity];
        Strength.text = "Str " + data.Strength;
        Health.text = "Hp " + data.Health;
        Speed.text = "Spd " + data.Speed;

        pos.anchoredPosition = new Vector2(0, offset);
    }
}
