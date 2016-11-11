using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessFileIO.Enums;
using System.Collections;

namespace ChessFileIO.Models
{
    public class PieceLogic : GameLogic
    {
        private ArrayList pieceLocations = new ArrayList();
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
            else if (piece == ChessTypes.Pawn.ToString().Substring(0, 1))
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
            return pieceLocations;
        }
        private ArrayList Queen(int file, int rank)
        {
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
