using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessFileIO.Enums;

namespace ChessFileIO.Models
{
    public class PlayerPiece
    {
        private string _location;
        public ChessTypes ToEnum(string input)
        {
            ChessTypes output = ChessTypes.Empty;
            if (input == " " || input == "--")
            {
                output = ChessTypes.Empty;
            }
            else if (input == "K" || input == "King")
            {
                output = ChessTypes.King;
            }
            else if (input == "Q" || input == "Queen")
            {
                output = ChessTypes.Queen;
            }
            else if (input == "B" || input == "Bishop")
            {
                output = ChessTypes.Bishop;
            }
            else if (input == "R" || input == "Rook")
            {
                output = ChessTypes.Rook;
            }
            else if (input == "P" || input == "Pawn" || input == "")
            {
                output = ChessTypes.Pawn;
            }
            else if (input == "N" || input == "Knight")
            {
                output = ChessTypes.Knight;
            }
            return output;
        }
        public string PieceLocation
        {
            get { return _location; }
            set { _location = value; }
        }
        public string PieceColor(string color, bool placement = false)
        {
            if (!placement)
            {
                if (color == "White")
                {
                    color = "l";
                }
                else if (color == "Black")
                {
                    color = "d";
                }
            }
            else if (placement)
            {
                if (color == "l")
                {
                    color = "White";
                }
                else if (color == "d")
                {
                    color = "Black";
                }
            }
            return color;
        }
        public string ActionTaken(string action)
        {
            if (action == "-")
            {
                action = ". ";
            }
            else if (action == "x")
            {
                action = ", Capturing opponent's piece. ";
            }
            else if (action == "+")
            {
                action = "Check! ";
            }
            else if (action == "#")
            {
                action = "Checkmate! ";
            }
            return action;
        }
    }
}
