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
        private const int MAX_DIRECTION = 8;
        public ArrayList AppropriatePiece(string piece, int oldFile, int oldRank, int newFile, int newRank)
        {
            ArrayList locations = new ArrayList();

            if (piece == ChessTypes.King.ToString().Substring(0, 1))
            {
                locations = King(oldFile, oldRank);
            }
            else if (piece == ChessTypes.Queen.ToString().Substring(0, 1))
            {
                locations = Queen(oldFile, oldRank, newFile, newRank);
            }
            else if (piece == ChessTypes.Knight.ToString().Substring(1, 1))
            {
                locations = Knight(oldFile, oldRank);
            }
            else if (piece == ChessTypes.Bishop.ToString().Substring(0, 1))
            {
                locations = Bishop(oldFile, oldRank, newFile, newRank);
            }
            else if (piece == "")
            {
                locations = Pawn(oldFile, oldRank);
            }
            else if (piece == ChessTypes.Rook.ToString().Substring(0, 1))
            {
                locations = Rook(oldFile, oldRank);
            }
            return locations;
        }

        private ArrayList King(int file, int rank)
        {
            for (int tempFile = file - 1; tempFile < file + 2; tempFile++)
            {
                for (int tempRank = rank - 1; tempRank < rank + 2; tempRank++)
                {
                    if (tempFile >= 0 && tempRank >= 0 && tempFile <= 7 && tempRank <= 7)
                    {
                        pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    }
                }
            }
            for (int tempFile = 0; tempFile < pieceLocations.Count; tempFile++)
            {
                if (pieceLocations.Contains(file.ToString() + rank.ToString()))
                {
                    pieceLocations.Remove(file.ToString() + rank.ToString());
                }
            }
            return pieceLocations;
        }
        private ArrayList Queen(int oldFile, int oldRank, int newFile, int newRank)
        {
            pieceLocations = Rook(oldFile, oldRank);
            pieceLocations = Bishop(oldFile, oldRank, newFile, newRank);
            return pieceLocations;
        }
        private ArrayList Bishop(int oldFile, int oldRank, int newFile, int newRank)
        {
            int tempRank = oldRank;
            for (int tempFile = oldFile; tempFile < MAX_DIRECTION; tempFile++)
            {
                if (tempFile >= 0 && tempRank >= 0 && tempFile <= 7 && tempRank <= 7)
                {
                    if (chessBoard[tempFile, tempRank] == BoardPiece(ChessTypes.Empty))
                    {
                        pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                        Console.WriteLine("Locations NorthEast: [{0}]", tempFile.ToString() + tempRank.ToString());
                    }
                    else
                    {
                        if (tempFile == newFile && tempRank == newRank)
                        {
                            pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                            Console.WriteLine("Locations NorthEast: [{0}]", tempFile.ToString() + tempRank.ToString());
                        }
                        else
                        {
                            Console.WriteLine(String.Format("[{0,-7}]    Cannot hop pieces.", "Error"));
                            if (pieceLocations != null)
                            {
                                foreach (string s in pieceLocations)
                                {
                                    if (s != null)
                                    {
                                        pieceLocations.Remove(s);
                                    }
                                }
                            }
                        }
                    }
                    tempRank++;
                }
            }
            //tempRank = oldRank;
            //for (int tempFile = oldFile; tempFile < MAX_DIRECTION; tempFile--)
            //{
            //    if (tempFile >= 0 && tempRank >= 0 && tempFile <= 7 && tempRank <= 7)
            //    {
            //        pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
            //        Console.WriteLine("Locations SouthWest: [{0}]", tempFile.ToString() + tempRank.ToString());
            //        tempRank--;
            //    }
            //}
            //tempRank = oldRank;
            //for (int tempFile = oldFile; tempFile < MAX_DIRECTION; tempFile++)
            //{
            //    if (tempFile >= 0 && tempRank >= 0 && tempFile <= 7 && tempRank <= 7)
            //    {
            //        pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
            //        Console.WriteLine("Locations NorthWest: [{0}]", tempFile.ToString() + tempRank.ToString());
            //        tempRank--;
            //    }
            //}
            //tempRank = oldRank;
            //for (int tempFile = oldFile; tempFile < MAX_DIRECTION; tempFile--)
            //{
            //    if (tempFile >= 0 && tempRank >= 0 && tempFile <= 7 && tempRank <= 7)
            //    {
            //        pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
            //        Console.WriteLine("Locations SouthEast: [{0}]", tempFile.ToString() + tempRank.ToString());
            //        tempRank++;
            //    }
            //}
            return pieceLocations;
        }
        private bool TopRightBishop(int file, int rank)
        {
            bool validMove = false;



            return validMove;

        }
        private ArrayList Knight(int file, int rank)
        {
            //------------Adding location------------
            //if (tempFile>= 0 && j >= 0 && tempFile<= 7 && j <= 7)
            //{
            //    pieceLocations.Add(i.ToString() + j.ToString());
            //}
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
