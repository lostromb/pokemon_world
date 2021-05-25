using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRpg
{
    public class PokemonMove
    {
        public string Name { get; set; }
        public ElementalType? Type { get; set; }
        public PokemonMoveCategory? Category { get; set; }
        public int? BasePower { get; set; }
        public int? AccuracyP20 { get; set; }
        public int Priority { get; set; }
        public string Description { get; set; }
        public int? MaxPP { get; set; }
        public bool MakesContact { get; set; }
        public int? LearnedAtLevel { get; set; }
        public string Targets { get; set; }
        public int? NumberOfNaturalUsers { get; set; }
        public string FlavorText { get; set; }

        public int? AttackDice(Pokemon user)
        {
            if (!BasePower.HasValue)
            {
                return null;
            }

            int totalPower = BasePower.Value;
            if (Category.GetValueOrDefault(PokemonMoveCategory.Physical) == PokemonMoveCategory.Physical)
            {
                totalPower += user.Stat_Attack_Leveled;
            }
            else if (Category.GetValueOrDefault(PokemonMoveCategory.Physical) == PokemonMoveCategory.Special)
            {
                totalPower += user.Stat_SpecialAttack_Leveled;
            }
            else if (Category.GetValueOrDefault(PokemonMoveCategory.Physical) == PokemonMoveCategory.Agility)
            {
                totalPower += user.Stat_Speed_Leveled;
            }

            return totalPower / 20;
        }

        public void ToTSV(TextWriter writer)
        {
            writer.Write(Name);
            writer.Write("\t");
            writer.Write(Enum.GetName(typeof(ElementalType), Type));
            writer.Write("\t");
            writer.Write(Enum.GetName(typeof(PokemonMoveCategory), Category));
            writer.Write("\t");
            if (BasePower.HasValue) writer.Write(BasePower);
            writer.Write("\t");
            if (AccuracyP20.HasValue) writer.Write(AccuracyP20);
            writer.Write("\t");
            writer.Write(Priority);
            writer.Write("\t");
            writer.Write(Description);
            writer.Write("\t");
            if (MaxPP.HasValue) writer.Write(MaxPP);
            writer.Write("\t");
            writer.Write(MakesContact);
            writer.Write("\t");
            if (LearnedAtLevel.HasValue) writer.Write(LearnedAtLevel);
            writer.Write("\t");
            writer.Write(Targets);
            writer.Write("\t");
            writer.Write(NumberOfNaturalUsers);
            writer.Write("\t");
            writer.Write(FlavorText);
            writer.Write("\r\n");
        }

        public static PokemonMove FromTSV(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            string[] parts = line.Split('\t');
            if (parts.Length < 13)
            {
                return null;
            }

            PokemonMove returnVal = new PokemonMove();
            returnVal.Name = parts[0];
            ElementalType t;
            if (Enum.TryParse(parts[1], out t))
            {
                returnVal.Type = t;
            }

            PokemonMoveCategory c;
            if (Enum.TryParse(parts[2], out c))
            {
                returnVal.Category = c;
            }

            int i;
            if (int.TryParse(parts[3], out i))
            {
                returnVal.BasePower = i;
            }

            if (int.TryParse(parts[4], out i))
            {
                returnVal.AccuracyP20 = i;
            }

            if (int.TryParse(parts[5], out i))
            {
                returnVal.Priority = i;
            }

            returnVal.Description = parts[6].Trim('\"');
            // column 7 is max PP
            returnVal.MakesContact = 
                string.Equals("Yes", parts[8], StringComparison.OrdinalIgnoreCase) ||
                string.Equals("True", parts[8], StringComparison.OrdinalIgnoreCase);

            if (int.TryParse(parts[9], out i))
            {
                returnVal.LearnedAtLevel = i;
            }

            returnVal.Targets = parts[10];
            // column 11 is number of natural users
            returnVal.FlavorText = parts[12];

            return returnVal;
        }
    }
}
