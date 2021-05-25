using Durandal.Common.MathExt;
using Durandal.Common.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRpg
{
    public class PokemonGenerator
    {
        private readonly Dictionary<string, PokemonTemplate> _dictionaryOfPokemon;
        private readonly Dictionary<string, PokemonMove> _dictionaryOfMoves;

        public PokemonGenerator(FileInfo pokemonJsonFile, FileInfo moveTsvFile)
        {
            string json = File.ReadAllText(pokemonJsonFile.FullName, StringUtils.UTF8_WITHOUT_BOM);
            List<PokemonTemplate> listOfPokemon = JsonConvert.DeserializeObject<List<PokemonTemplate>>(json);

            _dictionaryOfPokemon = new Dictionary<string, PokemonTemplate>();
            foreach (var poke in listOfPokemon)
            {
                _dictionaryOfPokemon[poke.Name] = poke;
            }

            // Read the list of moves
            string[] moveList = File.ReadAllLines(moveTsvFile.FullName, StringUtils.UTF8_WITHOUT_BOM);
            _dictionaryOfMoves = new Dictionary<string, PokemonMove>();
            bool first = true;
            foreach (string moveLine in moveList)
            {
                // skip header line
                if (first)
                {
                    first = false;
                    continue;
                }

                // Empty line
                if (moveLine.StartsWith("\t"))
                {
                    continue;
                }

                // Broken move
                if (moveLine.Contains("\tBroken\t"))
                {
                    Console.WriteLine("Skipping broken move " + moveLine.Substring(0, moveLine.IndexOf('\t')));
                    continue;
                }

                PokemonMove move = PokemonMove.FromTSV(moveLine);
                if (move != null && !_dictionaryOfMoves.ContainsKey(move.Name))
                {
                    _dictionaryOfMoves[move.Name] = move;
                }
            }

        }

        public Pokemon CreateRandomPokemon(
            string pokemonName,
            int level)
        {
            IRandom random = new FastRandom();
            PokemonTemplate template;

            if (!_dictionaryOfPokemon.TryGetValue(pokemonName, out template))
            {
                Console.WriteLine("Cannot find " + pokemonName);
                return null;
            }
            
            Pokemon returnVal = template.CreateInstance(level);

            // Pick an ability
            string[] abilities = template.Abilities.ToArray();
            returnVal.Ability = abilities[random.NextInt(0, abilities.Length)];

            // Simulate learning moves
            for (int c = 0; c < 3; c++)
            {
                LearnNewRandomMove(returnVal, template, 1, _dictionaryOfMoves, random);
            }

            for (int moveLevel = 5; moveLevel <= level; moveLevel += 5)
            {
                // is this the best way to do it?
                LearnNewRandomMove(returnVal, template, moveLevel, _dictionaryOfMoves, random);
            }

            return returnVal;
        }

        public Pokemon CreateGuidedPokemon(string pokemonName)
        {
            IRandom random = new FastRandom();
            PokemonTemplate template = _dictionaryOfPokemon[pokemonName];
            if (!_dictionaryOfPokemon.TryGetValue(pokemonName, out template))
            {
                Console.WriteLine("Cannot find " + pokemonName);
                return null;
            }

            Console.WriteLine("What level?");
            int level = PromptForNumber(1, 100);
            
            Console.WriteLine("Let's create a level " + level + " " + pokemonName);
            Pokemon returnVal = template.CreateInstance(level);

            string[] abilities = template.Abilities.ToArray();
            Console.WriteLine("Pick an ability");
            for (int c = 0; c < abilities.Length; c++)
            {
                Console.WriteLine("{0}: {1}", c + 1, abilities[c]);
            }

            int selectedAbility = PromptForNumber(1, abilities.Length);
            returnVal.Ability = abilities[selectedAbility - 1];
            Console.WriteLine("Selected " + returnVal.Ability);

            for (int c = 0; c < 3; c++)
            {
                LearnNewGuidedMove(returnVal, template, 1, _dictionaryOfMoves, random);
            }

            for (int moveLevel = 5; moveLevel <= level; moveLevel += 5)
            {
                LearnNewGuidedMove(returnVal, template, moveLevel, _dictionaryOfMoves, random);
            }

            return returnVal;
        }

        private static int PromptForNumber(int minValue, int maxValue)
        {
            string input;
            int userSelection;
            while (true)
            {
                input = Console.ReadLine();
                if (int.TryParse(input, out userSelection) && userSelection >= minValue && userSelection <= maxValue)
                {
                    return userSelection;
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }
            }
        }

        public static void PrintTotalMoveList(PokemonTemplate poke, Dictionary<string, PokemonMove> dictionaryOfMoves)
        {
            PokemonMove move;

            HashSet<string> addedMoves = new HashSet<string>();
            List<Tuple<PokemonMove, int>> returnVal = new List<Tuple<PokemonMove, int>>();
            foreach (var naturalMove in poke.NaturallyLearnedMoves)
            {
                if (!addedMoves.Contains(naturalMove.Key) &&
                    dictionaryOfMoves.TryGetValue(naturalMove.Key, out move))
                {
                    returnVal.Add(new Tuple<PokemonMove, int>(move, naturalMove.Value));
                    addedMoves.Add(naturalMove.Key);
                }
            }

            foreach (string otherMove in poke.OtherMoves)
            {
                if (!addedMoves.Contains(otherMove) &&
                    dictionaryOfMoves.TryGetValue(otherMove, out move))
                {
                    returnVal.Add(new Tuple<PokemonMove, int>(move, move.LearnedAtLevel.GetValueOrDefault(1)));
                    addedMoves.Add(otherMove);
                }
            }

            returnVal.Sort((a, b) => a.Item2.CompareTo(b.Item2));
            foreach (var aMove in returnVal)
            {
                Console.WriteLine("At level {0,3}: \t{1,-20} \t{2,-10} \t{3}",
                    aMove.Item2,
                    aMove.Item1.Name,
                    aMove.Item1.Category.GetValueOrDefault(PokemonMoveCategory.Physical),
                    aMove.Item1.Type.GetValueOrDefault(ElementalType.Normal));
            }
        }

        private struct MoveCandidate
        {
            public string Name;
            public int LearnedAtLevel;

            public MoveCandidate(string name, int learnedAtLevel)
            {
                Name = name;
                LearnedAtLevel = learnedAtLevel;
            }
        }

        private static void LearnNewGuidedMove(
            Pokemon poke,
            PokemonTemplate template,
            int maxLevel,
            Dictionary<string, PokemonMove> dictionaryOfMoves,
            IRandom random)
        {
            HashSet<string> knownMoves = new HashSet<string>();
            foreach (var m in poke.Moves)
            {
                knownMoves.Add(m.Name);
            }

            PokemonMove move;
            HashSet<string> consideredMoves = new HashSet<string>(knownMoves);
            List<MoveCandidate> possibleMoves = new List<MoveCandidate>();
            foreach (var naturalMove in template.NaturallyLearnedMoves)
            {
                if (dictionaryOfMoves.TryGetValue(naturalMove.Key, out move))
                {
                    if (!consideredMoves.Contains(naturalMove.Key))
                    {
                        if (naturalMove.Value <= maxLevel)
                        {
                            possibleMoves.Add(new MoveCandidate(naturalMove.Key, naturalMove.Value));
                        }

                        consideredMoves.Add(naturalMove.Key);
                    }
                }
            }

            foreach (var otherMove in template.OtherMoves)
            {
                if (dictionaryOfMoves.TryGetValue(otherMove, out move))
                {
                    if (move.LearnedAtLevel.GetValueOrDefault(1) <= maxLevel &&
                        !consideredMoves.Contains(otherMove))
                    {
                        possibleMoves.Add(new MoveCandidate(otherMove, move.LearnedAtLevel.GetValueOrDefault(1)));
                    }
                }
            }

            possibleMoves.Sort((a, b) => a.LearnedAtLevel.CompareTo(b.LearnedAtLevel));

            Console.WriteLine("Pick a move to learn at level " + maxLevel);
            for (int moveId = 0; moveId < possibleMoves.Count; moveId++)
            {
                MoveCandidate candidate = possibleMoves[moveId];
                move = dictionaryOfMoves[candidate.Name];

                if (template.NaturallyLearnedMoves.ContainsKey(move.Name))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.WriteLine("{0,2}: Level {1,3} {2,-20} {3,-10} {4,-8} Pwr {5,3} Acc {6,2} {7}",
                    moveId + 1,
                    candidate.LearnedAtLevel,
                    move.Name,
                    move.Category.GetValueOrDefault(PokemonMoveCategory.Physical),
                    move.Type.GetValueOrDefault(ElementalType.Normal),
                    move.BasePower.HasValue ? move.BasePower.Value.ToString() : "-",
                    move.AccuracyP20.HasValue ? move.AccuracyP20.Value.ToString() : "-",
                    (move.Description.Length > 80 ? move.Description.Substring(0, 80) : move.Description).Trim('\"'));

                Console.ResetColor();
            }

            int userSelection = PromptForNumber(1, possibleMoves.Count);
            move = dictionaryOfMoves[possibleMoves[userSelection - 1].Name];
            Console.WriteLine("Selected " + move.Name);
            poke.Moves.Add(move);
        }

        private static void LearnNewRandomMove(
            Pokemon poke,
            PokemonTemplate template,
            int maxLevel,
            Dictionary<string, PokemonMove> dictionaryOfMoves,
            IRandom random)
        {
            HashSet<string> knownMoves = new HashSet<string>();
            foreach (var m in poke.Moves)
            {
                knownMoves.Add(m.Name);
            }

            // this will prioritize higher level moves
            int minRecommendedLevel = maxLevel - 8;
            while (minRecommendedLevel >= -10)
            {
                PokemonMove move;
                HashSet<string> consideredMoves = new HashSet<string>(knownMoves);
                List<string> possibleMoves = new List<string>();
                foreach (var naturalMove in template.NaturallyLearnedMoves)
                {
                    if (dictionaryOfMoves.TryGetValue(naturalMove.Key, out move))
                    {
                        // Does this move seem to be a species exclusive?
                        if (move.NumberOfNaturalUsers < 10 &&
                            naturalMove.Value <= maxLevel &&
                            !consideredMoves.Contains(naturalMove.Key))
                        {
                            // Just learn it automatically.
                            poke.Moves.Add(move);
                            return;
                        }

                        if (!consideredMoves.Contains(naturalMove.Key))
                        {
                            if (naturalMove.Value <= maxLevel &&
                                naturalMove.Value >= minRecommendedLevel)
                            {
                                possibleMoves.Add(naturalMove.Key);
                            }

                            consideredMoves.Add(naturalMove.Key);
                        }
                    }
                }

                foreach (var otherMove in template.OtherMoves)
                {
                    if (dictionaryOfMoves.TryGetValue(otherMove, out move))
                    {
                        if (move.LearnedAtLevel.GetValueOrDefault(1) <= maxLevel &&
                            move.LearnedAtLevel.GetValueOrDefault(1) >= minRecommendedLevel &&
                            !consideredMoves.Contains(otherMove))
                        {
                            possibleMoves.Add(otherMove);
                        }
                    }
                }

                if (possibleMoves.Count > 3)
                {
                    while (true)
                    {
                        string moveToLearn = possibleMoves[random.NextInt(0, possibleMoves.Count)];
                        if (dictionaryOfMoves.TryGetValue(moveToLearn, out move))
                        {
                            // Favor naturally learned moves and ones of the user's own type
                            float clearance = 0.3f;
                            if (template.NaturallyLearnedMoves.ContainsKey(moveToLearn))
                            {
                                clearance += 0.2f;
                            }
                            if (poke.Type.HasFlag(move.Type.GetValueOrDefault(ElementalType.Normal)))
                            {
                                clearance += 0.2f;
                            }

                            if (random.NextFloat() < clearance)
                            {
                                poke.Moves.Add(move);
                                return;
                            }
                        }
                    }
                }

                minRecommendedLevel -= 1;
            }
        }
    }
}
