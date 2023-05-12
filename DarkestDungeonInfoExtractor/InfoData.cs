using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DarkestDungeonInfoExtractor
{
    /// <summary>
    /// The Darkest Dungeon monster info parsed from a *.info.darkest file.
    /// </summary>
    [DebuggerDisplay("Type={Type})")]
    public class InfoData
    {
        public string Type { get; set; } = "";
        public List<KeyValuePair<string, string>> Values { get; set; } = new List<KeyValuePair<string, string>>();


        /// <summary>
        /// Returns all of the InfoData from a *.info.darkest file.
        /// </summary>
        /// <param name="data">The contents of a *.info.darkest file. </param>
        /// <returns></returns>
        public static List<InfoData> ParseInfo(string data)
        {
            try
            {

                

                List<InfoData> infos = new List<InfoData>();

                StringReader sr = new StringReader(data);

                string line;

                //Example data line from *info.darkest files:
                //stats: .hp 14 .def 0% .prot 0 .spd 0 .stun_resist 50% .poison_resist 10% .bleed_resist 20% .debuff_resist 10% .move_resist 25%
                //skill: .id "explode" .type "melee" .atk 72.5% .dmg 5 13 .crit 2%  .effect "Thrall Revenge Stress 1" "kill_self_queued" .launch 4321 .target ~4321 .can_be_riposted false
                //skill: .id "bloated_swipe" .type "melee" .atk 62.5% .dmg 5 9 .crit 2%  .effect "Stress 1" .launch 1234 .target 12

                while ((line = sr.ReadLine()!) != null)
                {
                    if (line == "") continue;

                    //Get property
                    string[] keyValues = line.Split(':');

                    InfoData file = new InfoData();

                    file.Type = keyValues[0];

                    //--Values
                    //Extract the key/value pairs by finding the .[property name].  However, there can be more than one value per entry
                    //   and there are decimal numbers that have a '.'.

                    file.Values = ParseKeyValueData(keyValues[1]);

                    infos.Add(file);    
                }

                return infos;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error processing line: '{data}'", ex);
            }
        }


        private static List<KeyValuePair<string,string>> ParseKeyValueData(string line)
        {
            Regex propertyStartRegEx = new Regex(@" \.([A-Za-z_]+) ");

            var propertyMatch = propertyStartRegEx.Match(line);

            if (propertyMatch.Success == false)
            {
                throw new ApplicationException("No properties found");
            }

            int previousMatchEnd = 0;

            string previousPropertyName = "";

            List<KeyValuePair<string,string>> keyValuePairs = new();



            do
            {
                if (propertyMatch.Index != 0)
                {
                    //The value is from the end of the last match to the start of this match.

                    string value = line.Substring(previousMatchEnd, propertyMatch.Index - previousMatchEnd);

                    keyValuePairs.Add(new KeyValuePair<string,string>(previousPropertyName, value));
                }

                previousMatchEnd = propertyMatch.Index + propertyMatch.Length;

                previousPropertyName = propertyMatch.Groups[1].Value;

                propertyMatch = propertyMatch.NextMatch();
            }
            while (propertyMatch.Success);

            //Update the remaining data.
            keyValuePairs.Add(new KeyValuePair<string, string>(previousPropertyName, line.Substring(previousMatchEnd)));

            return keyValuePairs;

        }
    }
}
