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
        public ChessTypes NameSelector(string input)
        {
            ChessTypes type = new ChessTypes();
            if (input == "K")
            {
                type = ChessTypes.King;
            }
            else if (input == "Q")
            {
                type = ChessTypes.Queen;
            }
            else if (input == "B")
            {
                type = ChessTypes.Bishop;
            }
            else if (input == "R")
            {
                type = ChessTypes.Rook;
            }
            else if (input == "N")
            {
                type = ChessTypes.Knight;
            }
            else if (input == "P" || input == "")
            {
                type = ChessTypes.Pawn;
            }
            return type;
        }
    }
}
