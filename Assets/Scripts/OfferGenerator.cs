using UnityEngine;
using static CharacterGenerator;

public static class OfferGenerator
{
    private static int[] offerBrackets = new int[] { 500, 1500, 5000, 10000 };
    private static int[] statBrackets = new int[] { 5, 10, 15, 20 };

    public static OfferData GenerateOffer(int value)
    {
        int bracket = 0;
        for (int i = 0; i < offerBrackets.Length; i++)
        {
            if (offerBrackets[i] > value) break;
            bracket = i;
        }

        var unit = new OfferData()
        {
            Name = GenerateName(),
            Weapon = new ItemData
            {
                Name = GetAdjective() + " Sword",
                Strength = RandomStat(bracket),
                Health = RandomStat(bracket),
                Speed = RandomStat(bracket),
                Rarity = bracket
            },
            Armor = new ItemData
            {
                Name = GetAdjective() + " Armor",
                Strength = RandomStat(bracket),
                Health = RandomStat(bracket),
                Speed = RandomStat(bracket),
                Rarity = bracket
            }
        };

        return unit;
    }

    private static int RandomStat(int bracket)
    {
        int from = 0;
        int to = statBrackets[bracket];
        if (bracket != 0) from = statBrackets[bracket - 1];

        return Random.Range(from, to);
    }
}
