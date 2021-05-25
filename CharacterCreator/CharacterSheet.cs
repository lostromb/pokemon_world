//// DO NOT MODIFY!!! THIS FILE IS AUTOGENED AND WILL BE OVERWRITTEN!!! ////

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
namespace PokemonRpg
{
    public class CharacterSheet
    {
        private StringWriter Output;
        public Pokemon Poke {get; set;}
        public CharacterSheet()
        {
        }
        public string Render()
        {
            StringBuilder returnVal = new StringBuilder();
            Output = new StringWriter(returnVal);
            RenderViewLevel0();
            return returnVal.ToString();
        }
        private void RenderViewLevel0()
        {
    #line hidden
            Output.Write("<html>\r\n\t<head>\r\n\t\t<title>Pokémon Character Sheet</title>\r\n\t\t<meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\"/>\r\n\t\t<meta charset=\"UTF-8\">\r\n\t</head>\r\n\t<style>\r\n.flex\r\n{\r\n\tdisplay: -ms-flexbox;\r\n\tdisplay: -ms-flex;\r\n\tdisplay: -webkit-flex;\r\n\tdisplay: -webkit-box;\r\n\tdisplay: -moz-box;\r\n\tdisplay: flex;\r\n}\r\n.flex-stretch\r\n{\r\n\t-webkit-align-items: stretch;\r\n\t-moz-align-items: stretch;\r\n\t-ms-align-items: stretch;\r\n\talign-items: stretch;\r\n}\r\n.flex-center\r\n{\r\n\t-webkit-align-items: center;\r\n\t-moz-align-items: center;\r\n\t-ms-align-items: center;\r\n\talign-items: center;\r\n}\r\n.flexBoxRow\r\n{\r\n\twidth: 100%;\r\n\theight: 100%;\r\n\t-webkit-flex-direction: row;\r\n\tflex-direction: row;\r\n\t-webkit-flex-flow: row nowrap;\r\n\t-moz-flex-flow: row nowrap;\r\n\t-ms-flex-flow: row nowrap;\r\n\tflex-flow: row nowrap;\r\n}\r\n.flexBoxColumn\r\n{\r\n\twidth: 100%;\r\n\theight: 100%;\r\n\t-webkit-flex-direction: column;\r\n\tflex-direction: column;\r\n\t-webkit-flex-flow: column nowrap;\r\n\t-moz-flex-flow: column nowrap;\r\n\t-ms-flex-flow: column nowrap;\r\n\tflex-flow: column nowrap;\r\n}\r\n.noFlexShrink\r\n{\r\n\t-webkit-flex-shrink: 0;\r\n\t-moz-flex-shrink: 0;\r\n\t-ms-flex-shrink: 0;\r\n\tflex-shrink: 0;\r\n}\r\n.flex1\r\n{\r\n\t-webkit-box-flex: 1;\r\n\t-moz-box-flex: 1;\r\n\t-webkit-flex: 1;\r\n\t-ms-flex: 1 1 auto;\r\n\tflex: 1;\r\n}\r\n.flex2\r\n{\r\n\t-webkit-box-flex: 2;\r\n\t-moz-box-flex: 2;\r\n\t-webkit-flex: 2;\r\n\t-ms-flex: 2 2 auto;\r\n\tflex: 2;\r\n}\r\n.flex3\r\n{\r\n\t-webkit-box-flex: 3;\r\n\t-moz-box-flex: 3;\r\n\t-webkit-flex: 3;\r\n\t-ms-flex: 3 3 auto;\r\n\tflex: 3;\r\n}\r\n.flex5\r\n{\r\n\t-webkit-box-flex: 5;\r\n\t-moz-box-flex: 5;\r\n\t-webkit-flex: 5;\r\n\t-ms-flex: 5 5 auto;\r\n\tflex: 5;\r\n}\r\n.red { background-color: #F00; }\r\n.green { background-color: #0F0; }\r\n.blue { background-color: #00F; }\r\n\r\n.wide\r\n{\r\n\twidth: 100%;\r\n}\r\n\r\nbody\r\n{\r\n\tpadding: 20px;\r\n\tmargin: 0px;\r\n\tfont-family: arial, sans-serif;\r\n}\r\n\r\ntable\r\n{\r\n\tborder-collapse: collapse;\r\n\tpadding: 0px;\r\n\tmargin: 0px;\r\n}\r\n\r\ntd, th {\r\n\tborder: 1px solid #000;\r\n\ttext-align: left;\r\n\tpadding: 0px;\r\n}\r\n\r\n.positionAnchor\r\n{\r\n\tposition: relative;\r\n}\r\n.subtitle\r\n{\r\n\tfont-size: 10;\r\n\tposition: absolute;\r\n\tleft: 1;\r\n\ttop: 1;\r\n\tword-wrap: normal;\r\n}\r\n.infoCell\r\n{\r\n\tpadding: 8px;\r\n\toverflow: clip;\r\n}\r\n.noBorder\r\n{\r\n\tborder: 0px solid #000;\r\n}\r\n.noBorderRight\r\n{\r\n\tborder-right: 0px solid #000;\r\n}\r\n.noBorderBottom\r\n{\r\n\tborder-bottom: 0px solid #000;\r\n}\r\n.padTop\r\n{\r\n\tpadding-top: 12px;\r\n}\r\nh2\r\n{\r\n\tpadding: 6px;\r\n\tmargin: 0px;\r\n}\r\n.pokemonName\r\n{\r\n\tfont-size: 18pt;\r\n}\r\n\r\n\t</style>\r\n\t<body>\r\n\t\t<div class=\"flex\">\r\n\t\t\t<table class=\"wide\">\r\n\t\t\t<tr>\r\n\t\t\t<td class=\"noBorder\">\r\n\t\t\t\t<h2>Pokémon Information</h2>\r\n\t\t\t\t<div class=\"flex flexBoxRow flex-stretch\">\r\n\t\t\t\t\t<div class=\"flex flex1\">\r\n\t\t\t\t\t\t<table class=\"wide\">\r\n\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t<td class=\"positionAnchor infoCell padTop noBorderRight\">\r\n\t\t\t\t\t\t\t<span class=\"subtitle\">Name</span>&nbsp;&nbsp;<span class=\"pokemonName\">");
    #line default
            try
            {
    #line 176 "CharacterSheet"
                                                                                                        Output.Write(Poke.Name);
    #line default
            }
            catch(System.NullReferenceException)
            {
                Output.Write("${Poke.Name}");
            }
    #line hidden
            Output.Write("</span>\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t<td class=\"positionAnchor infoCell padTop noBorderRight\">\r\n\t\t\t\t\t\t\t<span class=\"subtitle\">Level</span>&nbsp;&nbsp;<span>");
    #line default
            try
            {
    #line 181 "CharacterSheet"
                                                                                     Output.Write(Poke.Level);
    #line default
            }
            catch(System.NullReferenceException)
            {
                Output.Write("${Poke.Level}");
            }
    #line hidden
            Output.Write("</span>\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t<td class=\"positionAnchor infoCell padTop noBorderRight\">\r\n\t\t\t\t\t\t\t<span class=\"subtitle\">Type</span>&nbsp;&nbsp;<span>");
    #line default
            try
            {
    #line 186 "CharacterSheet"
                                                                                    Output.Write(Poke.Type);
    #line default
            }
            catch(System.NullReferenceException)
            {
                Output.Write("${Poke.Type}");
            }
    #line hidden
            Output.Write("</span>\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t<td class=\"positionAnchor infoCell padTop noBorderRight noBorderBottom\">\r\n\t\t\t\t\t\t\t<span class=\"subtitle\">Ability</span>&nbsp;&nbsp;<span>");
    #line default
            try
            {
    #line 191 "CharacterSheet"
                                                                                       Output.Write(Poke.Ability);
    #line default
            }
            catch(System.NullReferenceException)
            {
                Output.Write("${Poke.Ability}");
            }
    #line hidden
            Output.Write("</span>\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t<td class=\"positionAnchor infoCell padTop noBorderRight noBorderBottom\">\r\n\t\t\t\t\t\t\t<span class=\"subtitle\">Current HP</span>&nbsp;&nbsp;<span></span>\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</table>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t\t<div class=\"flex flex2\">\r\n\t\t\t\t\t\t<table class=\"wide\">\r\n\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t<th class=\"infoCell\">\r\n\t\t\t\t\t\t\tStat\r\n\t\t\t\t\t\t\t</th>\r\n\t\t\t\t\t\t\t<th class=\"infoCell\">\r\n\t\t\t\t\t\t\tValue\r\n\t\t\t\t\t\t\t</th>\r\n\t\t\t\t\t\t\t<th class=\"infoCell noBorderRight\">\r\n\t\t\t\t\t\t\tGame Value\r\n\t\t\t\t\t\t\t</th>\r\n\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t<td class=\"infoCell\">\r\n\t\t\t\t\t\t\t\tHealth\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"infoCell\">\r\n\t\t\t\t\t\t\t\t");
    #line default
            try
            {
    #line 219 "CharacterSheet"
                                    Output.Write(Poke.Stat_Health_Leveled);
    #line default
            }
            catch(System.NullReferenceException)
            {
                Output.Write("${Poke.Stat_Health_Leveled}");
            }
    #line hidden
            Output.Write("\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"infoCell noBorderRight\">\r\n\t\t\t\t\t\t\t\t");
    #line default
            try
            {
    #line 222 "CharacterSheet"
                                    Output.Write(Poke.MaxHp);
    #line default
            }
            catch(System.NullReferenceException)
            {
                Output.Write("${Poke.MaxHp}");
            }
    #line hidden
            Output.Write(" Max HP\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t<td class=\"infoCell\">\r\n\t\t\t\t\t\t\t\tAttack\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"infoCell\">\r\n\t\t\t\t\t\t\t\t");
    #line default
            try
            {
    #line 230 "CharacterSheet"
                                    Output.Write(Poke.Stat_Attack_Leveled);
    #line default
            }
            catch(System.NullReferenceException)
            {
                Output.Write("${Poke.Stat_Attack_Leveled}");
            }
    #line hidden
            Output.Write("\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"infoCell noBorderRight\">\r\n\t\t\t\t\t\t\t\t");
    #line default
            try
            {
    #line 233 "CharacterSheet"
                                    Output.Write(Poke.Stat_Attack_Dice);
    #line default
            }
            catch(System.NullReferenceException)
            {
                Output.Write("${Poke.Stat_Attack_Dice}");
            }
    #line hidden
            Output.Write(" dice\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t<td class=\"infoCell\">\r\n\t\t\t\t\t\t\t\tDefense\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"infoCell\">\r\n\t\t\t\t\t\t\t\t");
    #line default
            try
            {
    #line 241 "CharacterSheet"
                                    Output.Write(Poke.Stat_Defense_Leveled);
    #line default
            }
            catch(System.NullReferenceException)
            {
                Output.Write("${Poke.Stat_Defense_Leveled}");
            }
    #line hidden
            Output.Write("\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"infoCell noBorderRight\">\r\n\t\t\t\t\t\t\t\t");
    #line default
            try
            {
    #line 244 "CharacterSheet"
                                    Output.Write(Poke.Stat_Defense_Dice);
    #line default
            }
            catch(System.NullReferenceException)
            {
                Output.Write("${Poke.Stat_Defense_Dice}");
            }
    #line hidden
            Output.Write(" dice\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t<td class=\"infoCell\">\r\n\t\t\t\t\t\t\t\tSp. Atk\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"infoCell\">\r\n\t\t\t\t\t\t\t\t");
    #line default
            try
            {
    #line 252 "CharacterSheet"
                                    Output.Write(Poke.Stat_SpecialAttack_Leveled);
    #line default
            }
            catch(System.NullReferenceException)
            {
                Output.Write("${Poke.Stat_SpecialAttack_Leveled}");
            }
    #line hidden
            Output.Write("\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"infoCell noBorderRight\">\r\n\t\t\t\t\t\t\t\t");
    #line default
            try
            {
    #line 255 "CharacterSheet"
                                    Output.Write(Poke.Stat_SpecialAttack_Dice);
    #line default
            }
            catch(System.NullReferenceException)
            {
                Output.Write("${Poke.Stat_SpecialAttack_Dice}");
            }
    #line hidden
            Output.Write(" dice\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t<td class=\"infoCell\">\r\n\t\t\t\t\t\t\t\tSp. Def\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"infoCell\">\r\n\t\t\t\t\t\t\t\t");
    #line default
            try
            {
    #line 263 "CharacterSheet"
                                    Output.Write(Poke.Stat_SpecialDefense_Leveled);
    #line default
            }
            catch(System.NullReferenceException)
            {
                Output.Write("${Poke.Stat_SpecialDefense_Leveled}");
            }
    #line hidden
            Output.Write("\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"infoCell noBorderRight\">\r\n\t\t\t\t\t\t\t\t");
    #line default
            try
            {
    #line 266 "CharacterSheet"
                                    Output.Write(Poke.Stat_SpecialDefense_Dice);
    #line default
            }
            catch(System.NullReferenceException)
            {
                Output.Write("${Poke.Stat_SpecialDefense_Dice}");
            }
    #line hidden
            Output.Write(" dice\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t<td class=\"infoCell noBorderBottom\">\r\n\t\t\t\t\t\t\t\tSpeed\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"infoCell noBorderBottom\">\r\n\t\t\t\t\t\t\t\t");
    #line default
            try
            {
    #line 274 "CharacterSheet"
                                    Output.Write(Poke.Stat_Speed_Leveled);
    #line default
            }
            catch(System.NullReferenceException)
            {
                Output.Write("${Poke.Stat_Speed_Leveled}");
            }
    #line hidden
            Output.Write("\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"infoCell noBorderRight noBorderBottom\">\r\n\t\t\t\t\t\t\t\t");
    #line default
            try
            {
    #line 277 "CharacterSheet"
                                    Output.Write(Poke.Stat_Speed_Dice);
    #line default
            }
            catch(System.NullReferenceException)
            {
                Output.Write("${Poke.Stat_Speed_Dice}");
            }
    #line hidden
            Output.Write(" dice, ");
    #line default
            try
            {
    #line 277 "CharacterSheet"
                                                                  Output.Write(Poke.Stat_Speed_MoveDistanceMeters);
    #line default
            }
            catch(System.NullReferenceException)
            {
                Output.Write("${Poke.Stat_Speed_MoveDistanceMeters}");
            }
    #line hidden
            Output.Write("m move\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</table>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t\t<div class=\"flex flex1\">\r\n\t\t\t\t\t\t<table class=\"wide tall\">\r\n\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t<td class=\"infoCell noBorderBottom\">");
    #line default
            {
    #line 285 "CharacterSheet"
                                                                    foreach(var element in Poke.GetResistancesAndWeaknesses())
    #line default
                {
    #line hidden
    #line default
    #line 286 "CharacterSheet"
                                                                                                  if (Math.Abs(element.Item2) < 0.01f)
    #line default
                    {
    #line hidden
                        Output.Write("\r\n\t\t\t\t\t\t\t\t\t\t<span>Immune to ");
    #line default
                        try
                        {
    #line 288 "CharacterSheet"
                                                            Output.Write(element.Item1);
    #line default
                        }
                        catch(System.NullReferenceException)
                        {
                            Output.Write("${element.Item1}");
                        }
    #line hidden
                        Output.Write("</span><br/>");
    #line default
                    }
    #line 289 "CharacterSheet"
                                             if (Math.Abs(element.Item2 - 0.25f) < 0.01f)
    #line default
                    {
    #line hidden
                        Output.Write("\r\n\t\t\t\t\t\t\t\t\t\t<span>Very resistant to ");
    #line default
                        try
                        {
    #line 291 "CharacterSheet"
                                                                    Output.Write(element.Item1);
    #line default
                        }
                        catch(System.NullReferenceException)
                        {
                            Output.Write("${element.Item1}");
                        }
    #line hidden
                        Output.Write("</span><br/>");
    #line default
                    }
    #line 292 "CharacterSheet"
                                             if (Math.Abs(element.Item2 - 0.5f) < 0.01f)
    #line default
                    {
    #line hidden
                        Output.Write("\r\n\t\t\t\t\t\t\t\t\t\t<span>Resistant to ");
    #line default
                        try
                        {
    #line 294 "CharacterSheet"
                                                               Output.Write(element.Item1);
    #line default
                        }
                        catch(System.NullReferenceException)
                        {
                            Output.Write("${element.Item1}");
                        }
    #line hidden
                        Output.Write("</span><br/>");
    #line default
                    }
    #line 295 "CharacterSheet"
                                             if (Math.Abs(element.Item2 - 2.0f) < 0.01f)
    #line default
                    {
    #line hidden
                        Output.Write("\r\n\t\t\t\t\t\t\t\t\t\t<span>Weak to ");
    #line default
                        try
                        {
    #line 297 "CharacterSheet"
                                                          Output.Write(element.Item1);
    #line default
                        }
                        catch(System.NullReferenceException)
                        {
                            Output.Write("${element.Item1}");
                        }
    #line hidden
                        Output.Write("</span><br/>");
    #line default
                    }
    #line 298 "CharacterSheet"
                                             if (Math.Abs(element.Item2 - 4.0f) < 0.01f)
    #line default
                    {
    #line hidden
                        Output.Write("\r\n\t\t\t\t\t\t\t\t\t\t<span>Very weak to ");
    #line default
                        try
                        {
    #line 300 "CharacterSheet"
                                                               Output.Write(element.Item1);
    #line default
                        }
                        catch(System.NullReferenceException)
                        {
                            Output.Write("${element.Item1}");
                        }
    #line hidden
                        Output.Write("</span><br/>");
    #line default
                    }
    #line hidden
    #line default
                }
            }
    #line hidden
            Output.Write("\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t\t</table>\r\n\t\t\t\t\t</div>\r\n\t\t\t\t</div>\r\n\t\t\t</tr>\r\n\t\t\t</td>\r\n\t\t\t<tr>\r\n\t\t\t<td class=\"noBorder\">\r\n\t\t\t\t<table class=\"wide\">\r\n\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t<td class=\"positionAnchor infoCell padTop\">\r\n\t\t\t\t\t\t\t<span class=\"subtitle\">Ability Description</span>&nbsp;&nbsp;<span>");
    #line default
            try
            {
    #line 315 "CharacterSheet"
                                                                                                   Output.Write(Poke.Ability);
    #line default
            }
            catch(System.NullReferenceException)
            {
                Output.Write("${Poke.Ability}");
            }
    #line hidden
            Output.Write("</span>\r\n\t\t\t\t\t\t</td>\r\n\t\t\t\t\t</tr>\r\n\t\t\t\t</table>\r\n\t\t\t</td>\r\n\t\t\t</tr>\r\n\t\t\t<tr>\r\n\t\t\t<td class=\"noBorder\">\r\n\t\t\t\t<h2>Moves</h2>");
    #line default
            {
    #line 323 "CharacterSheet"
                                  foreach(var move in Poke.Moves)
    #line default
                {
    #line hidden
    #line default
    #line hidden
                    Output.Write("\r\n\t\t\t\t\t<table class=\"wide\">\r\n\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t<td class=\"positionAnchor infoCell padTop noBorderBottom\">\r\n\t\t\t\t\t\t\t\t<span class=\"subtitle\">Name</span>&nbsp;&nbsp;<span>");
    #line default
                    try
                    {
    #line 328 "CharacterSheet"
                                                                                        Output.Write(move.Name);
    #line default
                    }
                    catch(System.NullReferenceException)
                    {
                        Output.Write("${move.Name}");
                    }
    #line hidden
                    Output.Write("</span>\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"positionAnchor infoCell padTop noBorderBottom\">\r\n\t\t\t\t\t\t\t\t<span class=\"subtitle\">Type</span>&nbsp;&nbsp;<span>");
    #line default
                    try
                    {
    #line 331 "CharacterSheet"
                                                                                        Output.Write(move.Type.GetValueOrDefault(ElementalType.Normal));
    #line default
                    }
                    catch(System.NullReferenceException)
                    {
                        Output.Write("${move.Type.GetValueOrDefault(ElementalType.Normal)}");
                    }
    #line hidden
                    Output.Write("</span>\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"positionAnchor infoCell padTop noBorderBottom\">\r\n\t\t\t\t\t\t\t\t<span class=\"subtitle\">Category</span>&nbsp;&nbsp;<span>");
    #line default
                    try
                    {
    #line 334 "CharacterSheet"
                                                                                            Output.Write(move.Category.GetValueOrDefault(PokemonMoveCategory.Physical));
    #line default
                    }
                    catch(System.NullReferenceException)
                    {
                        Output.Write("${move.Category.GetValueOrDefault(PokemonMoveCategory.Physical)}");
                    }
    #line hidden
                    Output.Write("</span>\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"positionAnchor infoCell padTop noBorderBottom\">\r\n\t\t\t\t\t\t\t\t<span class=\"subtitle\">Power</span>&nbsp;&nbsp;<span>");
    #line default
                    try
                    {
    #line 337 "CharacterSheet"
                                                                                         Output.Write(move.BasePower.HasValue ? move.BasePower.Value.ToString() : "-");
    #line default
                    }
                    catch(System.NullReferenceException)
                    {
                        Output.Write("${move.BasePower.HasValue ? move.BasePower.Value.ToString() : \"-\"}");
                    }
    #line hidden
                    Output.Write("</span>\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"positionAnchor infoCell padTop noBorderBottom\">\r\n\t\t\t\t\t\t\t\t<span class=\"subtitle\">Atk.&nbsp;Dice</span>&nbsp;&nbsp;<span>");
    #line default
                    try
                    {
    #line 340 "CharacterSheet"
                                                                                                  Output.Write(move.AttackDice(Poke).HasValue ? move.AttackDice(Poke).Value.ToString() : "-");
    #line default
                    }
                    catch(System.NullReferenceException)
                    {
                        Output.Write("${move.AttackDice(Poke).HasValue ? move.AttackDice(Poke).Value.ToString() : \"-\"}");
                    }
    #line hidden
                    Output.Write("</span>\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"positionAnchor infoCell padTop noBorderBottom\">\r\n\t\t\t\t\t\t\t\t<span class=\"subtitle\">Accuracy</span>&nbsp;&nbsp;<span>");
    #line default
                    try
                    {
    #line 343 "CharacterSheet"
                                                                                            Output.Write(move.AccuracyP20);
    #line default
                    }
                    catch(System.NullReferenceException)
                    {
                        Output.Write("${move.AccuracyP20}");
                    }
    #line hidden
                    Output.Write("</span>\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"positionAnchor infoCell padTop noBorderBottom\">\r\n\t\t\t\t\t\t\t\t<span class=\"subtitle\">Priority</span>&nbsp;&nbsp;<span>");
    #line default
                    try
                    {
    #line 346 "CharacterSheet"
                                                                                            Output.Write(move.Priority);
    #line default
                    }
                    catch(System.NullReferenceException)
                    {
                        Output.Write("${move.Priority}");
                    }
    #line hidden
                    Output.Write("</span>\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"positionAnchor infoCell padTop noBorderBottom\">\r\n\t\t\t\t\t\t\t\t<span class=\"subtitle\">Level</span>&nbsp;&nbsp;<span>");
    #line default
                    try
                    {
    #line 349 "CharacterSheet"
                                                                                         Output.Write(move.LearnedAtLevel);
    #line default
                    }
                    catch(System.NullReferenceException)
                    {
                        Output.Write("${move.LearnedAtLevel}");
                    }
    #line hidden
                    Output.Write("</span>\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t\t<td class=\"positionAnchor infoCell padTop noBorderBottom\">\r\n\t\t\t\t\t\t\t\t<span class=\"subtitle\">Targets</span>&nbsp;&nbsp;<span>");
    #line default
                    try
                    {
    #line 352 "CharacterSheet"
                                                                                           Output.Write(move.Targets);
    #line default
                    }
                    catch(System.NullReferenceException)
                    {
                        Output.Write("${move.Targets}");
                    }
    #line hidden
                    Output.Write("</span>\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t</table>\r\n\t\t\t\t\t<table class=\"wide\">\r\n\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t<td class=\"positionAnchor infoCell padTop noBorderBottom\">\r\n\t\t\t\t\t\t\t\t<span class=\"subtitle\">Description</span>&nbsp;&nbsp;<span>");
    #line default
                    try
                    {
    #line 359 "CharacterSheet"
                                                                                               Output.Write(move.Description);
    #line default
                    }
                    catch(System.NullReferenceException)
                    {
                        Output.Write("${move.Description}");
                    }
    #line hidden
                    Output.Write("</span>\r\n\t\t\t\t\t\t\t</td>\r\n\t\t\t\t\t\t</tr>\r\n\t\t\t\t\t</table>");
    #line default
    #line hidden
    #line default
                }
            }
    #line hidden
            Output.Write("\r\n\t\t\t</tr>\r\n\t\t\t</td>\r\n\t\t\t</table>\r\n\t\t</div>\r\n\t</body>\r\n</html>\r\n");
    #line default
        }
    }
}
