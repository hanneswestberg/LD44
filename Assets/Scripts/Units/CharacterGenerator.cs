﻿using UnityEngine;
using System.IO;


public static class CharacterGenerator
{
    public static UnitData GenerateCharacter(int baseStats, int extraStats) {

        var unit = new UnitData() {
            Strength = baseStats,
            Health = baseStats,
            Speed = baseStats,
            Hype = 1,
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

    public static string GenerateName() {
        var allNames = Resources.Load<TextAsset>("Names");
        string[] names = allNames.text.Split('\n');

        return UppercaseFirst(names[Random.Range(0, names.Length)]) + " the " + GetAdjective();
    }

    public static string GetAdjective() {
        var allAdj = Resources.Load<TextAsset>("Adjectives");
        string[] adj = allAdj.text.Split('\n');

        return UppercaseFirst(adj[Random.Range(0, adj.Length)]);
    }

    static string UppercaseFirst(string s) {
        if(string.IsNullOrEmpty(s)) {
            return string.Empty;
        }
        return char.ToUpper(s[0]) + s.Substring(1);
    }
}
