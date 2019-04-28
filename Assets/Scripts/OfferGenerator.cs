using UnityEngine;
using static CharacterGenerator;

public static class OfferGenerator
{
    private static int[] offerBrackets = new int[] { 500, 1500, 5000, 10000 };
    private static int[] statBrackets = new int[] { 5, 10, 15, 20 };

    public static OfferData GenerateOffer(float value)
    {
        int bracket = 0;
        for (int i = 0; i < offerBrackets.Length; i++)
        {
            bracket = i;
            if (offerBrackets[i] > value) break;
        }

        var unit = new OfferData()
        {
            Name = GenerateName(),
            Weapon = WeightedWeapon(statBrackets[bracket], bracket),
            Armor = WeightedArmor(statBrackets[bracket], bracket)
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

    private static ItemData WeightedWeapon(int stats, int bracket) {
        var data = new ItemData();
        data.Name = GetAdjective() + " Sword";
        data.Rarity = bracket;

        for(int i = 0; i < stats; i++) {
            var rand = Random.Range(0f, 1f);
            var rand2 = Random.Range(0f, 1f);

            if(rand <= 0.5f)
                data.Strength++;
            else if(rand <= 0.7f & rand2 <= 0.5f) {
                data.Health++;
            }
            else if(rand <= 0.7f & rand2 > 0.5f) {
                data.Health--;
                i--;
            }
            else if(rand <= 1f & rand2 <= 0.5f) {
                data.Speed++;
            }
            else if(rand <= 0.7f & rand2 > 0.5f) {
                data.Speed--;
                i--;
            }
        }
        return data;
    }

    private static ItemData WeightedArmor(int stats, int bracket) {
        var data = new ItemData();
        data.Name = GetAdjective() + " Armor";
        data.Rarity = bracket;

        for(int i = 0; i < stats; i++) {
            var rand = Random.Range(0f, 1f);
            var rand2 = Random.Range(0f, 1f);

            if(rand <= 0.5f)
                data.Health++;
            else if(rand <= 0.7f & rand2 <= 0.5f) {
                data.Strength++;
            }
            else if(rand <= 0.7f & rand2 > 0.5f) {
                data.Strength--;
                i--;
            }
            else if(rand <= 1f & rand2 <= 0.5f) {
                data.Speed++;
            }
            else if(rand <= 0.7f & rand2 > 0.5f) {
                data.Speed--;
                i--;
            }
        }
        return data;
    }
}
