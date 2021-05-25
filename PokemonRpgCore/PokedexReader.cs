using Durandal.Common.Logger;
using Durandal.Common.Net.Http;
using Durandal.Common.Time;
using Durandal.Common.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace PokemonRpg
{
    public static class PokedexReader
    {
        public static async Task<PokemonTemplate> ParsePokemonPage(Uri pokemonPage, IHttpClient httpClient, ILogger logger)
        {
            HttpRequest request = HttpRequest.BuildFromUrlString(pokemonPage.AbsolutePath);
            using (HttpResponse response = await httpClient.SendRequestAsync(request, CancellationToken.None, DefaultRealTimeProvider.Singleton, logger).ConfigureAwait(false))
            {
                if (response == null || response.ResponseCode != 200)
                {
                    logger.Log("Null or bad response", LogLevel.Err);
                    return null;
                }

                string responsePage = response.GetPayloadAsString();
                return ParsePokemonPage(responsePage, logger);
            }
        }

        public static async Task<PokemonMove> ParseMovePage(Uri movePage, IHttpClient httpClient, ILogger logger)
        {
            HttpRequest request = HttpRequest.BuildFromUrlString(movePage.AbsolutePath);
            using (HttpResponse response = await httpClient.SendRequestAsync(request, CancellationToken.None, DefaultRealTimeProvider.Singleton, logger).ConfigureAwait(false))
            {
                if (response == null || response.ResponseCode != 200)
                {
                    logger.Log("Null or bad response", LogLevel.Err);
                    return null;
                }

                string responsePage = response.GetPayloadAsString();
                return ParseMovePage(responsePage, logger);
            }
        }

        private static readonly Regex Parser_HtmlMarkupMatcher = new Regex("<\\/?\\w.*?>");
        private static readonly Regex Parser_WhitespaceMatcher = new Regex("\\s+");

        private static readonly Regex Parser_MoveName = new Regex("<h1>(.+?)<\\/h1>");
        private static readonly Regex Parser_MoveNameSanitizer = new Regex("<span.+?<\\/span>");
        private static readonly Regex Parser_MoveElementalType = new Regex("<th>Type<\\/th>.+>(.+)<\\/a>");
        private static readonly Regex Parser_MoveCategory = new Regex("<th>Category<\\/th>.+?title=\\\"(.+?)\\\"");
        private static readonly Regex Parser_MovePower = new Regex("<th>Power<\\/th>.+?<td>(\\d+)<\\/td>");
        private static readonly Regex Parser_MoveAccuracy = new Regex("<th>Accuracy<\\/th>.+?<td>(\\d+)<\\/td>");
        private static readonly Regex Parser_MovePriority = new Regex("<th>Priority<\\/th>.+?<td>\\+?([-\\d]+)<\\/td>");
        private static readonly Regex Parser_MovePP = new Regex("<th>PP<\\/th>.+?<td>(\\d+)");
        private static readonly Regex Parser_MoveMakesContact = new Regex("<th>Makes contact\\?<\\/th>.+?<td>(.+?)<\\/td>");
        private static readonly Regex Parser_MoveTarget = new Regex("<div class=\\\"move-target\\\">[\\w\\W]+?<p class=\\\"mt-descr\\\">(.+?)<\\/p>");
        private static readonly Regex Parser_MoveLearnedAtLevel = new Regex("<small class=\\\"text-muted\\\">Level (\\d+)<\\/small>");
        private static readonly Regex Parser_MoveDescriptionBaseFragment = new Regex("<h2 id=\\\"move-effects\\\">Effects<\\/h2>\\s*([\\w\\W]+?)\\s*(<h3|<\\/div)");
        private static readonly Regex Parser_MoveFlavorTextBaseFragment = new Regex("<h2 id=\\\"move-descr([\\w\\W]+?)\\s*(<h2|<\\/main)");
        private static readonly Regex Parser_MoveFlavorText = new Regex("<td class=\\\"cell-med-text\\\">(.+?)<\\/td>");

        private static readonly Regex Parser_PokemonName = new Regex("<h1>(.+?)<\\/h1>");
        private static readonly Regex Parser_PokemonNameSanitizer = new Regex("<span.+?<\\/span>");
        private static readonly Regex Parser_PokemonTypeParser = new Regex("<th>Type<\\/th>[\\w\\W]{1,100}?>(.+?)<\\/a>(?:[\\w\\W]{1,100}?>(.+?)<\\/a>)?");
        private static readonly Regex Parser_PokemonAbilityParser = new Regex("a href=\\\"\\/ability\\/[\\w\\W]{1,100}?>(.+?)<\\/a>");
        private static readonly Regex Parser_PokemonHPParser = new Regex("<th>HP<\\/th>[\\w\\W]*?<td class=\\\"cell-num\\\">(\\d+)<\\/td>");
        private static readonly Regex Parser_PokemonAttackParser = new Regex("<th>Attack<\\/th>[\\w\\W]*?<td class=\\\"cell-num\\\">(\\d+)<\\/td>");
        private static readonly Regex Parser_PokemonDefenseParser = new Regex("<th>Defense<\\/th>[\\w\\W]*?<td class=\\\"cell-num\\\">(\\d+)<\\/td>");
        private static readonly Regex Parser_PokemonSpecialAttackParser = new Regex("<th>Sp\\. Atk<\\/th>[\\w\\W]*?<td class=\\\"cell-num\\\">(\\d+)<\\/td>");
        private static readonly Regex Parser_PokemonSpecialDefenseParser = new Regex("<th>Sp\\. Def<\\/th>[\\w\\W]*?<td class=\\\"cell-num\\\">(\\d+)<\\/td>");
        private static readonly Regex Parser_PokemonSpeedParser = new Regex("<th>Speed<\\/th>[\\w\\W]*?<td class=\\\"cell-num\\\">(\\d+)<\\/td>");
        private static readonly Regex Parser_PokemonLevelUpRegionParser = new Regex("<h3>Moves learnt by level up<\\/h3>[\\w\\W]+?<\\/table");
        private static readonly Regex Parser_PokemonLeveledMoveParser = new Regex("<tr><td class=\\\"cell-num\\\">(\\d+)<\\/td>[\\w\\W]+?href=\\\"\\/move\\/.+?\\\"[\\w\\W]+?>(.+?)<\\/a>");
        private static readonly Regex Parser_PokemonMoveParser = new Regex("href=\\\"\\/move\\/.+?\\\"[\\w\\W]+?>(.+?)<\\/a>");

        public static PokemonTemplate ParsePokemonPage(string pokemonPageHtml, ILogger logger)
        {
            PokemonTemplate returnVal = new PokemonTemplate();
            string rippedString;

            rippedString = StringUtils.RegexRip(Parser_PokemonName, pokemonPageHtml, 1, logger);
            if (!string.IsNullOrEmpty(rippedString))
            {
                returnVal.Name = StringUtils.RegexRemove(Parser_PokemonNameSanitizer, rippedString).Trim();
            }
            else
            {
                logger.Log("Can't parse pokemon name", LogLevel.Wrn);
            }

            ElementalType elem;
            rippedString = StringUtils.RegexRip(Parser_PokemonTypeParser, pokemonPageHtml, 1, logger);
            if (!string.IsNullOrEmpty(rippedString) && Enum.TryParse(rippedString, out elem))
            {
                returnVal.Type = elem;
            }
            else
            {
                logger.Log("Can't parse pokemon type", LogLevel.Wrn);
            }

            rippedString = StringUtils.RegexRip(Parser_PokemonTypeParser, pokemonPageHtml, 2, logger);
            if (!string.IsNullOrEmpty(rippedString))
            {
                if (Enum.TryParse(rippedString, out elem))
                {
                    returnVal.Type |= elem;
                }
                else
                {
                    logger.Log("Can't parse pokemon secondary type", LogLevel.Wrn);
                }
            }

            foreach (Match abilityMatch in Parser_PokemonAbilityParser.Matches(pokemonPageHtml))
            {
                if (abilityMatch.Groups[1].Success && !returnVal.Abilities.Contains(abilityMatch.Groups[1].Value))
                {
                    returnVal.Abilities.Add(abilityMatch.Groups[1].Value);
                }
            }

            rippedString = StringUtils.RegexRip(Parser_PokemonHPParser, pokemonPageHtml, 1, logger);
            int stat;
            if (!string.IsNullOrEmpty(rippedString) && int.TryParse(rippedString, out stat))
            {
                returnVal.Health = stat;
            }
            else
            {
                logger.Log("Can't parse pokemon HP", LogLevel.Wrn);
            }

            rippedString = StringUtils.RegexRip(Parser_PokemonAttackParser, pokemonPageHtml, 1, logger);
            if (!string.IsNullOrEmpty(rippedString) && int.TryParse(rippedString, out stat))
            {
                returnVal.Attack = stat;
            }
            else
            {
                logger.Log("Can't parse pokemon attack", LogLevel.Wrn);
            }

            rippedString = StringUtils.RegexRip(Parser_PokemonDefenseParser, pokemonPageHtml, 1, logger);
            if (!string.IsNullOrEmpty(rippedString) && int.TryParse(rippedString, out stat))
            {
                returnVal.Defense = stat;
            }
            else
            {
                logger.Log("Can't parse pokemon defense", LogLevel.Wrn);
            }

            rippedString = StringUtils.RegexRip(Parser_PokemonSpecialAttackParser, pokemonPageHtml, 1, logger);
            if (!string.IsNullOrEmpty(rippedString) && int.TryParse(rippedString, out stat))
            {
                returnVal.SpecialAttack = stat;
            }
            else
            {
                logger.Log("Can't parse pokemon sp attack", LogLevel.Wrn);
            }

            rippedString = StringUtils.RegexRip(Parser_PokemonSpecialDefenseParser, pokemonPageHtml, 1, logger);
            if (!string.IsNullOrEmpty(rippedString) && int.TryParse(rippedString, out stat))
            {
                returnVal.SpecialDefense = stat;
            }
            else
            {
                logger.Log("Can't parse pokemon sp defense", LogLevel.Wrn);
            }

            rippedString = StringUtils.RegexRip(Parser_PokemonSpeedParser, pokemonPageHtml, 1, logger);
            if (!string.IsNullOrEmpty(rippedString) && int.TryParse(rippedString, out stat))
            {
                returnVal.Speed = stat;
            }
            else
            {
                logger.Log("Can't parse pokemon speed", LogLevel.Wrn);
            }

            string levelUpMoveRegion = StringUtils.RegexRip(Parser_PokemonLevelUpRegionParser, pokemonPageHtml, 0, logger);
            if (!string.IsNullOrEmpty(levelUpMoveRegion))
            {
                foreach (Match leveledMove in Parser_PokemonLeveledMoveParser.Matches(levelUpMoveRegion))
                {
                    int moveLevel = int.Parse(leveledMove.Groups[1].Value);
                    string moveName = leveledMove.Groups[2].Value;
                    returnVal.NaturallyLearnedMoves[moveName] = moveLevel;
                }
            }
            else
            {
                logger.Log("Can't parse pokemon level up moves", LogLevel.Wrn);
            }

            foreach (Match anyMove in Parser_PokemonMoveParser.Matches(pokemonPageHtml))
            {
                string moveName = anyMove.Groups[1].Value;
                if (!returnVal.NaturallyLearnedMoves.ContainsKey(moveName) &&
                    !returnVal.OtherMoves.Contains(moveName))
                {
                    returnVal.OtherMoves.Add(moveName);
                }
            }

            return returnVal;
        }

        public static PokemonMove ParseMovePage(string movePageHtml, ILogger logger)
        {
            PokemonMove returnVal = new PokemonMove();
            string rippedString;

            rippedString = StringUtils.RegexRip(Parser_MoveName, movePageHtml, 1, logger);
            if (!string.IsNullOrEmpty(rippedString))
            {
                returnVal.Name = StringUtils.RegexRemove(Parser_MoveNameSanitizer, rippedString).Trim();
            }
            else
            {
                logger.Log("Can't parse move name", LogLevel.Wrn);
            }

            rippedString = StringUtils.RegexRip(Parser_MoveElementalType, movePageHtml, 1, logger);
            ElementalType moveElementType;
            if (Enum.TryParse(rippedString, out moveElementType))
            {
                returnVal.Type = moveElementType;
            }
            else
            {
                logger.Log("Can't parse elemental type \"" + rippedString + "\"", LogLevel.Wrn);
                returnVal.Type = ElementalType.Normal;
            }

            rippedString = StringUtils.RegexRip(Parser_MoveCategory, movePageHtml, 1, logger);
            PokemonMoveCategory moveType;
            if (Enum.TryParse(rippedString, out moveType))
            {
                returnVal.Category = moveType;
            }
            else
            {
                logger.Log("Can't parse move type \"" + rippedString + "\"", LogLevel.Wrn);
                returnVal.Category = PokemonMoveCategory.Physical;
            }

            rippedString = StringUtils.RegexRip(Parser_MovePower, movePageHtml, 1, logger);
            if (!string.IsNullOrEmpty(rippedString))
            {
                int movePower;
                if (int.TryParse(rippedString, out movePower))
                {
                    returnVal.BasePower = movePower;
                }
                else
                {
                    logger.Log("Can't parse move power \"" + rippedString + "\"", LogLevel.Wrn);
                    returnVal.BasePower = null;
                }
            }

            rippedString = StringUtils.RegexRip(Parser_MoveAccuracy, movePageHtml, 1, logger);
            if (!string.IsNullOrEmpty(rippedString))
            {
                int moveAccuracyP100;
                if (int.TryParse(rippedString, out moveAccuracyP100))
                {
                    returnVal.AccuracyP20 = (moveAccuracyP100 / 5);
                }
                else
                {
                    logger.Log("Can't parse move accuracy \"" + rippedString + "\"", LogLevel.Wrn);
                    returnVal.AccuracyP20 = null;
                }
            }

            rippedString = StringUtils.RegexRip(Parser_MovePriority, movePageHtml, 1, logger);
            if (!string.IsNullOrEmpty(rippedString))
            {
                int movePriority;
                if (int.TryParse(rippedString, out movePriority))
                {
                    returnVal.Priority = movePriority;
                }
                else
                {
                    logger.Log("Can't parse move priority \"" + rippedString + "\"", LogLevel.Wrn);
                    returnVal.Priority = 0;
                }
            }
            else
            {
                returnVal.Priority = 0;
            }

            rippedString = StringUtils.RegexRip(Parser_MovePP, movePageHtml, 1, logger);
            if (!string.IsNullOrEmpty(rippedString))
            {
                int movePP;
                if (int.TryParse(rippedString, out movePP))
                {
                    returnVal.MaxPP = movePP;
                }
                else
                {
                    logger.Log("Can't parse move PP \"" + rippedString + "\"", LogLevel.Wrn);
                    returnVal.MaxPP = null;
                }
            }

            rippedString = StringUtils.RegexRip(Parser_MoveMakesContact, movePageHtml, 1, logger);
            if (string.Equals(rippedString, "Yes", StringComparison.OrdinalIgnoreCase))
            {
                returnVal.MakesContact = true;
            }
            else if (string.Equals(rippedString, "No", StringComparison.OrdinalIgnoreCase))
            {
                returnVal.MakesContact = false;
            }
            else
            {
                logger.Log("Can't parse move makes contact = \"" + rippedString + "\"", LogLevel.Wrn);
                returnVal.MakesContact = true;
            }

            DecimalSet levelSet = new DecimalSet();
            foreach (Match match in Parser_MoveLearnedAtLevel.Matches(movePageHtml))
            {
                int learnedLevel;
                if (match.Success && int.TryParse(match.Groups[1].Value, out learnedLevel))
                {
                    levelSet.AddNumber((float)learnedLevel);
                }
            }

            if (levelSet.Count > 4)
            {
                returnVal.LearnedAtLevel = (int)levelSet.Median;
            }
            else if (levelSet.Count > 0)
            {
                // Assume this is some legendary move or exclusive move, in which case we assume it is a high level. See for example Sacred Fire
                returnVal.LearnedAtLevel = (int)levelSet.Max;
            }
            else
            {
                logger.Log("Cannot determine learned-at level for this move; assuming it is some kind of exclusive", LogLevel.Wrn);
                returnVal.LearnedAtLevel = null;
            }

            returnVal.NumberOfNaturalUsers = levelSet.Count;

            rippedString = StringUtils.RegexRip(Parser_MoveTarget, movePageHtml, 1, logger);
            if (!string.IsNullOrEmpty(rippedString))
            {
                returnVal.Targets = rippedString;
            }

            // Now parse the description. This is tough since it's a complex XML fragment.
            rippedString = StringUtils.RegexRip(Parser_MoveDescriptionBaseFragment, movePageHtml, 1, logger);
            if (!string.IsNullOrEmpty(rippedString))
            {
                rippedString = StringUtils.RegexRemove(Parser_HtmlMarkupMatcher, rippedString);
                rippedString = StringUtils.RegexReplace(Parser_WhitespaceMatcher, rippedString, " ");
                rippedString = UnescapeXml(rippedString, logger);
                returnVal.Description = rippedString;
            }
            else
            {
                logger.Log("Cannot parse description for this move", LogLevel.Wrn);
            }

            // Similarly for the flavor text
            returnVal.FlavorText = string.Empty;
            string flavorTextFragment = StringUtils.RegexRip(Parser_MoveFlavorTextBaseFragment, movePageHtml, 1, logger);
            if (!string.IsNullOrEmpty(flavorTextFragment))
            {
                foreach (Match flavorTextMatch in Parser_MoveFlavorText.Matches(flavorTextFragment))
                {
                    rippedString = flavorTextMatch.Groups[1].Value;
                    rippedString = StringUtils.RegexRemove(Parser_HtmlMarkupMatcher, rippedString);
                    rippedString = StringUtils.RegexReplace(Parser_WhitespaceMatcher, rippedString, " ");
                    rippedString = UnescapeXml(rippedString, logger).Trim();
                    if (!rippedString.Contains("recommended that this move is forgotten"))
                    {
                        returnVal.FlavorText = rippedString.Trim();
                    }
                }
            }
            else
            {
                logger.Log("Cannot parse flavor text for this move", LogLevel.Wrn);
            }

            return returnVal;
        }
        private static string UnescapeXml(string xmlString, ILogger logger)
        {
            string unescaped = "<xml>" + xmlString + "</xml>";

            try
            {
                // Unescape XML tokens such as &#8260;
                XmlReader reader = XmlReader.Create(new StringReader(unescaped));
                if (reader.Read())
                {
                    return reader.ReadElementContentAsString();
                }
                else
                {
                    logger.Log("Can't read xml node " + unescaped, LogLevel.Wrn);
                    return xmlString;
                }
            }
            catch (Exception e)
            {
                logger.Log("Can't read xml node " + unescaped, LogLevel.Wrn);
                logger.Log(e, LogLevel.Wrn);
                return xmlString;
            }
        }
    }
}
