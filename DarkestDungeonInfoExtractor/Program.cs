using System.Diagnostics.CodeAnalysis;
using System.Text;
using CommandLine;
using static System.ArgumentNullException;

namespace DarkestDungeonInfoExtractor
{
    internal class Program
    {
        static void Main(string[] args)
        {

            CommandLineOptions options = CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args).Value;

            if (options == null) return;

            if (!Directory.Exists(options.InfoFilesDirectory))
            {

                Console.WriteLine("Directory not found");
                return;
            }

            ProcessMonsterInfo(options.InfoFilesDirectory, options.OutputFile);
        }

        /// <summary>
        /// Processes all of the *.info.darkest files in the path specified.  Outputs the data as CSV to the outputFile
        /// </summary>
        /// <param name="infoPath">The root folder to search for *.info.darkest files </param>
        /// <param name="outputFile">The file that will be written to with the CSV formatted data.</param>
        /// <exception cref="NotImplementedException"></exception>
        public static void ProcessMonsterInfo(string infoPath, string outputFile)
        {

            using StreamWriter writer = new (outputFile);

            //write header
            writer.WriteLine("MonsterName,Skill,IsMultiTarget, IsFriendlyTarget, TargetGlyph, TargetPos1,TargetPos2,TargetPos3,TargetPos4,LaunchGlyph, LaunchPos1,LaunchPos2,LaunchPos3,LaunchPos4");
            int lineNumber = 1;

            try
            {
                foreach (string file in Directory.EnumerateFiles(infoPath, "*.info.darkest", SearchOption.AllDirectories))
                {

                    string name = Path.GetFileNameWithoutExtension(file.Replace(".info.darkest", ""));

                    string fileText = File.ReadAllText(file);

                    //Fix for the one file that is missing a space between properties.
                    fileText = fileText.Replace(".target 1234.is_user_selected_targets", ".target 1234 .is_user_selected_targets");

                    List<InfoData> data = InfoData.ParseInfo(fileText);

                    MonsterInfo info = new MonsterInfo(name, data);

                    foreach (Skill skill in info.Skills)
                    {
                        StringBuilder sb = new StringBuilder();

                        sb.AppendComma(info.Name);
                        sb.AppendComma(skill.Name);
                        sb.AppendComma(skill.IsMultiTarget.ToString());
                        sb.AppendComma(skill.IsFriendlyTarget.ToString());
                        sb.AppendComma(skill.PositionToGlyph(false));
                        sb.AppendJoin(',', skill.ToFixedArray(false)).AppendComma();
                        sb.AppendComma(skill.PositionToGlyph(true));
                        sb.AppendJoin(',', skill.ToFixedArray(true));
                        writer.WriteLine(sb.ToString());
                    }

                    lineNumber++;


                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error processing line {lineNumber} {ex}",ex);
            }
        }

    }
}