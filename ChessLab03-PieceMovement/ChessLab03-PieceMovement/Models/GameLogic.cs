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
        private PieceLogic pieceLogic = new PieceLogic();
        public bool MovePiece(string piece, string action, int parsedOldFile, int parsedOldRank, int parsedNewFile, int parsedNewRank)
        {
            ArrayList possibleLocations = new ArrayList();
            bool validMove = false;
            bool isAttack = AttackOrMove(action);

            if (!IsEmpty(parsedOldFile, parsedOldRank))
            {
                pieceLogic.AppropriatePiece(piece, parsedOldFile, parsedOldRank);
                validMove = true;
            }
            else
            {
                if (IsEmpty(parsedNewFile, parsedOldRank) && isAttack || !IsEmpty(parsedNewFile, parsedNewRank) && !isAttack)
                {
                    validMove = false;
                }
                else if (IsEmpty(parsedNewFile, parsedOldRank) && !isAttack)
                { 

                    validMove = true;
                }
                else if (!IsEmpty(parsedNewFile, parsedNewRank) && isAttack)
                {
                    validMove = true;
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
