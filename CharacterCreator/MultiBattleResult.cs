using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRpg
{
    public class MultiBattleResult
    {
        public MultiBattleResult()
        {
            BattleLength = new DecimalSet();
            PokemonADamage = new DecimalSet();
            PokemonBDamage = new DecimalSet();
        }

        public string PokemonAName { get; set; }
        public string PokemonBName { get; set; }
        public int PokemonALevel { get; set; }
        public int PokemonBLevel { get; set; }
        public float PokemonAWinPercentage { get; set; }
        public DecimalSet BattleLength { get; set; }
        public DecimalSet PokemonADamage { get; set; }
        public DecimalSet PokemonBDamage { get; set; }
    }
}
