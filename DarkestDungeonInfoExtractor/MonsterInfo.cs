using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkestDungeonInfoExtractor
{
    public class MonsterInfo
    {
        public string Name { get; set; } = "";

        /// <summary>
        /// The target locations.  Should be 1-4.  Can be in preferred order.
        /// </summary>
        public List<Skill> Skills { get; set; } = new List<Skill>();

        public MonsterInfo()
        {
                
        }


        /// <summary>
        /// the parsed data
        /// </summary>
        /// <param name="monsterName">The name of the file.  </param>
        /// <param name="monsterData"></param>
        public MonsterInfo(string monsterName, List<InfoData> monsterData)
        {

            //The name of the monster is not in the file data.
            Name = monsterName;

            //Get skills
            foreach (InfoData data in monsterData.Where(x=> x.Type == "skill"))
            {
                //Skill Name
                Skill skill = new Skill();
                string keyValue = data.Values.Single(x => x.Key == "id").Value;

                skill.Name = keyValue;


                //Launch locations
                skill.LaunchPositions = ParsePositions("launch", data, out _, out _);
                skill.TargetPositions = ParsePositions("target", data, out bool isMultiTarget, out bool isFriendly);
                skill.IsMultiTarget = isMultiTarget;
                skill.IsFriendlyTarget = isFriendly;

                Skills.Add(skill);
            }
        }

        /// <summary>
        /// Returns the array of integer positions in a position value.
        /// EG:  "1234".
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static List<int> ParsePositions(string keyName, InfoData data, out bool isMultiTarget, out bool isFriendly)
        {
            string positionStringData = data.Values.Single(x => x.Key == keyName).Value!;

            //No targets.  Should be a self or friendly cast.
            if (string.IsNullOrWhiteSpace(positionStringData))
            {
                isMultiTarget = false;
                isFriendly = false;
                return new List<int>();
            }

            isMultiTarget = positionStringData.IndexOf('~') != -1;
            isFriendly = positionStringData.IndexOf('@') != -1;

            //TODO:  Add the @1234.  I think @ seems to mean target friendly.  I wonder if a "" target means no target required.
            // I think types will be Enemy, Friendly.

            //There is also a combo:
            // .target @~1234.  So I guess can be multi and friendly.

            //Remove the special tags
            positionStringData = positionStringData.Trim('~', '@');

            List<int> positions = new(); 

            foreach (char position in positionStringData)
            {
                positions.Add(Convert.ToInt32(position.ToString()));
            }

           return positions;
        }
    }
}
