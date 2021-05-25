using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRpg
{
    public static class PokemonBattleSimulator
    {
        private static bool LOG_BATTLE_EVENTS = false;

        public static MultiBattleResult SimulateBattles(Pokemon pokemonA, Pokemon pokemonB, int numBattles)
        {
            Random rand = new Random();
            float timesAWon = 0;
            MultiBattleResult returnVal = new MultiBattleResult();
            returnVal.PokemonAName = pokemonA.Name;
            returnVal.PokemonALevel = pokemonA.Level;
            returnVal.PokemonBName = pokemonB.Name;
            returnVal.PokemonBLevel = pokemonB.Level;
            for (int battle = 0; battle < numBattles; battle++)
            {
                SingleBattleResult battleResult = SimulateSingleBattle(pokemonA, pokemonB, rand);
                if (battleResult.PokemonAWon)
                {
                    timesAWon++;
                }

                returnVal.PokemonADamage.AddSet(battleResult.PokemonADamage);
                returnVal.PokemonBDamage.AddSet(battleResult.PokemonBDamage);
                returnVal.BattleLength.AddNumber(battleResult.TotalRounds);
            }

            returnVal.PokemonAWinPercentage = 100 * timesAWon / (float)numBattles;
            return returnVal;
        }

        public static void PrintMultiBattleResult(MultiBattleResult result)
        {
            Console.WriteLine();
            Console.WriteLine("Between {0} lvl {1} and {2} lvl {3}:", result.PokemonAName, result.PokemonALevel, result.PokemonBName, result.PokemonBLevel);
            Console.WriteLine("        {0} won {1:F2}% of the time; battle length: {2}", result.PokemonAName, result.PokemonAWinPercentage, result.BattleLength.ToString());
            Console.WriteLine("        {0} attack damage: {1}", result.PokemonAName, result.PokemonADamage.ToString());
            Console.WriteLine("        {0} attack damage: {1}", result.PokemonBName, result.PokemonBDamage.ToString());
        }

        public static void SimulateBattlesAcrossAllLevels(Func<int, Pokemon> pokemonA, Func<int, Pokemon> pokemonB, FileInfo outTsvFile)
        {
            using (FileStream fileWriter = new FileStream(outTsvFile.FullName, FileMode.Create, FileAccess.Write))
            using (StreamWriter tsvWriter = new StreamWriter(fileWriter))
            {
                // Write file header
                tsvWriter.Write(" ");
                Pokemon pkmnX = pokemonA(1);
                Pokemon pkmnY = pokemonB(1);
                for (int x = 1; x <= 100; x++)
                {
                    tsvWriter.Write("\tLv" + x + " " + pkmnX.Name);
                }
                tsvWriter.WriteLine();

                // Now do all the battles
                for (int y = 1; y <= 100; y++)
                {
                    tsvWriter.Write("Lv" + y + " " + pkmnY.Name);
                    for (int x = 1; x <= 100; x++)
                    {
                        MultiBattleResult result = SimulateBattles(pokemonA(x), pokemonB(y), 100);
                        tsvWriter.Write("\t");
                        tsvWriter.Write(result.PokemonAWinPercentage);
                    }

                    tsvWriter.WriteLine();
                }
            }
        }

        public static SingleBattleResult SimulateSingleBattle(Pokemon pokemonA, Pokemon pokemonB, Random rand)
        {
            int hpA = pokemonA.MaxHp;
            int hpB = pokemonB.MaxHp;
            float attacksByA = 0;
            float attacksByB = 0;
            SingleBattleResult returnVal = new SingleBattleResult();

            returnVal.PokemonAName = pokemonA.Name;
            returnVal.PokemonBName = pokemonB.Name;
            returnVal.PokemonAMaxHp = hpA;
            returnVal.PokemonBMaxHp = hpB;

            // Initiative
            bool aGoesFirst = pokemonA.Stat_Speed_Leveled > pokemonB.Stat_Speed_Leveled;
            if (pokemonA.Stat_Speed_Leveled == pokemonB.Stat_Speed_Leveled)
            {
                aGoesFirst = rand.NextDouble() < 0.5;
            }

            for (int round = 0; round < 100; round++)
            {
                PokemonMove move;
                int damage;
                if (round > 0 || aGoesFirst)
                {
                    // A attacks
                    attacksByA += 1;
                    move = pokemonA.Moves[rand.Next(0, pokemonA.Moves.Count)];
                    damage = CalculateDamage_Tabletop(pokemonA, move, pokemonB, rand);
                    returnVal.PokemonADamage.AddNumber(damage);
                    hpB -= damage;
                    if (LOG_BATTLE_EVENTS) Console.WriteLine("        {0} HP is now {1}", pokemonB.Name, hpB);
                    if (hpB <= 0)
                    {
                        returnVal.PokemonAWon = true;
                        returnVal.TotalRounds = (attacksByA + attacksByB) / 2.0f;
                        return returnVal;
                    }
                }

                // B attacks
                attacksByB += 1;
                move = pokemonB.Moves[rand.Next(0, pokemonB.Moves.Count)];
                damage = CalculateDamage_Tabletop(pokemonB, move, pokemonA, rand);
                returnVal.PokemonBDamage.AddNumber(damage);
                hpA -= damage;
                if (LOG_BATTLE_EVENTS) Console.WriteLine("        {0} HP is now {1}", pokemonA.Name, hpA);
                if (hpA <= 0)
                {
                    returnVal.PokemonAWon = false;
                    returnVal.TotalRounds = (attacksByA + attacksByB) / 2.0f;
                    return returnVal;
                }
            }

            returnVal.PokemonAWon = hpA > hpB;
            returnVal.TotalRounds = (attacksByA + attacksByB) / 2.0f;
            return returnVal;
        }

        public static int CalculateDamage_Tabletop(Pokemon attacker, PokemonMove move, Pokemon defender, Random rand)
        {
            if (move.Category == PokemonMoveCategory.Status)
            {
                return 0;
            }

            if (LOG_BATTLE_EVENTS) Console.WriteLine("        {0} attacks {1} using {2}", attacker.Name, defender.Name, move.Name);

            // Calculate attack dice
            int attackStat = move.BasePower.GetValueOrDefault(0);
            if (move.Category == PokemonMoveCategory.Physical)
            {
                attackStat += attacker.Stat_Attack_Leveled;
            }
            else if (move.Category == PokemonMoveCategory.Special)
            {
                attackStat += attacker.Stat_SpecialAttack_Leveled;
            }

            int attackDice = attackStat / 20;
            if (attacker.Type.HasFlag(move.Type))
            {
                // STAB
                attackDice += 1;
            }

            // defender weakness / resistance
            float attackMultiplier = TypeAdvantageCalculator.GetAttackTypeMultiplier(move.Type.Value, defender.Type);
            if (Math.Abs(attackMultiplier) < 0.01)
            {
                // 0 damage type resistance
                if (LOG_BATTLE_EVENTS) Console.WriteLine("          Attack multiplier of {0:F2} nullifies all damage", attackMultiplier);
                return 0;
            }
            else if (Math.Abs(attackMultiplier - 0.25) < 0.01)
            {
                // 1/4 damage type resistance
                attackDice = Math.Max(1, attackDice - 3);
                if (LOG_BATTLE_EVENTS) Console.WriteLine("          Attack multiplier of {0:F2} brings attack dice to {1}", attackMultiplier, attackDice);
            }
            else if (Math.Abs(attackMultiplier - 0.5) < 0.01)
            {
                // 1/2 damage type resistance
                attackDice = Math.Max(1, attackDice - 1);
                if (LOG_BATTLE_EVENTS) Console.WriteLine("          Attack multiplier of {0:F2} brings attack dice to {1}", attackMultiplier, attackDice);
            }
            else if (Math.Abs(attackMultiplier - 2.0) < 0.01)
            {
                // 2x damage type weakness
                attackDice += 1;
                if (LOG_BATTLE_EVENTS) Console.WriteLine("          Attack multiplier of {0:F2} brings attack dice to {1}", attackMultiplier, attackDice);
            }
            else if (Math.Abs(attackMultiplier - 4.0) < 0.01)
            {
                // 4x damage type weakness
                attackDice += 3;
                if (LOG_BATTLE_EVENTS) Console.WriteLine("          Attack multiplier of {0:F2} brings attack dice to {1}", attackMultiplier, attackDice);
            }

            // Calculate defense dice
            int defenseStat = 0;
            if (move.Category == PokemonMoveCategory.Physical)
            {
                defenseStat = defender.Stat_Defense_Leveled;
            }
            else if (move.Category == PokemonMoveCategory.Special)
            {
                defenseStat = defender.Stat_SpecialDefense_Leveled;
            }

            int defenseDice = Math.Max(0, (defenseStat / 20) - 1);

            if (LOG_BATTLE_EVENTS) Console.WriteLine("          Attack dice: {0} Defense dice: {1}", attackDice, defenseDice);

            int actualAttackDice = Math.Max(1, attackDice - defenseDice);

            int returnVal = Dice(actualAttackDice, 6, rand);
            if (LOG_BATTLE_EVENTS) Console.WriteLine("          Attacking with {0} dice for {1} damage", actualAttackDice, returnVal);

            //if (attackMultiplier != 1)
            //{
            //    returnVal = (int)Math.Floor(returnVal * attackMultiplier);
            //    if (LOG_BATTLE_EVENTS) Console.WriteLine("          Attack multiplier of {0:F2} brings it to {1} damage", attackMultiplier, returnVal);
            //}

            if (LOG_BATTLE_EVENTS) Console.WriteLine("        Total damage was {0}", returnVal);

            return returnVal;
        }

        public static int Dice(int diceCount, int diceSides, Random rand)
        {
            int returnVal = 0;
            for (int c = 0; c < diceCount; c++)
            {
                returnVal += rand.Next(0, diceSides) + 1;
            }

            return returnVal;
        }

        public static int CalculateDamage_GameAccurate(Pokemon attacker, PokemonMove move, Pokemon defender, Random rand)
        {
            if (move.Category == PokemonMoveCategory.Status)
            {
                return 0;
            }

            float returnVal = (((float)attacker.Level * 0.4f) + 2) * (float)move.BasePower;
            if (move.Category == PokemonMoveCategory.Physical)
            {
                returnVal *= (float)attacker.Stat_Attack_Leveled / (float)defender.Stat_Defense_Leveled;
            }
            else if (move.Category == PokemonMoveCategory.Special)
            {
                returnVal *= (float)attacker.Stat_SpecialAttack_Leveled / (float)defender.Stat_SpecialDefense_Leveled;
            }

            returnVal = (returnVal / 50.0f) + 2;

            if (attacker.Type.HasFlag(move.Type))
            {
                // stab
                returnVal *= 1.5f;
            }

            // defender weakness / resistance
            returnVal *= TypeAdvantageCalculator.GetAttackTypeMultiplier(move.Type.Value, defender.Type);

            // random variance
            returnVal *= (((float)rand.NextDouble() * 0.15f) + 0.85f);

            return (int)Math.Round(returnVal);
        }
    }
}
