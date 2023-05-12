using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace DarkestDungeonInfoExtractor
{
    public class CommandLineOptions
    {
        [Value(0,Required = true, HelpText = "The monsters folder of Darkest Dungeon.  Will be [DarkestDungeon folder]\\monsters")]
        public string InfoFilesDirectory { get; set; } = "";


        [Value(1, Required = true, HelpText = "The name of the output file")]
        public string OutputFile { get; set; } = "";
    }
}
