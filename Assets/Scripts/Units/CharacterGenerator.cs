public static class CharacterGenerator
{
    public static UnitData GenerateCharacter() {

        // TODO: Some more fancy logic

        return new UnitData() {
            Strength = 5,
            Health = 5,
            Speed = 5,
            Hype = 0,
            LifeValue = 100
        };
    }
}
