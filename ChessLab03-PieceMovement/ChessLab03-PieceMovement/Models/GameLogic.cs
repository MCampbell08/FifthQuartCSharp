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
        //
        //-------------
        //Return ArrayList of Locations to check for validation.
        //-------------
        //
        PieceLogic pieceLogic = new PieceLogic();
        public bool MovePiece(string piece, string action, int parsedOldFile, int parsedOldRank, int parsedNewFile, int parsedNewRank)
        {
            bool validMove = false;
            bool isAttack = AttackOrMove(action);
            int tempFile = parsedNewFile - 1;
            int tempRank = parsedNewRank - 1;
            string newLocation = tempFile.ToString() + tempRank.ToString();
            string movingPiece = chessBoard[parsedOldFile - 1, parsedOldRank - 1];
            string emptyPiece = chessBoard[parsedNewFile - 1, parsedNewRank - 1];
            ArrayList possibleLocations = pieceLogic.AppropriatePiece(piece, parsedOldFile - 1, parsedOldRank - 1, parsedNewFile - 1, parsedNewRank - 1);

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
                if (!IsEmpty(parsedNewFile, parsedNewRank) && isAttack)
                {
                    if (possibleLocations.Contains(newLocation))
                    {
                        if (char.IsUpper(chessBoard[parsedOldFile - 1, parsedOldRank - 1].First()) && char.IsLower(chessBoard[parsedNewFile - 1, parsedNewRank - 1].First()) 
                            || char.IsLower(chessBoard[parsedOldFile - 1, parsedOldRank - 1].First()) && char.IsUpper(chessBoard[parsedNewFile - 1, parsedNewRank - 1].First()))
                        {
                            validMove = true;
                            chessBoard[parsedNewFile - 1, parsedNewRank - 1] = movingPiece;
                            chessBoard[parsedOldFile - 1, parsedOldRank - 1] = BoardPiece(ChessTypes.Empty);
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
                        validMove = true;
                        chessBoard[parsedNewFile - 1, parsedNewRank - 1] = movingPiece;
                        chessBoard[parsedOldFile - 1, parsedOldRank - 1] = emptyPiece;
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
            else {
                throw new InvalidOperationException("Not a valid action.");
            }
        }
        private bool IsEmpty(int file, int rank)
        {
            if (chessBoard[file - 1, rank - 1] == BoardPiece(ChessTypes.Empty)) { return true; }
            else { return false; }
        }
    }
}
