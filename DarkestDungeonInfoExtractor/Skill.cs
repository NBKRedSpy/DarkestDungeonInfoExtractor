using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DarkestDungeonInfoExtractor
{

    
    /// <summary>
    /// A monster skill
    /// </summary>
    public class Skill
    {
        /// <summary>
        /// The maximum number of positions that the game uses.
        /// </summary>
        public const int MaxPositionCount = 4;

        public string Name { get; set; } = "";

        /// <summary>
        /// The position that the attacker must be in to use.
        /// </summary>
        public List<int> LaunchPositions { get; set; } = new List<int>();

        /// <summary>
        /// The possible target positions.  May be in preference order.
        /// </summary>
        public List<int> TargetPositions { get; set; } = new List<int>();

        /// <summary>
        /// If false the attack is for only one of the positions.  
        /// Otherwise the attack targets all of the listed positions.
        /// </summary>
        public bool IsMultiTarget { get; set; }

        /// <summary>
        /// If true, the target is for a friendly, otherwise is an enemy.
        /// </summary>
        public bool IsFriendlyTarget { get;set; }


        /// <summary>
        /// Returns the position list as a graphics string.  Ex: ○◉◉○
        /// </summary>
        /// <param name="isLaunch">If true, use the launch positions, else the target positions</param>
        /// <returns></returns>
        public string PositionToGlyph(bool isLaunch)
        {

            List<int> positions = isLaunch ? LaunchPositions : TargetPositions;

            if (positions.Count == 0)
            {
                //Should be a self cast or friendly only cast.
                return "";
            }

            StringBuilder sb = new StringBuilder();
            bool previousSet = false;  //True if the previous position was set.


            for (int i = 0; i < MaxPositionCount; i++)
            {

                bool set = positions.Any(x => x == i + 1);

                if (isLaunch == false && set && IsMultiTarget && previousSet)
                {
                    sb.Append('-');
                }

                sb.Append(set ? '◉' : '○');

                previousSet = set;

            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a fixed bool array indicating the columns that are valid positions. One for each position.
        /// </summary>
        /// <param name="isLaunch">If true, use the launch positions, else the target positions</param>
        /// <returns></returns>
        public bool[] ToFixedArray(bool isLaunch)
        {
            List<int> positions = isLaunch ? LaunchPositions : TargetPositions;

            bool[] validPositions = new bool[MaxPositionCount];

            foreach (int position in positions)
            {
                validPositions[position -1] = true;
            }

            return validPositions;
        }
     }

}
