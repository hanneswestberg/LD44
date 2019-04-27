using UnityEngine;

public static class CharacterGenerator
{
    public static UnitData GenerateCharacter(int extraStats) {

        var unit = new UnitData() {
            Strength = 5,
            Health = 5,
            Speed = 5,
            Hype = 0,
            LifeValue = 100,
            Weapon = new ItemData {
                Name = "Shitty Sword",
                Strength = 1,
                Health = 0,
                Speed = 0,
                Rarity = 0
            },
            Armor = new ItemData
            {
                Name = "Leather scraps",
                Strength = 0,
                Health = 1,
                Speed = 0,
                Rarity = 3
            }
        };

        for(int i = 0; i < extraStats; i++) {
            var rand = Random.Range(0f, 1f);

            if(rand < 0.33f) {
                unit.Strength++;
            }
            else if(rand < 0.66f) {
                unit.Health++;
            }
            else {
                unit.Speed++;
            }
        }

        return unit;
    }
}
