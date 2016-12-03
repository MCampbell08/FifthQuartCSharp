using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChessFileIO.Models;

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
        public int[] StringLocParser(string input)
        {
            int[] location = new int[2];
            if (input != null && input != "")
            {
                if (input.Substring(0, 1) == "-")
                {
                    location[0] = int.Parse(input.Substring(1, 1));
                    location[1] = int.Parse(input.Substring(2, 1));
                }
                else
                {
                    location[0] = int.Parse(input.Substring(0, 1));
                    location[1] = int.Parse(input.Substring(1, 1));
                }
            }

            return location;
        }
    }
}
