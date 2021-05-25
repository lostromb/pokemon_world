using Durandal.Common.Logger;
using Durandal.Common.MathExt;
using Durandal.Common.Net.Http;
using Durandal.Common.Tasks;
using Durandal.Common.Time;
using Durandal.Common.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace PokemonRpg
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Loading data...");
            PokemonGenerator generator = new PokemonGenerator(
                new FileInfo(".\\data\\Pokemon.json"),
                new FileInfo(".\\data\\Moves.tsv"));

            while (true)
            {
                Console.WriteLine("Which Pokemon do you want to make? (e.g. Squirtle, Vulpix, Onix)");
                string name = Console.ReadLine();
                Pokemon poke = generator.CreateGuidedPokemon(name);
                if (poke != null)
                {
                    poke.PrintCharacterSheet();
                    string charSheetHtml = new CharacterSheet()
                        {
                            Poke = poke
                        }.Render();
                    string fileName = poke.Name + " level " + poke.Level + ".html";
                    File.WriteAllText(fileName, charSheetHtml, StringUtils.UTF8_WITHOUT_BOM);
                    Console.WriteLine("Saved character sheet to " + fileName);
                }
            }

            //PrintTotalMoveList(dictionaryOfPokemon["Snorlax"], dictionaryOfMoves);

            //DownloadPokemonData().Await();
        }

        public static void RunStatistics()
        {
            //MovingPercentile percentile = new MovingPercentile(10000, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9);
            //FastRandom rand = new FastRandom();

            //for (int c = 0; c < 100000; c++)
            //{
            //    int tries = 0;
            //    while (true)
            //    {
            //        tries++;
            //        if ((rand.NextInt(0, 20) + 1) >= 17)
            //        {
            //            break;
            //        }
            //    }

            //    percentile.Add(tries);
            //}

            //Console.WriteLine("Resolving status effects took this many attempts: " + percentile.ToString());
        }

        public static async Task DownloadPokemonData()
        {
            ILogger logger = new ConsoleLogger();
            IHttpClientFactory httpClientFactory = new PortableHttpClientFactory();
            IHttpClient httpClient = httpClientFactory.CreateHttpClient(new Uri("https://pokemondb.net"), logger.Clone("HttpClient"));

            // Get the master list of pokemon
            string masterPokemonList;
            HttpRequest request = HttpRequest.BuildFromUrlString("/pokedex/all");
            using (HttpResponse response = await httpClient.SendRequestAsync(request, CancellationToken.None, DefaultRealTimeProvider.Singleton, logger).ConfigureAwait(false))
            {
                if (response == null || response.ResponseCode != 200)
                {
                    logger.Log("Null or bad response when fetching pokemon list", LogLevel.Err);
                    return;
                }

                masterPokemonList = response.GetPayloadAsString();
            }

            RateLimiter limiter = new RateLimiter(2, 10, false);
            Regex pokemonMatcher = new Regex("href=\\\"(\\/pokedex\\/.+?)\\\"");
            List<PokemonTemplate> allPokemon = new List<PokemonTemplate>();
            foreach (Match urlMatch in pokemonMatcher.Matches(masterPokemonList))
            {
                Uri pokemonUri = new Uri("https://pokemondb.net" + urlMatch.Groups[1].Value);
                logger.Log("Crawling " + pokemonUri.AbsoluteUri);
                PokemonTemplate parsedPokemon = await PokedexReader.ParsePokemonPage(pokemonUri, httpClient, logger);
                if (parsedPokemon != null && parsedPokemon.Type != 0)
                {
                    allPokemon.Add(parsedPokemon);
                    limiter.Limit();
                }
            }

            string allJson = JsonConvert.SerializeObject(allPokemon);
            File.WriteAllText(@"C:\Code\PokemonRpg\Pokemon.json", allJson, Encoding.UTF8);
        }

        public static async Task DownloadMoveData()
        {
            ILogger logger = new ConsoleLogger();
            IHttpClientFactory httpClientFactory = new PortableHttpClientFactory();
            IHttpClient httpClient = httpClientFactory.CreateHttpClient(new Uri("https://pokemondb.net"), logger.Clone("HttpClient"));

            // Get the master list of moves
            string masterMoveList;
            HttpRequest request = HttpRequest.BuildFromUrlString("/move/all");
            using (HttpResponse response = await httpClient.SendRequestAsync(request, CancellationToken.None, DefaultRealTimeProvider.Singleton, logger).ConfigureAwait(false))
            {
                if (response == null || response.ResponseCode != 200)
                {
                    logger.Log("Null or bad response when fetching move list", LogLevel.Err);
                    return;
                }

                masterMoveList = response.GetPayloadAsString();
            }

            using (FileStream fileOut = new FileStream("moves.tsv", FileMode.Create, FileAccess.Write))
            using (StreamWriter tsvWriter = new StreamWriter(fileOut, StringUtils.UTF8_WITHOUT_BOM))
            {
                RateLimiter limiter = new RateLimiter(2, 10, false);
                Regex moveMatcher = new Regex("href=\\\"(\\/move\\/.+?)\\\"");
                Regex zMoveMatcher = new Regex("Z-Move", RegexOptions.IgnoreCase);
                Regex gmaxMoveMatcher = new Regex("G-Max", RegexOptions.IgnoreCase);
                foreach (Match urlMatch in moveMatcher.Matches(masterMoveList))
                {
                    Uri moveUri = new Uri("https://pokemondb.net" + urlMatch.Groups[1].Value);
                    logger.Log("Crawling " + moveUri.AbsoluteUri);
                    PokemonMove parsedMove = await PokedexReader.ParseMovePage(moveUri, httpClient, logger);
                    if (parsedMove != null &&
                        !string.IsNullOrEmpty(parsedMove.Description) &&
                        !zMoveMatcher.Match(parsedMove.Description).Success &&
                        !string.IsNullOrEmpty(parsedMove.Name) &&
                        !gmaxMoveMatcher.Match(parsedMove.Name).Success)
                    {
                        parsedMove.ToTSV(tsvWriter);
                        limiter.Limit();
                    }
                }
            }
        }

        public static void SimulateBattles()
        {
            //const int NUM_BATTLES = 10000;
            //Console.WriteLine();
            //Console.WriteLine("SHOULD BE ABOUT EVEN");
            //PokemonBattleSimulator.PrintMultiBattleResult(PokemonBattleSimulator.SimulateBattles(PokemonList.Audino(20), PokemonList.Squirtle(20), NUM_BATTLES));
            //Console.WriteLine();
            //Console.WriteLine("SHOULD BE ABOUT EVEN");
            //PokemonBattleSimulator.PrintMultiBattleResult(PokemonBattleSimulator.SimulateBattles(PokemonList.Audino(20), PokemonList.Drilbur(20), NUM_BATTLES));
            //Console.WriteLine();
            //Console.WriteLine("SQUIRTLE SHOULD WIN AFTER 3+ ROUNDS");
            //PokemonBattleSimulator.PrintMultiBattleResult(PokemonBattleSimulator.SimulateBattles(PokemonList.Squirtle(20), PokemonList.Drilbur(20), NUM_BATTLES));
            //Console.WriteLine();
            //Console.WriteLine("SQUIRTLE SHOULD WIN AFTER 3+ ROUNDS");
            //PokemonBattleSimulator.PrintMultiBattleResult(PokemonBattleSimulator.SimulateBattles(PokemonList.Squirtle(20), PokemonList.Charmander(20), NUM_BATTLES));
            //Console.WriteLine();
            //Console.WriteLine("SHOULD BE ABOUT EVEN");
            //PokemonBattleSimulator.PrintMultiBattleResult(PokemonBattleSimulator.SimulateBattles(PokemonList.Squirtle(20), PokemonList.Charmander(30), NUM_BATTLES));
            //Console.WriteLine();
            //Console.WriteLine("CHARMANDER SHOULD WIN");
            //PokemonBattleSimulator.PrintMultiBattleResult(PokemonBattleSimulator.SimulateBattles(PokemonList.Charmander(40), PokemonList.Squirtle(20), NUM_BATTLES));
            //Console.WriteLine();
            //Console.WriteLine("SHOULD BE ABOUT EVEN");
            //PokemonBattleSimulator.PrintMultiBattleResult(PokemonBattleSimulator.SimulateBattles(PokemonList.Audino(100), PokemonList.Charmander(100), NUM_BATTLES));

            //PokemonBattleSimulator.SimulateBattlesAcrossAllLevels(PokemonList.Charmander, PokemonList.Squirtle, new FileInfo(@"C:\Code\PokemonRpg\output.tsv"));
        }
    }
}
