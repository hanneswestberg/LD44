using UnityEngine;

public static class CharacterGenerator
{
    public static UnitData GenerateCharacter(int extraStats) {

        var unit = new UnitData() {
            Strength = 5,
            Health = 5,
            Speed = 5,
            Hype = 0,
            LifeValue = 100
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
