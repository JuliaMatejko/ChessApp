using ChessApp.Models.Chess.BoardProperties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChessApp.Models.Chess
{
    public class Board
    {
        public const int boardSize = 8;
        public static readonly string[] files = new string[boardSize] { "a", "b", "c", "d", "e", "f", "g", "h" };
        public static readonly string[] ranks = new string[boardSize] { "1", "2", "3", "4", "5", "6", "7", "8" };

        [Key]
        public int GameID { get; set; }
        public Game Game { get; set; }

        [Required]
        public List<BoardFile> BoardsFiles { get; set; }
        [Required]
        public List<BoardRank> BoardsRanks { get; set; }
        [Required]
        public List<BoardPosition> BoardsPositions { get; set; }
        [Required]
        public List<BoardFieldColumn> BoardsFieldColumns { get; set; }

        public Board(int gameId)
        {
            GameID = gameId;
        }

        public Board()
        {

        }
    }
}
