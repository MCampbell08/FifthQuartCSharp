using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessFileIO.Utilities;
using ChessFileIO.Enums;
using System.Collections;

namespace ChessFileIO.Models
{
    public class GameLogic : BoardLayout
    {
        PieceLogic pieceLogic = new PieceLogic();
        static char previousPiece = ' ';
        static char currentPiece = ' ';
        public bool MovePiece(string piece, string action, int parsedOldFile, int parsedOldRank, int parsedNewFile, int parsedNewRank)
        {
            bool validMove = false;
            bool isAttack = AttackOrMove(action);
            int tempFile = parsedNewFile - 1;
            int tempRank = parsedNewRank - 1;
            string newLocation = tempFile.ToString() + tempRank.ToString();
            string movingPiece = chessBoard[parsedOldFile - 1, parsedOldRank - 1];
            string emptyPiece = chessBoard[parsedNewFile - 1, parsedNewRank - 1];
            ArrayList possibleLocations = pieceLogic.AppropriatePiece(piece, action, parsedOldFile - 1, parsedOldRank - 1, parsedNewFile - 1, parsedNewRank - 1);

            if (!IsEmpty(parsedOldFile, parsedOldRank))
            {
                if (IsEmpty(parsedNewFile, parsedNewRank) && isAttack)
                {
                    string incMove = String.Format("[{0,-7}]    Cannot attack empty space.", "Error");
                    Console.WriteLine(incMove);
                    validMove = false;
                }
                else if (!IsEmpty(parsedNewFile, parsedNewRank) && !isAttack)
                {
                    string incMove = String.Format("[{0,-7}]    Cannot move to taken space.", "Error");
                    Console.WriteLine(incMove);
                    validMove = false;
                }
                if (allMoves.Count == 0)
                {
                    allMoves.Add(" ");
                }
                currentPiece = chessBoard[parsedOldFile - 1, parsedOldRank - 1].First();
                if (!IsEmpty(parsedNewFile, parsedNewRank) && isAttack)
                {
                    if (possibleLocations.Contains(newLocation))
                    {
                        if (char.IsUpper(previousPiece) && char.IsLower(currentPiece) || char.IsLower(previousPiece) && char.IsUpper(currentPiece))
                        {
                            validMove = true;
                            chessBoard[parsedNewFile - 1, parsedNewRank - 1] = movingPiece;
                            chessBoard[parsedOldFile - 1, parsedOldRank - 1] = BoardPiece(ChessTypes.Empty);
                            allMoves.Add(chessBoard[parsedNewFile - 1, parsedNewRank - 1]);
                            previousPiece = currentPiece;
                        }
                        else
                        {
                            string incMove = String.Format("[{0,-7}]    Incorrect player trying to capture.", "Error");
                            Console.WriteLine(incMove);
                            validMove = false;
                        }
                    }
                    else
                    {
                        string incMove = String.Format("[{0,-7}]    This is not a possible location to capture.", "Error");
                        Console.WriteLine(incMove);
                        validMove = false;
                    }
                }
                else if (IsEmpty(parsedNewFile, parsedNewRank) && !isAttack)
                {
                    if (possibleLocations.Contains(newLocation))
                    {
                        if (char.IsUpper(previousPiece) && char.IsLower(currentPiece) || char.IsLower(previousPiece) && char.IsUpper(currentPiece) || previousPiece == ' ')
                        {
                            validMove = true;
                            chessBoard[parsedNewFile - 1, parsedNewRank - 1] = movingPiece;
                            chessBoard[parsedOldFile - 1, parsedOldRank - 1] = emptyPiece;
                            allMoves.Add(chessBoard[parsedNewFile - 1, parsedNewRank - 1]);
                            previousPiece = currentPiece;
                            CheckKing(currentPiece);
                        }
                        else
                        {
                            string incMove = String.Format("[{0,-7}]    Incorrect player trying to move.", "Error");
                            Console.WriteLine(incMove);
                            validMove = false;
                        }
                    }
                    else
                    {
                        string incMove = String.Format("[{0,-7}]    This is not a possible location to move to.", "Error");
                        Console.WriteLine(incMove);
                        validMove = false;
                    }
                }
            }

            return validMove;
        }

        private bool AttackOrMove(string action)
        {
            if (action == "x")
            {
                return true;
            }
            else if (action == "-")
            {
                return false;
            }
            else
            {
                throw new InvalidOperationException("Not a valid action.");
            }
        }
        private bool IsEmpty(int file, int rank)
        {
            if (chessBoard[file - 1, rank - 1] == BoardPiece(ChessTypes.Empty)) { return true; }
            else { return false; }
        }
        private void CheckKing(char piece)
        {
            string boardPiece = " ";
            int kingFile = 0;
            int kingRank = 0;
            bool checkWhiteK = char.IsLower(piece);

            for (int i = 0; i < Math.Sqrt(chessBoard.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(chessBoard.Length); j++)
                {
                    boardPiece = chessBoard[i, j];
                    if (checkWhiteK)
                    {
                        if (boardPiece == "KL")
                        {
                            kingFile = i;
                            kingRank = j;
                        }
                    }
                    else if (!checkWhiteK)
                    {
                        if (boardPiece == "kd")
                        {
                            kingFile = i;
                            kingRank = j; 
                        }
                    }
                }
            }
            pieceLogic.CheckKingLogic(checkWhiteK, kingFile, kingRank);
        }
    }
}
