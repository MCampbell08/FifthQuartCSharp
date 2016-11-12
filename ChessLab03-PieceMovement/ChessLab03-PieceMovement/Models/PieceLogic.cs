using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessFileIO.Enums;
using System.Collections;
using ChessFileIO.Utilities;

namespace ChessFileIO.Models
{
    public class PieceLogic : BoardLayout
    {
        private ArrayList pieceLocations = new ArrayList(64);
        public ArrayList AppropriatePiece(string piece, int file, int rank)
        {
            ArrayList locations = new ArrayList();

            if (piece == ChessTypes.King.ToString().Substring(0, 1))
            {
                locations = King(file, rank);
            }
            else if (piece == ChessTypes.Queen.ToString().Substring(0, 1))
            {
                locations = Queen(file, rank);
            }
            else if (piece == ChessTypes.Knight.ToString().Substring(1, 1))
            {
                locations = Knight(file, rank);
            }
            else if (piece == ChessTypes.Bishop.ToString().Substring(0, 1))
            {
                locations = Bishop(file, rank);
            }
            else if (piece == "")
            {
                locations = Pawn(file, rank);
            }
            else if (piece == ChessTypes.Rook.ToString().Substring(0, 1))
            {
                locations = Rook(file, rank);
            }
            return locations;
        }

        private ArrayList King(int file, int rank)
        {
            for (int i = file - 2; i < file + 1; i++)
            {
                for (int j = rank - 2; j < rank + 1; j++)
                {
                    pieceLocations.Add(i.ToString() + j.ToString());
                    Console.WriteLine("Possible Locations: " + i.ToString() + j.ToString());
                }
            }
            for (int i = 0; i < pieceLocations.Count; i++)
            {
                if (pieceLocations.Contains(file.ToString() + rank.ToString()))
                {
                    Console.WriteLine(file.ToString() + rank.ToString());
                    pieceLocations.Remove(file.ToString() + rank.ToString());
                }
            }
            return pieceLocations;
        }
        private ArrayList Queen(int file, int rank)
        {
            pieceLocations = Rook(file, rank);
            pieceLocations = Bishop(file, rank);
            return pieceLocations;
        }
        private ArrayList Bishop(int file, int rank)
        {
            return pieceLocations;
        }
        private ArrayList Knight(int file, int rank)
        {
            return pieceLocations;
        }
        private ArrayList Rook(int file, int rank)
        {
            return pieceLocations;
        }
        private ArrayList Pawn(int file, int rank)
        {
            return pieceLocations;
        }
    }
}
