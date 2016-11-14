using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessFileIO.Models;
using ChessFileIO.Enums;

namespace ChessFileIO.Utilities
{
    public class BoardLayout
    {
        protected static string[,] chessBoard = new string[8, 8];
        private BasicUtilities utilities = new BasicUtilities();
        private PlayerPiece playerPiece = new PlayerPiece();
        private const int charNumConverter = 48;

        public void AddPlacement(string piece, string color, string rank, string file)
        {
            ChessTypes pieceType = playerPiece.NameSelector(piece);
            char charRank = char.Parse(rank);
            charRank -= (char)48;
            string output = BoardPiece(pieceType, color);
            int newFile = Int32.Parse(file);
            int newRank = Int32.Parse(charRank.ToString());
            chessBoard[newFile - 1, newRank - 1] = output;
            Board();
        }
        public bool AddMovement(string piece, string color, string oldRank, string oldFile, string newRank, string newFile, string action)
        {
            GameLogic gameLogic = new GameLogic();
            bool logMove = false;
            char charRawOld = char.Parse(oldRank); charRawOld -= (char)charNumConverter;
            char charRawNew = char.Parse(newRank); charRawNew -= (char)charNumConverter;
            ChessTypes pieceType = playerPiece.NameSelector(piece);
            string output = BoardPiece(pieceType, playerPiece.PieceColor(color));
            int parsedOldFile = Int32.Parse(oldFile);
            int parsedOldRank = Int32.Parse(charRawOld.ToString());
            int parsedNewFile = Int32.Parse(newFile);
            int parsedNewRank = Int32.Parse(charRawNew.ToString());

            if (chessBoard[parsedOldFile - 1, parsedOldRank - 1] != BoardPiece(ChessTypes.Empty))
            {
                if (gameLogic.MovePiece(piece, action, parsedOldFile, parsedOldRank, parsedNewFile, parsedNewRank))
                {
                    logMove = true;
                }
            }
            else
            {
                string incMove = String.Format("[{0,-7}]    Cannot move empty piece, Skipping.", "Error");
                Console.WriteLine(incMove);
            }
            Board();
            return logMove;
        }
        public void BoardInit()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    chessBoard[i, j] = "__";
                }
            }
        }
        public void Board()
        {
            int rank = chessBoard.Length / 8;
            int file = 0;
            char temp = char.Parse(file.ToString());
            temp += (char)17;

            Console.WriteLine(" -----------------------------------------");
            for (int i = 8; i > 0; i--)
            {
                Console.Write(rank-- + "| ");
                for (int j = 0; j < 8; j++)
                {
                    string output = chessBoard[rank, j];
                    Console.Write(output);

                    if (j != 7) { Console.Write(" | "); }
                    else { Console.Write(" |"); }
                }
                Console.WriteLine("\n -----------------------------------------");
            }
            Console.Write(" ");
            for (int k = 0; k < 8; k++)
            {
                Console.Write("  " + (temp++) + "  ");
            }
            Console.Write("\n\n");
        }
        public string BoardPiece(ChessTypes piece, string color = "")
        {
            string output = "";
            if (color != "d" && color != "l")
            {
                piece = ChessTypes.Empty;
            }
            if (piece != ChessTypes.Empty)
            {
                if (piece == ChessTypes.King)
                {
                    output = ChessTypes.King.ToString().First().ToString();
                }
                else if (piece == ChessTypes.Queen)
                {
                    output = ChessTypes.Queen.ToString().First().ToString();
                }
                else if (piece == ChessTypes.Bishop)
                {
                    output = ChessTypes.Bishop.ToString().First().ToString();
                }
                else if (piece == ChessTypes.Pawn)
                {
                    output = ChessTypes.Pawn.ToString().First().ToString();
                }
                else if (piece == ChessTypes.Knight)
                {
                    output = ChessTypes.Knight.ToString();
                    output = output[1].ToString();
                }
                else if (piece == ChessTypes.Rook)
                {
                    output = ChessTypes.Rook.ToString().First().ToString();
                }
                output += color;
                if (output.Contains("d"))
                {
                    output = output.ToLower();
                }
                else if (output.Contains("l"))
                {
                    output = output.ToUpper();
                }

            }
            else
            {
                output = "__";
            }

            return output;
        }
    }
}
