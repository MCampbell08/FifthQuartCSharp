using ChessFileIO.Models;
using ChessFileIO.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChessFileIO
{
    public class ChessParser
    {
        private BasicUtilities utilities = new BasicUtilities();
        private PlayerPiece player = new PlayerPiece();
        private BoardLayout board = new BoardLayout();
        private bool initialize = true;
        private bool firstMovement = true;
        private bool validMove = false;

        #region Regex Matches
        private Match placement;
        private Match movement;
        private Match castling;
        private Match subMove;
        private Match subCastling;
        #endregion

        public void RegexChooser(string fileLine)
        {
            placement = Regex.Match(fileLine, @"^\s*([RQKPNB])([ld])([a-h])([1-8])$");
            movement = Regex.Match(fileLine, @"^\s*([RQKNB])?([a-h])([1-8])([-x])([a-h])([1-8])([+#])?\s*([RQKNB])?([a-h])([1-8])([-x])([a-h])([1-8])([+#])?$");
            castling = Regex.Match(fileLine, @"^\s*(O\-O)\s*(O\-O\-O)|\s*(O\-O\-O)\s*(O\-O)$");
            subMove = Regex.Match(fileLine, @"^\s*([RQKNB])?([a-h])([1-8])([-x])([a-h])([1-8])([+#])?\s*$");
            subCastling = Regex.Match(fileLine, @"([O][-][O](-O)?)");

            if (initialize)
            {
                board.BoardInit();
                initialize = false;
            }

            if (placement.Success)
            {
                Placement(placement);
            }
            else if (movement.Success || castling.Success || (subCastling.Success && !castling.Success) || (subMove.Success && !movement.Success))
            {
                Movement(fileLine, movement);
            }
            else
            {
                if (!castling.Success && subCastling.Success)
                {
                    Movement(fileLine, movement);
                }
                else if (!placement.Success && fileLine != "")
                {
                    string failedLine = String.Format("[{0,-7}]  Skipping [{1}]", "Warning", fileLine);
                    Console.WriteLine(failedLine);
                }
            }
            if (validMove)
            {
                board.Board();
            }
        }
        private void Placement(Match placement)
        {
            string finishedPlacement = String.Format("[{0,-7}]  {1} {2} was placed at {3}{4}", utilities.WhitespaceRemover(placement.Groups[0].Value), player.NameSelector(placement.Groups[1].Value), player.PieceColor(placement.Groups[2].Value, true), placement.Groups[3].Value, placement.Groups[4].Value);
            Console.WriteLine(finishedPlacement);
            board.AddPlacement(placement.Groups[1].Value, placement.Groups[2].Value, placement.Groups[3].Value, placement.Groups[4].Value);
            validMove = true;
        }
        private void Movement(string line, Match movement)
        {
            PlayerMovement(line, movement);
        }
        private void PlayerMovement(string line, Match movement)
        {
            int breakPoint = utilities.MoveSeparator(line);
            string firstMove = utilities.WhitespaceRemover(line.Substring(0));
            subMove = Regex.Match(firstMove, @"^\s*([RQKNB])?([a-h])([1-8])([-x])([a-h])([1-8])([+#])?\s*$");
            subCastling = Regex.Match(firstMove, @"([O][-][O](-O)?)");

            if (subMove.Success)
            {
                Console.WriteLine(SepMovement(subMove));
            }
            else if (subCastling.Success)
            {
                Console.WriteLine(SepCastling(firstMove, subCastling));
            }
        }
        private string SepMovement(Match subMove)
        {
            string move = "";
            string color = "";
            if (firstMovement) { color = "White"; }
            else { color = "Black"; }
            if (board.AddMovement(subMove.Groups[1].Value, player.PieceColor(color), subMove.Groups[2].Value, subMove.Groups[3].Value, subMove.Groups[5].Value, subMove.Groups[6].Value, subMove.Groups[4].Value))
            {
                firstMovement = firstMovement ? firstMovement = false : firstMovement = true;
                move = String.Format("[{0,-7}]  {1} moves {2} at {3}{4} to {5}{6}{7}{8}", subMove.Value, color, player.NameSelector(subMove.Groups[1].Value), subMove.Groups[2].Value, subMove.Groups[3].Value, subMove.Groups[5].Value, subMove.Groups[6].Value, player.ActionTaken(subMove.Groups[4].Value), player.ActionTaken(subMove.Groups[7].Value));
                validMove = true;
            }
            else
            {
                validMove = false;
            }
            
            return move;
        }
        private string SepCastling(string line, Match subCast)
        {
            string move = "";
            string color = "";
            string pieceType = "";

            if (line == "O-O-O")
            {
                pieceType = "Q";
            }
            else if (line == "O-O")
            {
                pieceType = "K";
            }
            color = firstMovement ? "White" : "Black";
            move = String.Format("[{0,-7}]  {1} castles {2} side", line, color, player.NameSelector(pieceType));

            return move;
        }
    }
}
