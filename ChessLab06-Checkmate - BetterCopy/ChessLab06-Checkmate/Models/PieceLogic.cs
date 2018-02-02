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
        private BasicUtilities utilities = new BasicUtilities();
        private PlayerPiece piece = new PlayerPiece();
        private ArrayList pieceLocations = new ArrayList(64);
        private ArrayList kingCompetitors = new ArrayList();
        private int shadowPieceFile = 0;
        private int shadowPieceRank = 0;
        private bool cantAttack = true;
        private const int MAX_UP_DIRECTION = 8;
        private const int MAX_DOWN_DIRECTION = 0;
        public ArrayList AppropriatePiece(string piece, string action, int oldFile, int oldRank, int newFile, int newRank)
        {
            ArrayList locations = new ArrayList();

            if (piece == ChessTypes.King.ToString().Substring(0, 1))
            {
                locations = King(oldFile, oldRank, false);
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
                locations = Bishop(oldFile, oldRank, newFile, newRank, false);
            }
            else if (piece == "")
            {
                locations = Pawn(action, oldFile, oldRank);
            }
            else if (piece == ChessTypes.Rook.ToString().Substring(0, 1))
            {
                locations = Rook(oldFile, oldRank, newFile, newRank, false);
            }
            return locations;
        }

        private ArrayList King(int file, int rank, bool isCheck)
        {
            for (int tempFile = file - 1; tempFile < file + 2; tempFile++)
            {
                for (int tempRank = rank - 1; tempRank < rank + 2; tempRank++)
                {
                    if (tempFile >= 0 && tempRank >= 0 && tempFile <= 7 && tempRank <= 7)
                    {
                        if (!isCheck) { pieceLocations.Add(tempFile.ToString() + tempRank.ToString()); }
                        else { kingCompetitors.Add(tempFile.ToString() + tempRank.ToString()); }
                    }
                }
            }
            if (pieceLocations.Contains(file.ToString() + rank.ToString()) || kingCompetitors.Contains(file.ToString() + rank.ToString()))
            {
                if (!isCheck) { pieceLocations.Remove(file.ToString() + rank.ToString()); }
                else { kingCompetitors.Remove(file.ToString() + rank.ToString()); return kingCompetitors; }
            }
            return pieceLocations;
        }
        private ArrayList Queen(int oldFile, int oldRank, int newFile, int newRank)
        {
            if (oldFile == newFile && oldRank == newRank)
            {
                pieceLocations = Rook(oldFile, oldRank, newFile, newRank, true);
                pieceLocations = Bishop(oldFile, oldRank, newFile, newRank, true);
            }
            else
            {
                pieceLocations = Rook(oldFile, oldRank, newFile, newRank, false);
                pieceLocations = Bishop(oldFile, oldRank, newFile, newRank, false);
            }
            return pieceLocations;
        }
        private ArrayList Bishop(int oldFile, int oldRank, int newFile, int newRank, bool isCheck)
        {
            if (newFile > oldFile && newRank > oldRank)
            {
                TopRightBishop(oldFile, oldRank, newFile, newRank, isCheck);
            }
            else if (newFile > oldFile && newRank < oldRank)
            {
                TopLeftBishop(oldFile, oldRank, newFile, newRank, isCheck);
            }
            else if (newFile < oldFile && newRank < oldRank)
            {
                BottomLeftBishop(oldFile, oldRank, newFile, newRank, isCheck);
            }
            else if (newFile < oldFile && newRank > oldRank)
            {
                BottomRightBishop(oldFile, oldRank, newFile, newRank, isCheck);
            }
            else if (newFile == oldFile && newRank == oldRank)
            {
                TopRightBishop(oldFile, oldRank, newFile, newRank, isCheck);
                TopLeftBishop(oldFile, oldRank, newFile, newRank, isCheck);
                BottomLeftBishop(oldFile, oldRank, newFile, newRank, isCheck);
                BottomRightBishop(oldFile, oldRank, newFile, newRank, isCheck);
                return kingCompetitors;
            }
            return pieceLocations;
        }
        private void TopRightBishop(int oldFile, int oldRank, int newFile, int newRank, bool isCheck)
        {
            int tempRank = oldRank;
            int previousFile = oldFile;
            int previousRank = oldRank;
            bool clearMoves = false;
            for (int tempFile = oldFile; tempFile < MAX_UP_DIRECTION; tempFile++)
            {
                if (tempFile >= 0 && tempRank >= 0 && tempFile <= 7 && tempRank <= 7)
                {
                    if (!isCheck)
                    {
                        if (tempFile != oldFile && tempRank != oldRank && tempFile < newFile && tempRank < newRank)
                        {
                            if (chessBoard[tempFile, tempRank] != BoardPiece(ChessTypes.Empty) && tempFile != newFile && tempRank != newRank)
                            {
                                clearMoves = true;
                            }
                        }
                        pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    }
                    else
                    {
                        if (!kingCompetitors.Contains(tempFile.ToString() + tempRank.ToString()) && previousFile != newFile && previousRank != newRank)
                        {
                            if (chessBoard[previousFile, previousRank] == BoardPiece(ChessTypes.Empty))
                            {
                                kingCompetitors.Add(tempFile.ToString() + tempRank.ToString());
                            }
                            else
                            {
                                if (chessBoard[newFile, newRank] == "kd" && (chessBoard[previousFile, previousRank] == "BL" || chessBoard[previousFile, previousRank] == "QL")
                                    || chessBoard[newFile, newRank] == "KL" && (chessBoard[previousFile, previousRank] == "bd" || chessBoard[previousFile, previousRank] == "qd"))
                                {
                                    kingCompetitors.Add(previousFile.ToString() + previousRank.ToString());
                                }
                                tempFile = MAX_DOWN_DIRECTION;
                            }
                        }
                        previousFile = tempFile;
                        previousRank = tempRank;
                    }
                    tempRank++;
                }
            }
            if (clearMoves)
            {
                pieceLocations.Clear();
            }
        }
        private void TopLeftBishop(int oldFile, int oldRank, int newFile, int newRank, bool isCheck)
        {
            int tempRank = oldRank;
            int previousFile = oldFile;
            int previousRank = oldRank;
            bool clearMoves = false;
            for (int tempFile = oldFile; tempFile < MAX_UP_DIRECTION; tempFile++)
            {
                if (tempFile >= 0 && tempRank >= 0 && tempFile <= 7 && tempRank <= 7)
                {
                    if (!isCheck)
                    {
                        if (tempFile != oldFile && tempRank != oldRank && tempFile < newFile && tempRank > newRank)
                        {
                            if (chessBoard[tempFile, tempRank] != BoardPiece(ChessTypes.Empty) && tempFile != newFile && tempRank != newRank)
                            {
                                clearMoves = true;
                            }
                        }
                        pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    }
                    else
                    {
                        if (!kingCompetitors.Contains(tempFile.ToString() + tempRank.ToString()) && previousFile != newFile && previousRank != newRank)
                        {
                            if (chessBoard[previousFile, previousRank] == BoardPiece(ChessTypes.Empty))
                            {
                                kingCompetitors.Add(tempFile.ToString() + tempRank.ToString());
                            }
                            else
                            {
                                if (chessBoard[newFile, newRank] == "kd" && (chessBoard[previousFile, previousRank] == "BL" || chessBoard[previousFile, previousRank] == "QL")
                                    || chessBoard[newFile, newRank] == "KL" && (chessBoard[previousFile, previousRank] == "bd" || chessBoard[previousFile, previousRank] == "qd"))
                                {
                                    kingCompetitors.Add(previousFile.ToString() + previousRank.ToString());
                                }
                                tempFile = MAX_DOWN_DIRECTION;
                            }
                        }
                        previousFile = tempFile;
                        previousRank = tempRank;
                    }
                    tempRank--;
                }
            }
            if (clearMoves)
            {
                pieceLocations.Clear();
            }
        }
        private void BottomLeftBishop(int oldFile, int oldRank, int newFile, int newRank, bool isCheck)
        {
            int tempRank = oldRank;
            int previousFile = oldFile;
            int previousRank = oldRank;
            bool clearMoves = false;
            for (int tempFile = oldFile; tempFile >= MAX_DOWN_DIRECTION; tempFile--)
            {
                if (tempFile >= 0 && tempRank >= 0 && tempFile <= 7 && tempRank <= 7)
                {
                    if (!isCheck)
                    {
                        if (tempFile != oldFile && tempRank != oldRank && tempFile > newFile && tempRank > newRank)
                        {
                            if (chessBoard[tempFile, tempRank] != BoardPiece(ChessTypes.Empty) && tempFile != newFile && tempRank != newRank)
                            {
                                clearMoves = true;
                            }
                        }
                        pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    }
                    else
                    {
                        if (!kingCompetitors.Contains(tempFile.ToString() + tempRank.ToString()) && previousFile != newFile && previousRank != newRank)
                        {
                            if (chessBoard[previousFile, previousRank] == BoardPiece(ChessTypes.Empty))
                            {
                                kingCompetitors.Add(tempFile.ToString() + tempRank.ToString());
                            }
                            else
                            {
                                if (chessBoard[newFile, newRank] == "kd" && (chessBoard[previousFile, previousRank] == "BL" || chessBoard[previousFile, previousRank] == "QL")
                                    || chessBoard[newFile, newRank] == "KL" && (chessBoard[previousFile, previousRank] == "bd" || chessBoard[previousFile, previousRank] == "qd"))
                                {
                                    kingCompetitors.Add(previousFile.ToString() + previousRank.ToString());
                                }
                                tempFile = MAX_DOWN_DIRECTION;
                            }
                        }
                        previousFile = tempFile;
                        previousRank = tempRank;
                    }
                    tempRank--;
                }
            }
            if (clearMoves)
            {
                pieceLocations.Clear();
            }
        }
        private void BottomRightBishop(int oldFile, int oldRank, int newFile, int newRank, bool isCheck)
        {
            int tempRank = oldRank;
            int previousFile = oldFile;
            int previousRank = oldRank;
            bool clearMoves = false;
            for (int tempFile = oldFile; tempFile >= MAX_DOWN_DIRECTION; tempFile--)
            {
                if (tempFile >= 0 && tempRank >= 0 && tempFile <= 7 && tempRank <= 7)
                {
                    if (!isCheck)
                    {
                        if (tempFile != oldFile && tempRank != oldRank && tempFile > newFile && tempRank < newRank)
                        {
                            if (chessBoard[tempFile, tempRank] != BoardPiece(ChessTypes.Empty) && tempFile != newFile && tempRank != newRank)
                            {
                                clearMoves = true;
                            }
                        }
                        pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    }
                    else
                    {
                        if (!kingCompetitors.Contains(tempFile.ToString() + tempRank.ToString()) && previousFile != newFile && previousRank != newRank)
                        {
                            if (chessBoard[previousFile, previousRank] == BoardPiece(ChessTypes.Empty))
                            {
                                kingCompetitors.Add(tempFile.ToString() + tempRank.ToString());
                            }
                            else
                            {
                                if (chessBoard[newFile, newRank] == "kd" && (chessBoard[previousFile, previousRank] == "BL" || chessBoard[previousFile, previousRank] == "QL")
                                    || chessBoard[newFile, newRank] == "KL" && (chessBoard[previousFile, previousRank] == "bd" || chessBoard[previousFile, previousRank] == "qd"))
                                {
                                    kingCompetitors.Add(previousFile.ToString() + previousRank.ToString());
                                }
                                tempFile = MAX_DOWN_DIRECTION;
                            }
                        }
                        previousFile = tempFile;
                        previousRank = tempRank;
                    }
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
            if (oldFile != newFile && oldRank != newRank)
            {
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
            }
            else
            {
                int counter = 0;
                while (counter < 4)
                {
                    tempFile = oldFile;
                    tempRank = oldRank;
                    if (counter == 0)
                    {
                        tempFile += 1; tempRank += 2;
                        kingCompetitors.Add(tempFile.ToString() + tempRank.ToString());
                        tempFile += 1; tempRank -= 1;
                        kingCompetitors.Add(tempFile.ToString() + tempRank.ToString());
                    }
                    else if (counter == 1)
                    {
                        tempFile -= 1; tempRank += 2;
                        kingCompetitors.Add(tempFile.ToString() + tempRank.ToString());
                        tempFile -= 1; tempRank -= 1;
                        kingCompetitors.Add(tempFile.ToString() + tempRank.ToString());
                    }
                    else if (counter == 2)
                    {
                        tempFile -= 1; tempRank -= 2;
                        kingCompetitors.Add(tempFile.ToString() + tempRank.ToString());
                        tempFile -= 1; tempRank += 1;
                        kingCompetitors.Add(tempFile.ToString() + tempRank.ToString());
                    }
                    else if (counter == 3)
                    {
                        tempFile += 1; tempRank -= 2;
                        kingCompetitors.Add(tempFile.ToString() + tempRank.ToString());
                        tempFile += 1; tempRank += 1;
                        kingCompetitors.Add(tempFile.ToString() + tempRank.ToString());
                    }
                    counter++;
                }
                return kingCompetitors;
            }
            return pieceLocations;
        }
        private ArrayList Rook(int oldFile, int oldRank, int newFile, int newRank, bool isCheck)
        {
            if (newFile > oldFile && newRank == oldRank)
            {
                UpRook(oldFile, oldRank, newFile, newRank, isCheck);
            }
            else if (newFile < oldFile && newRank == oldRank)
            {
                DownRook(oldFile, oldRank, newFile, newRank, isCheck);
            }
            else if (newFile == oldFile && newRank < oldRank)
            {
                LeftRook(oldFile, oldRank, newFile, newRank, isCheck);
            }
            else if (newFile == oldFile && newRank > oldRank)
            {
                RightRook(oldFile, oldRank, newFile, newRank, isCheck);
            }
            else if (newFile == oldFile && newRank == oldRank)
            {
                UpRook(oldFile, oldRank, newFile, newRank, isCheck);
                DownRook(oldFile, oldRank, newFile, newRank, isCheck);
                LeftRook(oldFile, oldRank, newFile, newRank, isCheck);
                RightRook(oldFile, oldRank, newFile, newRank, isCheck);

                return kingCompetitors;
            }
            return pieceLocations;
        }
        private void UpRook(int oldFile, int oldRank, int newFile, int newRank, bool isCheck)
        {
            int previousFile = oldFile;
            bool clearMoves = false;

            for (int tempFile = oldFile; tempFile < MAX_UP_DIRECTION; tempFile++)
            {
                if (tempFile >= 0 && tempFile <= 7)
                {
                    if (!isCheck)
                    {
                        if (tempFile != oldFile && tempFile < newFile)
                        {
                            if (chessBoard[tempFile, oldRank] != BoardPiece(ChessTypes.Empty) && tempFile != newFile)
                            {
                                clearMoves = true;
                            }
                        }
                        pieceLocations.Add(tempFile.ToString() + oldRank.ToString());
                    }
                    else
                    {
                        if (!kingCompetitors.Contains(tempFile.ToString() + oldRank.ToString()) && previousFile != newFile)
                        {
                            if (chessBoard[previousFile, oldRank] == BoardPiece(ChessTypes.Empty))
                            {
                                kingCompetitors.Add(tempFile.ToString() + oldRank.ToString());
                            }
                            else
                            {
                                if (chessBoard[newFile, newRank] == "kd" && (chessBoard[previousFile, oldRank] == "RL" || chessBoard[previousFile, oldRank] == "QL")
                                    || chessBoard[newFile, newRank] == "KL" && (chessBoard[previousFile, oldRank] == "rd" || chessBoard[previousFile, oldRank] == "qd"))
                                {
                                    kingCompetitors.Add(previousFile.ToString() + oldRank.ToString());
                                }
                                tempFile = MAX_UP_DIRECTION;
                            }
                        }
                        previousFile = tempFile;
                    }
                }
            }
            if (clearMoves)
            {
                pieceLocations.Clear();
            }
        }
        private void DownRook(int oldFile, int oldRank, int newFile, int newRank, bool isCheck)
        {
            int previousFile = oldFile;
            bool clearMoves = false;

            for (int tempFile = oldFile; tempFile >= MAX_DOWN_DIRECTION; tempFile--)
            {
                if (tempFile >= 0 && tempFile <= 7)
                {
                    if (!isCheck)
                    {
                        if (tempFile != oldFile && tempFile > newFile)
                        {
                            if (chessBoard[tempFile, oldRank] != BoardPiece(ChessTypes.Empty) && tempFile != newFile)
                            {
                                clearMoves = true;
                            }
                        }
                        pieceLocations.Add(tempFile.ToString() + oldRank.ToString());
                    }
                    else
                    {
                        if (!kingCompetitors.Contains(tempFile.ToString() + oldRank.ToString()) && previousFile != newFile)
                        {
                            if (chessBoard[previousFile, oldRank] == BoardPiece(ChessTypes.Empty))
                            {
                                kingCompetitors.Add(tempFile.ToString() + oldRank.ToString());
                            }
                            else
                            {
                                if (chessBoard[newFile, newRank] == "kd" && (chessBoard[previousFile, oldRank] == "RL" || chessBoard[previousFile, oldRank] == "QL")
                                    || chessBoard[newFile, newRank] == "KL" && (chessBoard[previousFile, oldRank] == "rd" || chessBoard[previousFile, oldRank] == "qd"))
                                {
                                    kingCompetitors.Add(previousFile.ToString() + oldRank.ToString());
                                }
                                tempFile = MAX_DOWN_DIRECTION;
                            }
                        }
                        previousFile = tempFile;
                    }
                }
            }
            if (clearMoves)
            {
                pieceLocations.Clear();
            }
        }
        private void LeftRook(int oldFile, int oldRank, int newFile, int newRank, bool isCheck)
        {
            int previousRank = oldRank;
            bool clearMoves = false;

            for (int tempRank = oldRank; tempRank >= MAX_DOWN_DIRECTION; tempRank--)
            {
                if (tempRank >= 0 && tempRank <= 7)
                {
                    if (!isCheck)
                    {
                        if (tempRank != oldRank && tempRank > newRank)
                        {
                            if (chessBoard[oldFile, tempRank] != BoardPiece(ChessTypes.Empty) && tempRank != newRank)
                            {
                                clearMoves = true;
                            }
                        }
                        pieceLocations.Add(oldFile.ToString() + tempRank.ToString());
                    }
                    else
                    {
                        if (!kingCompetitors.Contains(oldFile.ToString() + tempRank.ToString()) && previousRank != newRank)
                        {
                            if (chessBoard[oldFile, previousRank] == BoardPiece(ChessTypes.Empty))
                            {
                                kingCompetitors.Add(oldFile.ToString() + tempRank.ToString());
                            }
                            else
                            {
                                if (chessBoard[newFile, newRank] == "kd" && (chessBoard[oldFile, previousRank] == "RL" || chessBoard[oldFile, previousRank] == "QL")
                                    || chessBoard[newFile, newRank] == "KL" && (chessBoard[oldFile, previousRank] == "rd" || chessBoard[oldFile, previousRank] == "qd"))
                                {
                                    kingCompetitors.Add(oldFile.ToString() + previousRank.ToString());
                                }
                                tempRank = MAX_DOWN_DIRECTION;
                            }
                        }
                        previousRank = tempRank;
                    }
                }
                if (clearMoves)
                {
                    pieceLocations.Clear();
                }
            }
        }
        private void RightRook(int oldFile, int oldRank, int newFile, int newRank, bool isCheck)
        {
            int previousRank = oldRank;
            bool clearMoves = false;

            for (int tempRank = oldRank; tempRank < MAX_UP_DIRECTION; tempRank++)
            {
                if (tempRank >= 0 && tempRank <= 7)
                {
                    if (!isCheck)
                    {
                        if (tempRank != oldRank && tempRank < newRank)
                        {
                            if (chessBoard[oldFile, tempRank] != BoardPiece(ChessTypes.Empty))
                            {
                                clearMoves = true;
                            }
                        }
                        pieceLocations.Add(oldFile.ToString() + tempRank.ToString());
                    }
                    else
                    {
                        if (!kingCompetitors.Contains(oldFile.ToString() + tempRank.ToString()) && previousRank != newRank)
                        {
                            if (chessBoard[oldFile, previousRank] == BoardPiece(ChessTypes.Empty))
                            {
                                kingCompetitors.Add(oldFile.ToString() + tempRank.ToString());
                            }
                            else
                            {
                                if (chessBoard[newFile, newRank] == "kd" && (chessBoard[oldFile, previousRank] == "RL" || chessBoard[oldFile, previousRank] == "QL")
                                    || chessBoard[newFile, newRank] == "KL" && (chessBoard[oldFile, previousRank] == "rd" || chessBoard[oldFile, previousRank] == "qd"))
                                {
                                    kingCompetitors.Add(oldFile.ToString() + previousRank.ToString());
                                }
                                tempRank = MAX_UP_DIRECTION;
                            }
                        }
                        previousRank = tempRank;
                    }
                }
                if (clearMoves)
                {
                    pieceLocations.Clear();
                }
            }
        }
        private ArrayList Pawn(string action, int file, int rank)
        {
            bool canMoveTwo = false;
            int tempFile = file;
            int tempRank = rank;


            if (file == 1 && chessBoard[file, rank] == "PL")
            {
                canMoveTwo = true;
            }
            else if (file == 6 && chessBoard[file, rank] == "pd")
            {
                canMoveTwo = true;
            }


            if (chessBoard[file, rank] == "PL" || chessBoard[file, rank] == "KL")
            {
                if (action == "x")
                {
                    tempFile += 1; tempRank += 1;
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    tempRank -= 2;
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                }
                else if (action == "-")
                {
                    tempFile += 1;
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    if (canMoveTwo)
                    {
                        tempFile += 1;
                        pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    }
                }
                else if (action == "+")
                {
                    tempFile += 1; tempRank += 1;
                    kingCompetitors.Add(tempFile.ToString() + tempRank.ToString());
                    tempRank -= 2;
                    kingCompetitors.Add(tempFile.ToString() + tempRank.ToString());
                }
            }
            else if (chessBoard[file, rank] == "pd" || chessBoard[file, rank] == "kd")
            {
                if (action == "x")
                {
                    tempFile -= 1; tempRank -= 1;
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    tempRank += 2;
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                }
                else if (action == "-")
                {
                    tempFile -= 1;
                    pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    if (canMoveTwo)
                    {
                        tempFile -= 1;
                        pieceLocations.Add(tempFile.ToString() + tempRank.ToString());
                    }
                }
                else if (action == "+")
                {
                    tempFile -= 1; tempRank -= 1;
                    kingCompetitors.Add(tempFile.ToString() + tempRank.ToString());
                    tempRank += 2;
                    kingCompetitors.Add(tempFile.ToString() + tempRank.ToString());
                }
            }
            if (action == "+")
            {
                return kingCompetitors;
            }
            return pieceLocations;
        }
        public void CheckKingLogic(bool isWhite, bool checkNextMove, int file, int rank)
        {
            int enemyPieceFile = 0;
            int enemyPieceRank = 0;
            ChessTypes pieceSwitcher = ChessTypes.King;
            pieceAttckKing.Clear();
            if (isWhite)
            {
                whiteInCheck = false;
                while (pieceSwitcher != ChessTypes.Empty && !whiteInCheck)
                {
                    AddCheckLogic(pieceSwitcher, file, rank);
                    foreach (string s in kingCompetitors)
                    {
                        if (s.Substring(0, 1) == "-" && s.Substring(2, 1) != "-")
                        {
                            enemyPieceFile = int.Parse(s.Substring(0, 2));
                            enemyPieceRank = int.Parse(s.Substring(2, 1));
                        }
                        else if (s.Substring(0, 1) == "-" && s.Substring(2, 1) == "-")
                        {
                            enemyPieceFile = int.Parse(s.Substring(0, 2));
                            enemyPieceRank = int.Parse(s.Substring(2, 2));
                        }
                        else if (s.Substring(1, 1) == "-")
                        {
                            enemyPieceFile = int.Parse(s.Substring(0, 1));
                            enemyPieceRank = int.Parse(s.Substring(1, 2));
                        }
                        else
                        {
                            enemyPieceFile = int.Parse(s.Substring(0, 1));
                            enemyPieceRank = int.Parse(s.Substring(1, 1));
                        }

                        if (enemyPieceFile >= 0 && enemyPieceFile <= 7 && enemyPieceRank >= 0 && enemyPieceRank <= 7)
                        {
                            if (CheckIndividualPiece(pieceSwitcher, isWhite, enemyPieceFile, enemyPieceRank, file, rank))
                            {
                                attackPiece = enemyPieceFile.ToString() + enemyPieceRank.ToString();
                                if (!checkNextMove)
                                {
                                    whiteInCheck = true;
                                }
                                else
                                {
                                    if (pieceSwitcher == ChessTypes.Rook)
                                    {
                                        if (RookSaveAlly(enemyPieceFile, enemyPieceRank, file, rank))
                                        {
                                            whiteInCheck = false;
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                    pieceSwitcher++;
                }
            }
            else
            {
                blackInCheck = false;
                while (pieceSwitcher != ChessTypes.Empty && !blackInCheck)
                {
                    AddCheckLogic(pieceSwitcher, file, rank);
                    foreach (string s in kingCompetitors)
                    {
                        if (s.Substring(0, 1) == "-" && s.Substring(2, 1) != "-")
                        {
                            enemyPieceFile = int.Parse(s.Substring(0, 2));
                            enemyPieceRank = int.Parse(s.Substring(2, 1));
                        }
                        else if (s.Substring(0, 1) == "-" && s.Substring(2, 1) == "-")
                        {
                            enemyPieceFile = int.Parse(s.Substring(0, 2));
                            enemyPieceRank = int.Parse(s.Substring(2, 2));
                        }
                        else if (s.Substring(1, 1) == "-")
                        {
                            enemyPieceFile = int.Parse(s.Substring(0, 1));
                            enemyPieceRank = int.Parse(s.Substring(1, 2));
                        }
                        else
                        {
                            enemyPieceFile = int.Parse(s.Substring(0, 1));
                            enemyPieceRank = int.Parse(s.Substring(1, 1));
                        }
                        if (enemyPieceFile >= 0 && enemyPieceFile <= 7 && enemyPieceRank >= 0 && enemyPieceRank <= 7)
                        {
                            if (CheckIndividualPiece(pieceSwitcher, isWhite, enemyPieceFile, enemyPieceRank, file, rank))
                            {
                                attackPiece = enemyPieceFile.ToString() + enemyPieceRank.ToString();
                                if (!checkNextMove)
                                {
                                    blackInCheck = true;
                                }
                                else
                                {
                                    if (pieceSwitcher == ChessTypes.Rook)
                                    {
                                        if (RookSaveAlly(enemyPieceFile, enemyPieceRank, file, rank))
                                        {
                                            blackInCheck = false;
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                    pieceSwitcher++;
                }
            }
        }

        private void AddCheckLogic(ChessTypes pieceType, int file, int rank)
        {
            kingCompetitors.Clear();
            if (pieceType == ChessTypes.King)
            {
                kingCompetitors = King(file, rank, true);
            }
            else if (pieceType == ChessTypes.Pawn)
            {
                kingCompetitors = Pawn("+", file, rank);
            }
            else if (pieceType == ChessTypes.Rook)
            {
                kingCompetitors = Rook(file, rank, file, rank, true);
            }
            else if (pieceType == ChessTypes.Bishop)
            {
                kingCompetitors = Bishop(file, rank, file, rank, true);
            }
            else if (pieceType == ChessTypes.Knight)
            {
                kingCompetitors = Knight(file, rank, file, rank);
            }
            else if (pieceType == ChessTypes.Queen)
            {
                kingCompetitors = Queen(file, rank, file, rank);
            }
        }
        private bool CheckIndividualPiece(ChessTypes pieceType, bool isWhite, int file, int rank, int kingFile, int kingRank)
        {
            bool inCheck = false;
            if (isWhite)
            {
                if (pieceType == ChessTypes.Pawn && chessBoard[file, rank] == "pd" || pieceType == ChessTypes.Queen && chessBoard[file, rank] == "qd" || pieceType == ChessTypes.Knight && chessBoard[file, rank] == "nd"
                    || pieceType == ChessTypes.Rook && chessBoard[file, rank] == "rd" && (file == kingFile && rank != kingRank || file != kingFile && rank == kingRank)
                     || pieceType == ChessTypes.Bishop && chessBoard[file, rank] == "bd" && (file != kingFile && rank != kingRank))
                {

                    inCheck = true;
                }
                else if (pieceType == ChessTypes.King)
                {
                    if (chessBoard[file, rank] == "qd")
                    {
                        inCheck = true;
                    }
                }
                else
                {
                    inCheck = false;
                }
            }
            else if (!isWhite)
            {
                if (pieceType == ChessTypes.Pawn && chessBoard[file, rank] == "PL" || pieceType == ChessTypes.Queen && chessBoard[file, rank] == "QL" || pieceType == ChessTypes.Knight && chessBoard[file, rank] == "NL"
                    || pieceType == ChessTypes.Rook && chessBoard[file, rank] == "RL" && (file == kingFile && rank != kingRank || file != kingFile && rank == kingRank)
                     || pieceType == ChessTypes.Bishop && chessBoard[file, rank] == "BL" && (file != kingFile && rank != kingRank))
                {
                    inCheck = true;
                }
                else if (pieceType == ChessTypes.King)
                {
                    if (chessBoard[file, rank] == "QL")
                    {
                        inCheck = true;
                    }
                }
                else
                {
                    inCheck = false;
                }
            }

            return inCheck;
        }
        public bool CheckmateLogic(bool isWhite, int file, int rank)
        {
            bool isCheckmate = false;
            ChessTypes pieceType = ChessTypes.King;
            ArrayList tempKingComp = new ArrayList();

            int allySurrounded = 0;
            int tempFile = 0;
            int tempRank = 0;
            int[] parsedString = new int[1];
            string[] parsedLocations = new string[8];

            AddCheckLogic(pieceType, file, rank);
            tempKingComp = kingCompetitors;
            int loopLength = tempKingComp.Count;
            int possibleLocCounter = 0;
            string possibleLoc = "";

            for (int counter = 0; counter < loopLength; counter++)
            {
                parsedLocations[counter] = tempKingComp[counter].ToString();
                possibleLocCounter++;
            }

            for (int i = 0; i < loopLength; i++)
            {
                possibleLoc = parsedLocations[i];
                parsedString = utilities.StringLocParser(possibleLoc);
                tempFile = parsedString[0]; tempRank = parsedString[1];

                if (tempFile >= 0 && tempFile <= 7 && tempRank >= 0 && tempRank <= 7)
                {
                    if (chessBoard[tempFile, tempRank] == BoardPiece(ChessTypes.Empty)
                        || (chessBoard[tempFile, tempRank] == BoardPiece(ChessTypes.Knight, "l") && !isWhite)
                        || (chessBoard[tempFile, tempRank] == BoardPiece(ChessTypes.Knight, "d") && isWhite)
                        || (chessBoard[tempFile, tempRank].Substring(1, 1) != chessBoard[file, rank].Substring(1, 1)))
                    {
                        string previousLoc = chessBoard[tempFile, tempRank];
                        chessBoard[tempFile, tempRank] = chessBoard[file, rank];
                        chessBoard[file, rank] = BoardPiece(ChessTypes.Empty);
                        shadowPieceFile = tempFile;
                        shadowPieceRank = tempRank;

                        CheckKingLogic(isWhite, true, tempFile, tempRank);

                        chessBoard[file, rank] = chessBoard[tempFile, tempRank];
                        chessBoard[tempFile, tempRank] = previousLoc;

                        if (isWhite && whiteInCheck || !isWhite && blackInCheck) { isCheckmate = true; }
                        else { isCheckmate = false; }
                    }
                    else
                    {
                        if (chessBoard[file, rank].Substring(1, 1) == "d" && chessBoard[tempFile, tempRank].Substring(1, 1) == "L"
                            || chessBoard[file, rank].Substring(1, 1) == "L" && chessBoard[tempFile, tempRank].Substring(1, 1) == "d")
                        {
                            isCheckmate = false;
                        }
                        else if (chessBoard[file, rank].Substring(1, 1) == "d" && chessBoard[tempFile, tempRank].Substring(1, 1) == "d"
                            || chessBoard[file, rank].Substring(1, 1) == "L" && chessBoard[tempFile, tempRank].Substring(1, 1) == "L")
                        {
                            allySurrounded++;
                            pieceType = piece.NameSelector(char.ToUpper(chessBoard[tempFile, tempRank].First()).ToString());

                            AddCheckLogic(pieceType, tempFile, tempRank);

                            int enemyFile = 0;
                            int enemyRank = 0;

                            if (attackPiece != "")
                            {
                                foreach (string findingLoc in kingCompetitors)

                                {
                                    parsedString = utilities.StringLocParser(findingLoc);
                                    tempFile = parsedString[0]; tempRank = parsedString[1];
                                    enemyFile = int.Parse(attackPiece.Substring(0, 1));
                                    enemyRank = int.Parse(attackPiece.Substring(1, 1));

                                    if (tempFile == enemyFile && tempRank == enemyRank)
                                    {
                                        cantAttack = false;
                                    }
                                }
                            }
                        }
                        if (allySurrounded == possibleLocCounter && cantAttack)
                        {
                            pieceType = ChessTypes.Knight;

                            AddCheckLogic(pieceType, file, rank);

                            int knightFile = 0;
                            int knightRank = 0;

                            foreach (string location in kingCompetitors)
                            {
                                knightFile = int.Parse(location.Substring(0, 1));
                                knightRank = int.Parse(location.Substring(1, 1));

                                if (knightFile <= 7 && knightFile >= 0 && knightRank <= 7 && knightRank >= 0)
                                {
                                    if ((chessBoard[knightFile, knightRank].Substring(0, 1) == "N" || chessBoard[knightFile, knightRank].Substring(0, 1) == "n")
                                        && chessBoard[knightFile, knightRank].Substring(1, 1) != chessBoard[file, rank].Substring(1, 1))
                                    {
                                        isCheckmate = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (isCheckmate)
            {
                Console.WriteLine("Checkmate bois!");
            }

            return isCheckmate;
        }
        private bool RookSaveAlly(int enemyPieceFile, int enemyPieceRank, int file, int rank)
        {
            if (chessBoard[file, rank].Substring(1,1) == "d")
            {
                if (enemyPieceFile < file)
                {
                    while (enemyPieceFile >= 0)
                    {
                        if (chessBoard[enemyPieceFile, enemyPieceRank] == "rd")
                        {
                            return true;
                        }
                        enemyPieceFile--;
                    }
                }
                else if (enemyPieceFile > file)
                {
                    while (enemyPieceFile <= 7)
                    {
                        if (chessBoard[enemyPieceFile, enemyPieceRank] == "rd")
                        {
                            return true;
                        }
                        enemyPieceFile++;
                    }
                }
                else if (enemyPieceRank < rank)
                {
                    while (enemyPieceRank >= 0)
                    {
                        if (chessBoard[enemyPieceFile, enemyPieceRank] == "rd")
                        {
                            return true;
                        }
                        enemyPieceRank--;
                    }
                }
                else if (enemyPieceRank > file)
                {
                    while (enemyPieceRank <= 7)
                    {
                        if (chessBoard[enemyPieceFile, enemyPieceRank] == "rd")
                        {
                            return true;
                        }
                        enemyPieceRank--;
                    }
                }
                whiteInCheck = true;
            }
            else if (chessBoard[file, rank].Substring(1, 1) == "L")
            {
                if (enemyPieceFile < file)
                {
                    while (enemyPieceFile >= 0)
                    {
                        if (chessBoard[enemyPieceFile, enemyPieceRank] == "RL")
                        {
                            return true;
                        }
                        enemyPieceFile--;
                    }
                }
                else if (enemyPieceFile > file)
                {
                    while (enemyPieceFile <= 7)
                    {
                        if (chessBoard[enemyPieceFile, enemyPieceRank] == "RL")
                        {
                            return true;
                        }
                        enemyPieceFile++;
                    }
                }
                else if (enemyPieceRank < rank)
                {
                    while (enemyPieceRank >= 0)
                    {
                        if (chessBoard[enemyPieceFile, enemyPieceRank] == "RL")
                        {
                            return true;
                        }
                        enemyPieceRank--;
                    }
                }
                else if (enemyPieceRank > file)
                {
                    while (enemyPieceRank <= 7)
                    {
                        if (chessBoard[enemyPieceFile, enemyPieceRank] == "RL")
                        {
                            return true;
                        }
                        enemyPieceRank--;
                    }
                }
                blackInCheck = true;
            }
            Console.WriteLine("Cannot move there, king would be in check!");
            return false;
        }
        private bool BishopSaveally(int enemyPieceFile, int enemyPieceRank, int file, int rank)
        {
            if (chessBoard[file, rank].Substring(1, 1) == "d")
            {
                if (enemyPieceFile < file && enemyPieceRank < rank)
                {
                    while (enemyPieceFile >= 0)
                    {
                        if (chessBoard[enemyPieceFile, enemyPieceRank] == "bd")
                        {
                            return true;
                        }
                        enemyPieceFile--;
                    }
                }
            }
            else if (chessBoard[file, rank].Substring(1, 1) == "L")
            {
                if (enemyPieceFile < file)
                {
                    while (enemyPieceFile >= 0)
                    {
                        if (chessBoard[enemyPieceFile, enemyPieceRank] == "BL")
                        {
                            return true;
                        }
                        enemyPieceFile--;
                    }
                }
            }
            return false;
        }
    }
}
