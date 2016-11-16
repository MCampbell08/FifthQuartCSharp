using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessFileIO.Enums
{
    public enum ChessTypes
    {
        Empty = 0,
        King = 1,
        Queen = 2,
        Rook = 4,
        Bishop = 8,
        Pawn = 16,
        Knight = 32
    }
}
