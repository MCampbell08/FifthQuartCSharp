using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChessFileIO.Utilities
{
    public class BasicUtilities
    {
        public int MoveSeparator(string line)
        {
            int counter = 0;
            int breakPoint = 0;

            foreach (char c in line)
            {
                if (c == '\t' || c == '\n' || c == ' ')
                {
                    breakPoint = counter;
                }
                counter++;
            }
            return breakPoint;
        }
        public string WhitespaceRemover(string input)
        {
            Match newInput = Regex.Match(input, @"[\S]+");
            return newInput.Value;
        }
    }
}
