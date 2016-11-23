using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace Ginger.Util
{
    internal class HighlightUtils
    {
        private static Regex highlightRegex = new Regex(@"<mark>([^>]+)</mark>");

        public static List<Run> GetHighlightRunList(string value, Color color)
        {
            List<Run> runs = new List<Run>();
            var v = value;
            while (true)
            {
                var match = highlightRegex.Match(v);
                if (!match.Success)
                {
                    break;
                }

                runs.Add(new Run(v.Substring(0, match.Index)));
                runs.Add(CreateRun(match.Groups[1].Value, Colors.Red));

                v = v.Substring(match.Index + match.Length);
            }

            runs.Add(new Run(v));

            return runs;
        }

        private static Run CreateRun(string value, Color color)
        {
            var run = new Run();
            run.Foreground = new SolidColorBrush(color);
            run.Text = value;
            return run;
        }
    }
}
