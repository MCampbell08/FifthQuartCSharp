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
        private ArrayList kingCompetitors = new ArrayList();
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
                locations = Rook(oldFile, oldRank, newFile, newRank, false);
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
            pieceLocations = Rook(oldFile, oldRank, newFile, newRank, false);
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
            int previousFile = 0;
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
                                if (chessBoard[newFile, newRank] == "kd" && chessBoard[previousFile, oldRank] == "RL" || chessBoard[newFile, newRank] == "KL" && chessBoard[previousFile, oldRank] == "rd")
                                {
                                    kingCompetitors.Add(tempFile.ToString() + oldRank.ToString());
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
            int previousRank = 0;
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
        public void CheckKingLogic(bool isWhite, int file, int rank)
        {
            int enemyPieceFile = 0;
            int enemyPieceRank = 0;
            bool doneCheck = false;
            ChessTypes pieceSwitcher = ChessTypes.King;

            if (isWhite)
            {
                while (!doneCheck && pieceSwitcher != ChessTypes.Empty)
                {
                    AddCheckLogic(pieceSwitcher, file, rank);
                    foreach (string s in kingCompetitors)
                    {
                        if (s.Substring(0, 1) == "-")
                        {
                            enemyPieceFile = int.Parse(s.Substring(0, 2));
                            enemyPieceRank = int.Parse(s.Substring(2, 1));
                        }
                        else
                        {
                            enemyPieceFile = int.Parse(s.Substring(0, 1));
                            enemyPieceRank = int.Parse(s.Substring(1, 1));
                        }

                        if (enemyPieceFile >= 0 && enemyPieceFile <= 7 && enemyPieceRank >= 0 && enemyPieceRank <= 7)
                        {
                            if (CheckIndividualPiece(pieceSwitcher, isWhite, enemyPieceFile, enemyPieceRank))
                            {
                                Console.WriteLine("White is in check!");
                            }
                        }
                    }
                    pieceSwitcher++;
                }
            }
            else
            {
                while (!doneCheck && pieceSwitcher != ChessTypes.Empty)
                {
                    AddCheckLogic(pieceSwitcher, file, rank);
                    foreach (string s in kingCompetitors)
                    {
                        if (s.Substring(0, 1) == "-")
                        {
                            enemyPieceFile = int.Parse(s.Substring(0, 2));
                            enemyPieceRank = int.Parse(s.Substring(2, 1));
                        }
                        else
                        {
                            enemyPieceFile = int.Parse(s.Substring(0, 1));
                            enemyPieceRank = int.Parse(s.Substring(1, 1));
                        }
                        if (enemyPieceFile >= 0 && enemyPieceFile <= 7 && enemyPieceRank >= 0 && enemyPieceRank <= 7)
                        {
                            if (CheckIndividualPiece(pieceSwitcher, isWhite, enemyPieceFile, enemyPieceRank))
                            {
                                Console.WriteLine("Black is in check!");
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
            if (pieceType == ChessTypes.Pawn)
            {
                kingCompetitors = Pawn("+", file, rank);
            }
            else if (pieceType == ChessTypes.Rook)
            {
                kingCompetitors = Rook(file, rank, file, rank, true);
            }
            else if (pieceType == ChessTypes.Bishop)
            {

            }
            else if (pieceType == ChessTypes.Knight)
            {
                kingCompetitors = Knight(file, rank, file, rank);
            }
            else if (pieceType == ChessTypes.Queen)
            {

            }
        }
        private bool CheckIndividualPiece(ChessTypes pieceType, bool isWhite, int file, int rank)
        {
            bool inCheck = false;
            if (isWhite)
            {
                if (pieceType == ChessTypes.Pawn && chessBoard[file, rank] == "pd" || pieceType == ChessTypes.Rook && chessBoard[file, rank] == "rd" || pieceType == ChessTypes.Queen && chessBoard[file, rank] == "ql"
                     || pieceType == ChessTypes.Bishop && chessBoard[file, rank] == "bd" || pieceType == ChessTypes.Knight && chessBoard[file, rank] == "nd")
                {
                    inCheck = true;
                }
                else
                {
                    inCheck = false;
                }
            }
            else if (!isWhite)
            {
                if (pieceType == ChessTypes.Pawn && chessBoard[file, rank] == "PL" || pieceType == ChessTypes.Rook && chessBoard[file, rank] == "RL" || pieceType == ChessTypes.Queen && chessBoard[file, rank] == "QL"
                     || pieceType == ChessTypes.Bishop && chessBoard[file, rank] == "BL" || pieceType == ChessTypes.Knight && chessBoard[file, rank] == "NL")
                {
                    inCheck = true;
                }
                else
                {
                    inCheck = false;
                }
            }

            return inCheck;
        }
    }
}
