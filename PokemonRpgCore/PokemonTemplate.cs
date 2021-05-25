using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRpg
{
    public class PokemonTemplate
    {
        public PokemonTemplate()
        {
            Abilities = new HashSet<string>();
            NaturallyLearnedMoves = new Dictionary<string, int>();
            OtherMoves = new HashSet<string>();
        }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Type")]
        [JsonConverter(typeof(ElementalTypeConverter))]
        public ElementalType Type { get; set; }

        [JsonProperty("Health")]
        public int Health { get; set; }

        [JsonProperty("Attack")]
        public int Attack { get; set; }

        [JsonProperty("Defense")]
        public int Defense { get; set; }

        [JsonProperty("SpecialAttack")]
        public int SpecialAttack { get; set; }

        [JsonProperty("SpecialDefense")]
        public int SpecialDefense { get; set; }

        [JsonProperty("Speed")]
        public int Speed { get; set; }

        [JsonProperty("Abilities")]
        public HashSet<string> Abilities { get; set; }

        [JsonProperty("NaturallyLearnedMoves")]
        public Dictionary<string, int> NaturallyLearnedMoves { get; set; }

        [JsonProperty("OtherMoves")]
        public HashSet<string> OtherMoves { get; set; }

        public Pokemon CreateInstance(int level)
        {
            Pokemon returnVal = new Pokemon();
            returnVal.Name = Name;
            returnVal.Level = level;
            returnVal.Stat_Health_Base = Health;
            returnVal.Stat_Attack_Base = Attack;
            returnVal.Stat_Defense_Base = Defense;
            returnVal.Stat_SpecialAttack_Base = SpecialAttack;
            returnVal.Stat_SpecialDefense_Base = SpecialDefense;
            returnVal.Stat_Speed_Base = Speed;
            returnVal.Type = Type;
            return returnVal;
        }
    }
}
