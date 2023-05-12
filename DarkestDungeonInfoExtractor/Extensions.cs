using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkestDungeonInfoExtractor
{
    public static class Extensions
    {
        public static void AppendComma(this StringBuilder sb, string text)
        {
            sb.Append(text);
            sb.Append(",");
        }

        public static void AppendComma(this StringBuilder sb)
        {
            sb.Append(",");
        }

    }
}
