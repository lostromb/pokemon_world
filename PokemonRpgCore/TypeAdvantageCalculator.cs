using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRpg
{
    public static class TypeAdvantageCalculator
    {
        public static float GetAttackTypeMultiplier(ElementalType attackingMoveType, ElementalType defendingPokemonType)
        {
            float returnVal = 1.0f;
            foreach (var defendingTypeKvp in ElementalTypeAdvantages[attackingMoveType])
            {
                if (defendingPokemonType.HasFlag(defendingTypeKvp.Key))
                {
                    returnVal *= defendingTypeKvp.Value;
                }
            }

            return returnVal;
        }

        // map from attack type -> defender type -> stat multiplier
        private static readonly Dictionary<ElementalType, Dictionary<ElementalType, float>> ElementalTypeAdvantages = new Dictionary<ElementalType, Dictionary<ElementalType, float>>()
        {
            { ElementalType.Normal, new Dictionary<ElementalType, float>() {
                { ElementalType.Rock, 0.5f },
                { ElementalType.Ghost, 0 },
                { ElementalType.Steel, 0.5f },
            } },
            { ElementalType.Fire, new Dictionary<ElementalType, float>() {
                { ElementalType.Fire, 0.5f },
                { ElementalType.Water, 0.5f },
                { ElementalType.Grass, 2 },
                { ElementalType.Ice, 2 },
                { ElementalType.Bug, 2 },
                { ElementalType.Rock, 0.5f },
                { ElementalType.Dragon, 0.5f },
                { ElementalType.Steel, 2 },
            } },
            { ElementalType.Water, new Dictionary<ElementalType, float>() {
                { ElementalType.Fire, 2f },
                { ElementalType.Water, 0.5f },
                { ElementalType.Grass, 0.5f },
                { ElementalType.Ground, 2 },
                { ElementalType.Rock, 3f },
                { ElementalType.Dragon, 0.5f },
            } },
            { ElementalType.Electric, new Dictionary<ElementalType, float>() {
                { ElementalType.Water, 2f },
                { ElementalType.Electric, 0.5f },
                { ElementalType.Grass, 0.5f },
                { ElementalType.Ground, 0 },
                { ElementalType.Flying, 2f },
                { ElementalType.Dragon, 0.5f },
            } },
            { ElementalType.Grass, new Dictionary<ElementalType, float>() {
                { ElementalType.Fire, 0.5f },
                { ElementalType.Water, 2f },
                { ElementalType.Grass, 0.5f },
                { ElementalType.Poison, 0.5f },
                { ElementalType.Ground, 2f },
                { ElementalType.Flying, 0.5f },
                { ElementalType.Bug, 0.5f },
                { ElementalType.Rock, 2f },
                { ElementalType.Dragon, 0.5f },
                { ElementalType.Steel, 0.5f },
            } },
            { ElementalType.Ice, new Dictionary<ElementalType, float>() {
                { ElementalType.Fire, 0.5f },
                { ElementalType.Water, 0.5f },
                { ElementalType.Grass, 2f },
                { ElementalType.Ice, 0.5f },
                { ElementalType.Ground, 2f },
                { ElementalType.Flying, 2f },
                { ElementalType.Dragon, 2f },
                { ElementalType.Steel, 0.5f },
            } },
            { ElementalType.Fighting, new Dictionary<ElementalType, float>() {
                { ElementalType.Normal, 2f },
                { ElementalType.Ice, 2f },
                { ElementalType.Poison, 0.5f },
                { ElementalType.Flying, 0.5f },
                { ElementalType.Psychic, 0.5f },
                { ElementalType.Bug, 0.5f },
                { ElementalType.Rock, 2f },
                { ElementalType.Ghost, 0.5f },
                { ElementalType.Dark, 2f },
                { ElementalType.Steel, 2f },
                { ElementalType.Fairy, 0.5f },
            } },
            { ElementalType.Poison, new Dictionary<ElementalType, float>() {
                { ElementalType.Grass, 2f },
                { ElementalType.Poison, 0.5f },
                { ElementalType.Ground, 0.5f },
                { ElementalType.Rock, 0.5f },
                { ElementalType.Ghost, 0.5f },
                { ElementalType.Steel, 0f },
                { ElementalType.Fairy, 2f },
            } },
            { ElementalType.Ground, new Dictionary<ElementalType, float>() {
                { ElementalType.Fire, 2f },
                { ElementalType.Electric, 2f },
                { ElementalType.Grass, 0.5f },
                { ElementalType.Poison, 2f },
                { ElementalType.Flying, 0 },
                { ElementalType.Bug, 0.5f },
                { ElementalType.Rock, 2f },
                { ElementalType.Steel, 2f },
            } },
            { ElementalType.Flying, new Dictionary<ElementalType, float>() {
                { ElementalType.Electric, 0.5f },
                { ElementalType.Grass, 2f },
                { ElementalType.Fighting, 2f },
                { ElementalType.Bug, 2f },
                { ElementalType.Rock, 0.5f },
                { ElementalType.Steel, 0.5f },
            } },
            { ElementalType.Psychic, new Dictionary<ElementalType, float>() {
                { ElementalType.Fighting, 2f },
                { ElementalType.Poison, 2f },
                { ElementalType.Flying, 0.5f },
                { ElementalType.Dark, 0 },
                { ElementalType.Steel, 0.5f },
            } },
            { ElementalType.Bug, new Dictionary<ElementalType, float>() {
                { ElementalType.Fire, 0.5f },
                { ElementalType.Grass, 2f },
                { ElementalType.Fighting, 0.5f },
                { ElementalType.Poison, 0.5f },
                { ElementalType.Flying, 0.5f },
                { ElementalType.Psychic, 2f },
                { ElementalType.Ghost, 0.5f },
                { ElementalType.Dark, 2f },
                { ElementalType.Steel, 0.5f },
                { ElementalType.Fairy, 0.5f },
            } },
            { ElementalType.Rock, new Dictionary<ElementalType, float>() {
                { ElementalType.Fire, 2f },
                { ElementalType.Ice, 2f },
                { ElementalType.Fighting, 0.5f },
                { ElementalType.Ground,  0.5f },
                { ElementalType.Flying, 2f },
                { ElementalType.Bug, 2f },
                { ElementalType.Steel, 0.5f },
            } },
            { ElementalType.Ghost, new Dictionary<ElementalType, float>() {
                { ElementalType.Normal, 0 },
                { ElementalType.Psychic, 2f },
                { ElementalType.Ghost, 2f },
                { ElementalType.Dragon,  0.5f },
            } },
            { ElementalType.Dragon, new Dictionary<ElementalType, float>() {
                { ElementalType.Dragon, 2f },
                { ElementalType.Steel, 0.5f },
                { ElementalType.Fairy, 0 },
            } },
            { ElementalType.Dark, new Dictionary<ElementalType, float>() {
                { ElementalType.Fighting, 0.5f },
                { ElementalType.Psychic, 2f },
                { ElementalType.Ghost, 2f },
                { ElementalType.Dark, 0.5f },
                { ElementalType.Fairy, 0.5f },
            } },
            { ElementalType.Steel, new Dictionary<ElementalType, float>() {
                { ElementalType.Fire, 0.5f },
                { ElementalType.Water, 0.5f },
                { ElementalType.Electric, 0.5f },
                { ElementalType.Ice, 2f },
                { ElementalType.Rock, 2f },
                { ElementalType.Steel, 0.5f },
                { ElementalType.Fairy, 2f },
            } },
            { ElementalType.Fairy, new Dictionary<ElementalType, float>() {
                { ElementalType.Fire, 0.5f },
                { ElementalType.Fighting, 2f },
                { ElementalType.Poison, 0.5f },
                { ElementalType.Dragon, 2f },
                { ElementalType.Dark, 2f },
                { ElementalType.Steel, 0.5f },
            } },
        };
    }
}
