using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRpg
{
    public class SingleBattleResult
    {
        public SingleBattleResult()
        {
            PokemonADamage = new DecimalSet();
            PokemonBDamage = new DecimalSet();
        }

        public string PokemonAName { get; set; }
        public string PokemonBName { get; set; }
        public int PokemonAMaxHp { get; set; }
        public int PokemonBMaxHp { get; set; }
        public float TotalRounds { get; set; }
        public bool PokemonAWon { get; set; }
        public DecimalSet PokemonADamage { get; set; }
        public DecimalSet PokemonBDamage { get; set; }
    }
}
