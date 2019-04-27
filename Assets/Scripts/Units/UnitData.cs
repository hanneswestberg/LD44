public class ItemData
{
    public string Name { get; set; }

    public int Strength { get; set; }
    public int Health { get; set; }
    public int Speed { get; set; }

    // 0 - 3, bronze, silver, gold, diamond
    public int Rarity { get; set; }
}

public class UnitData
{
    public int Strength { get; set; }
    public int Health { get; set; }
    public int Speed { get; set; }

    public float Hype { get; set; }
    public float LifeValue { get; set; }

    public ItemData Weapon { get; set; }
    public ItemData Armor { get; set; }
}
