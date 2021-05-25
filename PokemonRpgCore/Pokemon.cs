using Durandal.Common.MathExt;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRpg
{
    public class Pokemon
    {
        private const float LV1_TOTAL_STAT_BASE = 340;
        private const float TOTAL_STAT_INCREASE_PER_LEVEL = 10;
        private int _level;
        private bool _statsDirty;

        private int _statHealthLeveled;
        private int _statAttackLeveled;
        private int _statDefenseLeveled;
        private int _statSpecialAttackLeveled;
        private int _statSpecialDefenseLeveled;
        private int _statSpeedLeveled;

        private int _statMaxHpLeveled;

        public Pokemon()
        {
            _statsDirty = true;
            _level = 1;
            Moves = new List<PokemonMove>();
        }

        private void CalculateInternalStatsIfNeeded()
        {
            if (_statsDirty)
            {
                float statTotalBase = Stat_Health_Base + Stat_Attack_Base + Stat_Defense_Base + Stat_SpecialAttack_Base + Stat_SpecialDefense_Base + Stat_Speed_Base;
                float statTotalLeveled = LV1_TOTAL_STAT_BASE + (_level * TOTAL_STAT_INCREASE_PER_LEVEL);
                _statHealthLeveled = (int)Math.Round(statTotalLeveled * (float)Stat_Health_Base / (float)statTotalBase);
                _statAttackLeveled = (int)Math.Round(statTotalLeveled * (float)Stat_Attack_Base / (float)statTotalBase);
                _statDefenseLeveled = (int)Math.Round(statTotalLeveled * (float)Stat_Defense_Base / (float)statTotalBase);
                _statSpecialAttackLeveled = (int)Math.Round(statTotalLeveled * (float)Stat_SpecialAttack_Base / (float)statTotalBase);
                _statSpecialDefenseLeveled = (int)Math.Round(statTotalLeveled * (float)Stat_SpecialDefense_Base / (float)statTotalBase);
                _statSpeedLeveled = (int)Math.Round(statTotalLeveled * (float)Stat_Speed_Base / (float)statTotalBase);
                _statMaxHpLeveled = 20 + (_statHealthLeveled / 2);
                _statsDirty = false;
            }
        }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Type")]
        [JsonConverter(typeof(ElementalTypeConverter))]
        public ElementalType Type { get; set; }

        [JsonIgnore]
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                if (value < 1 || value > 100)
                {
                    throw new ArgumentOutOfRangeException(nameof(Level));
                }

                _level = value;
                _statsDirty = true;
            }
        }

        [JsonProperty("Health")]
        public int Stat_Health_Base { get; set; }

        [JsonProperty("Attack")]
        public int Stat_Attack_Base { get; set; }

        [JsonProperty("Defense")]
        public int Stat_Defense_Base { get; set; }

        [JsonProperty("SpecialAttack")]
        public int Stat_SpecialAttack_Base { get; set; }

        [JsonProperty("SpecialDefense")]
        public int Stat_SpecialDefense_Base { get; set; }

        [JsonProperty("Speed")]
        public int Stat_Speed_Base { get; set; }

        [JsonIgnore]
        public int MaxHp
        {
            get
            {
                CalculateInternalStatsIfNeeded();
                return _statMaxHpLeveled;
            }
        }

        [JsonIgnore]
        public int Stat_Health_Leveled
        {
            get
            {
                CalculateInternalStatsIfNeeded();
                return _statHealthLeveled;
            }
        }

        [JsonIgnore]
        public int Stat_Attack_Leveled
        {
            get
            {
                CalculateInternalStatsIfNeeded();
                return _statAttackLeveled;
            }
        }

        [JsonIgnore]
        public int Stat_Attack_Dice
        {
            get
            {
                CalculateInternalStatsIfNeeded();
                return _statAttackLeveled / 20;
            }
        }

        [JsonIgnore]
        public int Stat_Defense_Leveled
        {
            get
            {
                CalculateInternalStatsIfNeeded();
                return _statDefenseLeveled;
            }
        }

        [JsonIgnore]
        public int Stat_Defense_Dice
        {
            get
            {
                CalculateInternalStatsIfNeeded();
                return _statDefenseLeveled / 20;
            }
        }

        [JsonIgnore]
        public int Stat_SpecialAttack_Leveled
        {
            get
            {
                CalculateInternalStatsIfNeeded();
                return _statSpecialAttackLeveled;
            }
        }

        [JsonIgnore]
        public int Stat_SpecialAttack_Dice
        {
            get
            {
                CalculateInternalStatsIfNeeded();
                return _statSpecialAttackLeveled / 20;
            }
        }

        [JsonIgnore]
        public int Stat_SpecialDefense_Leveled
        {
            get
            {
                CalculateInternalStatsIfNeeded();
                return _statSpecialDefenseLeveled;
            }
        }

        [JsonIgnore]
        public int Stat_SpecialDefense_Dice
        {
            get
            {
                CalculateInternalStatsIfNeeded();
                return _statSpecialDefenseLeveled / 20;
            }
        }

        [JsonIgnore]
        public int Stat_Speed_Leveled
        {
            get
            {
                CalculateInternalStatsIfNeeded();
                return _statSpeedLeveled;
            }
        }

        [JsonIgnore]
        public int Stat_Speed_Dice
        {
            get
            {
                CalculateInternalStatsIfNeeded();
                return _statSpeedLeveled / 20;
            }
        }

        [JsonIgnore]
        public int Stat_Speed_MoveDistanceMeters
        {
            get
            {
                CalculateInternalStatsIfNeeded();
                return 5 + (_statSpeedLeveled / 10);
            }
        }

        [JsonIgnore]
        public List<PokemonMove> Moves { get; set; }

        [JsonProperty("Ability")]
        public string Ability { get; set; }

        public void PrintCharacterSheet()
        {
            Console.WriteLine("{0} lvl {1}", this.Name, this.Level);
            Console.WriteLine("Type: {0}", this.Type);
            Console.WriteLine("Ability: {0}", this.Ability);
            Console.WriteLine("Health  {0} \tMaxHP {1}", this.Stat_Health_Leveled, this.MaxHp);
            Console.WriteLine("Attack  {0} \tDice {1}", this.Stat_Attack_Leveled, this.Stat_Attack_Leveled / 20);
            Console.WriteLine("Defense {0} \tDice {1}", this.Stat_Defense_Leveled, this.Stat_Defense_Leveled / 20);
            Console.WriteLine("Sp. Atk {0} \tDice {1}", this.Stat_SpecialAttack_Leveled, this.Stat_SpecialAttack_Leveled / 20);
            Console.WriteLine("Sp. Def {0} \tDice {1}", this.Stat_SpecialDefense_Leveled, this.Stat_SpecialDefense_Leveled / 20);
            Console.WriteLine("Speed   {0} \tMove Range {1}m", this.Stat_Speed_Leveled, 5 + (this.Stat_Speed_Leveled / 10));

            // Simulate d8 rolls to kill them
            DecimalSet percentile = new DecimalSet();
            FastRandom rand = new FastRandom();
            for (int bench = 0; bench < 1000; bench++)
            {
                int hp = this.MaxHp;
                int count = 0;
                while (hp > 0)
                {
                    count++;
                    hp -= rand.NextInt(1, 9);
                }

                percentile.AddNumber(count);
            }

            Console.WriteLine("Average d8 rolls to faint: " + percentile.Median);
            
            List<Tuple<ElementalType, float>> elements = new List<Tuple<ElementalType, float>>();
            for (int elementalType = (int)ElementalType.Normal; elementalType <= (int)ElementalType.Fairy; elementalType <<= 1)
            {
                float multiplier = TypeAdvantageCalculator.GetAttackTypeMultiplier((ElementalType)elementalType, this.Type);
                if (Math.Abs(multiplier - 1) > 0.01f)
                {
                    elements.Add(new Tuple<ElementalType, float>((ElementalType)elementalType, multiplier));
                }
            }

            if (elements.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Elemental weakness / resistance:");
                elements.Sort((a, b) => a.Item2.CompareTo(b.Item2));
                foreach (var element in elements)
                {
                    if (Math.Abs(element.Item2) < 0.01f)
                    {
                        Console.WriteLine("    Immune to {0}", element.Item1);
                    }
                    else if (Math.Abs(element.Item2 - 0.25) < 0.01f)
                    {
                        Console.WriteLine("    Very resistant to {0}", element.Item1);
                    }
                    else if (Math.Abs(element.Item2 - 0.5) < 0.01f)
                    {
                        Console.WriteLine("    Resistant to {0}", element.Item1);
                    }
                    else if (Math.Abs(element.Item2 - 2.0) < 0.01f)
                    {
                        Console.WriteLine("    Weak to {0}", element.Item1);
                    }
                    else if (Math.Abs(element.Item2 - 4.0) < 0.01f)
                    {
                        Console.WriteLine("    Very weak to {0}", element.Item1);
                    }
                }
            }

            // Print out their move list
            Console.WriteLine();
            Console.WriteLine("Move list:");
            foreach (var move in this.Moves)
            {
                Console.WriteLine("Move: \t{0,-20} \t{1,-10} \t{2}",
                    move.Name,
                    move.Category.GetValueOrDefault(PokemonMoveCategory.Physical),
                    move.Type.GetValueOrDefault(ElementalType.Normal));
            }

            Console.WriteLine();
        }

        public IEnumerable<Tuple<ElementalType, float>> GetResistancesAndWeaknesses()
        {
            List<Tuple<ElementalType, float>> elements = new List<Tuple<ElementalType, float>>();
            for (int elementalType = (int)ElementalType.Normal; elementalType <= (int)ElementalType.Fairy; elementalType <<= 1)
            {
                float multiplier = TypeAdvantageCalculator.GetAttackTypeMultiplier((ElementalType)elementalType, this.Type);
                if (Math.Abs(multiplier - 1) > 0.01f)
                {
                    elements.Add(new Tuple<ElementalType, float>((ElementalType)elementalType, multiplier));
                }
            }

            if (elements.Count > 0)
            {
                elements.Sort((a, b) => a.Item2.CompareTo(b.Item2));
            }

            return elements;
        }
    }
}
