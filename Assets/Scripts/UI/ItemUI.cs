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
        new Color(0.5f, 0.5f, 0.5f), // gray
        new Color(0.0f, 0.90f, 0.25f), // green
        new Color(0.65f, 0.22f, 0.99f), // gold
        new Color(0.01f, 0.71f, 1.0f) // diamond blue
    };

    public void NewValues(ItemData data, float offset = 0)
    {
        if (pos == null) pos = GetComponent<RectTransform>();

        Name.text = data.Name;
        Name.color = rarityColors[data.Rarity];
        Strength.text = "Str " + data.Strength;
        Strength.color = (data.Strength > 0) ? Color.green : Color.red;
        Health.text = "Hp " + data.Health;
        Health.color = (data.Health > 0) ? Color.green : Color.red;
        Speed.text = "Spd " + data.Speed;
        Speed.color = (data.Speed > 0) ? Color.green : Color.red;

        pos.anchoredPosition = new Vector2(0, offset);
    }
}
