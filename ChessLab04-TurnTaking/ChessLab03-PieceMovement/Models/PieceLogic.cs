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
        private const int MAX_UP_DIRECTION = 8;
        private const int MAX_DOWN_DIRECTION = 0;
        public ArrayList AppropriatePiece(string piece, string action, int oldFile, int oldRank, int newFile, int newRank)
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
            else if (piece == ChessTypes.Knight.ToString().Substring(1, 1).ToUpper())
            {
                locations = Knight(oldFile, oldRank, newFile, newRank);
            }
            else if (piece == ChessTypes.Bishop.ToString().Substring(0, 1))
            {
                locations = Bishop(oldFile, oldRank, newFile, newRank);
            }
            else if (piece == "")
            {
                locations = Pawn(action, oldFile, oldRank);
            }
            else if (piece == ChessTypes.Rook.ToString().Substring(0, 1))
            {
                locations = Rook(oldFile, oldRank, newFile, newRank);
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
            pieceLocations = Rook(oldFile, oldRank, newFile, newRank);
            pieceLocations = Bishop(oldFile, oldRank, newFile, newRank);
            return pieceLocations;
        }
        private ArrayList Bishop(int oldFile, int oldRank, int newFile, int newRank)
        {
            if (newFile > oldFile && newRank > oldRank)
            {
                TopRightBishop(oldFile, oldRank, newFile, newRank);
            }
            else if (newFile > oldFile && newRank < oldRank)
            {
                TopLeftBishop(oldFile, oldRank, newFile, newRank);
            }
            else if (newFile < oldFile && newRank < oldRank)
            {
                BottomLeftBishop(oldFile, oldRank, newFile, newRank);
            }
            else if (newFile < oldFile && newRank > oldRank)
            {
                BottomRightBishop(oldFile, oldRank, newFile, newRank);
            }
            return pieceLocations;
        }
        private void TopRightBishop(int oldFile, int oldRank, int newFile, int newRank)
        {
            int tempRank = oldRank;
            bool clearMoves = false;
            for (int tempFile = oldFile; tempFile < MAX_UP_DIRECTION; tempFile++)
            {
                if (tempFile >= 0 && tempRank >= 0 && tempFile <= 7 && tempRank <= 7)
                {
                    if (tempFile != oldFile && tempRank != oldRank && tempFile < newFile && tempRank < newRank)
                    {
                        if (chessBoard[tempFile, tempRank] != BoardPiece(ChessTypes.Empty) && tempFile != newFile && tempRank != newRank)
                        {
                            clearMoves = true;
                        }
                    }
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    tempRank++;
                }
            }
            if (clearMoves)
            {
                pieceLocations.Clear();
            }
        }
        private void TopLeftBishop(int oldFile, int oldRank, int newFile, int newRank)
        {
            int tempRank = oldRank;
            bool clearMoves = false;
            for (int tempFile = oldFile; tempFile < MAX_UP_DIRECTION; tempFile++)
            {
                if (tempFile >= 0 && tempRank >= 0 && tempFile <= 7 && tempRank <= 7)
                {
                    if (tempFile != oldFile && tempRank != oldRank && tempFile < newFile && tempRank > newRank)
                    {
                        if (chessBoard[tempFile, tempRank] != BoardPiece(ChessTypes.Empty) && tempFile != newFile && tempRank != newRank)
                        {
                            clearMoves = true;
                        }
                    }
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    tempRank--;
                }
            }
            if (clearMoves)
            {
                pieceLocations.Clear();
            }
        }
        private void BottomLeftBishop(int oldFile, int oldRank, int newFile, int newRank)
        {
            int tempRank = oldRank;
            bool clearMoves = false;
            for (int tempFile = oldFile; tempFile >= MAX_DOWN_DIRECTION; tempFile--)
            {
                if (tempFile >= 0 && tempRank >= 0 && tempFile <= 7 && tempRank <= 7)
                {
                    if (tempFile != oldFile && tempRank != oldRank && tempFile > newFile && tempRank > newRank)
                    {
                        if (chessBoard[tempFile, tempRank] != BoardPiece(ChessTypes.Empty) && tempFile != newFile && tempRank != newRank)
                        {
                            clearMoves = true;
                        }
                    }
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    tempRank--;
                }
            }
            if (clearMoves)
            {
                pieceLocations.Clear();
            }
        }
        private void BottomRightBishop(int oldFile, int oldRank, int newFile, int newRank)
        {
            int tempRank = oldRank;
            bool clearMoves = false;
            for (int tempFile = oldFile; tempFile >= MAX_DOWN_DIRECTION; tempFile--)
            {
                if (tempFile >= 0 && tempRank >= 0 && tempFile <= 7 && tempRank <= 7)
                {
                    if (tempFile != oldFile && tempRank != oldRank && tempFile > newFile && tempRank < newRank)
                    {
                        if (chessBoard[tempFile, tempRank] != BoardPiece(ChessTypes.Empty) && tempFile != newFile && tempRank != newRank)
                        {
                            clearMoves = true;
                        }
                    }
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    tempRank++;
                }
            }
            if (clearMoves)
            {
                pieceLocations.Clear();
            }
        }
        private ArrayList Knight(int oldFile, int oldRank, int newFile, int newRank)
        {
            int tempFile = oldFile;
            int tempRank = oldRank;

            if (newFile > oldFile && newRank > oldRank)
            {
                tempFile += 1; tempRank += 2;
                pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                tempFile += 1; tempRank -= 1;
                pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
            }
            else if (newFile < oldFile && newRank > oldRank)
            {
                tempFile -= 1; tempRank += 2;
                pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                tempFile -= 1; tempRank -= 1;
                pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
            }
            else if (newFile < oldFile && newRank < oldRank)
            {
                tempFile -= 1; tempRank -= 2;
                pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                tempFile -= 1; tempRank += 1;
                pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
            }
            else if (newFile > oldFile && newRank < oldRank)
            {
                tempFile += 1; tempRank -= 2;
                pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                tempFile += 1; tempRank += 1;
                pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
            }
            return pieceLocations;
        }
        private ArrayList Rook(int oldFile, int oldRank, int newFile, int newRank)
        {
            if (newFile > oldFile && newRank == oldRank)
            {
                UpRook(oldFile, oldRank, newFile, newRank);
            }
            if (newFile < oldFile && newRank == oldRank)
            {
                DownRook(oldFile, oldRank, newFile, newRank);
            }
            if (newFile == oldFile && newRank < oldRank)
            {
                LeftRook(oldFile, oldRank, newFile, newRank);
            }
            if (newFile == oldFile && newRank > oldRank)
            {
                RightRook(oldFile, oldRank, newFile, newRank);
            }
            return pieceLocations;
        }
        private void UpRook(int oldFile, int oldRank, int newFile, int newRank)
        {
            int tempRank = oldRank;
            bool clearMoves = false;
            for (int tempFile = oldFile; tempFile < MAX_UP_DIRECTION; tempFile++)
            {
                if (tempFile >= 0 && tempFile <= 7)
                {
                    if (tempFile != oldFile && tempFile < newFile)
                    {
                        if (chessBoard[tempFile, tempRank] != BoardPiece(ChessTypes.Empty) && tempFile != newFile)
                        {
                            clearMoves = true;
                        }
                    }
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                }
            }
            if (clearMoves)
            {
                pieceLocations.Clear();
            }
        }
        private void DownRook(int oldFile, int oldRank, int newFile, int newRank)
        {
            int tempRank = oldRank;
            bool clearMoves = false;
            for (int tempFile = oldFile; tempFile >= MAX_DOWN_DIRECTION; tempFile--)
            {
                if (tempFile >= 0 && tempFile <= 7)
                {
                    if (tempFile != oldFile && tempFile > newFile)
                    {
                        if (chessBoard[tempFile, tempRank] != BoardPiece(ChessTypes.Empty) && tempFile != newFile)
                        {
                            clearMoves = true;
                        }
                    }
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                }
            }
            if (clearMoves)
            {
                pieceLocations.Clear();
            }
        }
        private void LeftRook(int oldFile, int oldRank, int newFile, int newRank)
        {
            int tempFile = oldFile;
            bool clearMoves = false;
            for (int tempRank = oldRank; tempRank >= MAX_DOWN_DIRECTION; tempRank--)
            {
                if (tempRank >= 0 && tempRank <= 7)
                {
                    if (tempRank != oldRank && tempRank > newRank)
                    {
                        if (chessBoard[tempFile, tempRank] != BoardPiece(ChessTypes.Empty) && tempRank != newRank)
                        {
                            clearMoves = true;
                        }
                    }
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                }
            }
            if (clearMoves)
            {
                pieceLocations.Clear();
            }
        }
        private void RightRook(int oldFile, int oldRank, int newFile, int newRank)
        {
            int tempFile = oldFile;
            bool clearMoves = false;
            for (int tempRank = oldRank; tempRank < MAX_UP_DIRECTION; tempRank++)
            {
                if (tempRank >= 0 && tempRank <= 7)
                {
                    if (tempRank != oldRank && tempRank < newRank)
                    {
                        if (chessBoard[tempFile, tempRank] != BoardPiece(ChessTypes.Empty) && tempRank != newRank)
                        {
                            clearMoves = true;
                        }
                    }
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                }
            }
            if (clearMoves)
            {
                pieceLocations.Clear();
            }
        }
        private ArrayList Pawn(string action, int file, int rank)
        {
            bool isAttack = false;
            bool canMoveTwo = false;
            int tempFile = file;
            int tempRank = rank;

            if (action == "x") { isAttack = true; } else if (action == "-") { isAttack = false; }

            if (file == 1 && chessBoard[ file, rank] == "PL")
            {
                canMoveTwo = true;
            }
            else if (file == 6 && chessBoard[file, rank] == "pd")
            {
                canMoveTwo = true;
            }


            if (chessBoard[file, rank] == "PL")
            {
                if (isAttack)
                {
                    tempFile += 1; tempRank += 1;
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    tempFile -= 2;
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                }
                else
                {
                    tempFile += 1;
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    if (canMoveTwo)
                    {
                        tempFile += 1;
                        pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    }
                }
            }
            else if (chessBoard[file, rank] == "pd")
            {
                if (isAttack)
                {
                    tempFile -= 1; tempRank -= 1;
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    tempFile += 2;
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                }
                else
                {
                    tempFile -= 1;
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    if (canMoveTwo)
                    {
                        tempFile -= 1;
                        pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    }
                }
            }

            return pieceLocations;
        }
    }
}
