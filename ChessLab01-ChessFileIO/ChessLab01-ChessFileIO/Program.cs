﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChessFileIO
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StreamReader file = new StreamReader(args[0]);
            Console.WriteLine("Welcome to Chess!\n\nFile: " + args[0] + "\n\n ---------------------- \n");
            FileReader(file);
        }
        private static void FileReader(StreamReader fileName)
        {
            string line = "";

            while ((line = fileName.ReadLine()) != null)
            {
                Match comment = Regex.Match(line, @"(\s+)?(\/+).*");

                if (comment.Success)
                {
                    line = line.Remove(comment.Groups[1].Index);
                }
                else
                {
                    ChessParser parse = new ChessParser();
                    parse.RegexChooser(line);
                }
            }
            fileName.Close();
        }
    }
}
