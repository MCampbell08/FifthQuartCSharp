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
            ArrayList possibleLocations = pieceLogic.AppropriatePiece(piece, parsedOldFile-1, parsedOldRank-1);

            if (!IsEmpty(parsedOldFile, parsedOldRank))
            {
                validMove = true;
                if (IsEmpty(parsedNewFile, parsedNewRank) && isAttack || !IsEmpty(parsedNewFile, parsedNewRank) && !isAttack)
                {
                    return false;
                }
                else if (!IsEmpty(parsedNewFile, parsedNewRank) && isAttack)
                {

                }
                else if (IsEmpty(parsedNewFile, parsedNewRank) && !isAttack)
                {
                    for (int i = 0; i < possibleLocations.Count; i++)
                    {
                        //if ()
                        //{

                        //}
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
