using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessFileIO.Enums;

namespace ChessFileIO.Models
{
    public class PieceLogic : GameLogic
    {
        public bool AppropriatePiece(string piece)
        {
            bool validMove = false;

            if (piece == ChessTypes.King.ToString().Substring(0, 1))
            {
                King();
            }
            else if (piece == ChessTypes.Queen.ToString().Substring(0, 1))
            {
                Queen();
            }
            else if (piece == ChessTypes.Knight.ToString().Substring(1, 1))
            {
                Knight();
            }
            else if (piece == ChessTypes.Bishop.ToString().Substring(0, 1))
            {
                Bishop();
            }
            else if (piece == ChessTypes.Pawn.ToString().Substring(0, 1))
            {
                Pawn();
            }
            else if (piece == ChessTypes.Rook.ToString().Substring(0, 1))
            {
                Rook();
            }
            return validMove;
        }

        private bool King()
        {
            return false;
        }
        private bool Queen()
        {
            return false;
        }
        private bool Bishop()
        {
            return false;
        }
        private bool Knight()
        {
            return false;
        }
        private bool Rook()
        {
            return false;
        }
        private bool Pawn()
        {
            return false;
        }
    }
}
