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
        public int squaresNumber = 0;

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
            BoardsFiles = AddBoardFiles();
            BoardsRanks = AddBoardRanks();
            BoardsPositions = AddBoardPositions();
            BoardsFieldColumns = AddBoardFieldColumns();
        }

        public Board()
        {

        }

        private List<BoardFieldColumn> AddBoardFieldColumns()
        {
            List<BoardFieldColumn> list = new();
            for (int i = 1; i <= boardSize; i++)
            {
                FieldColumn fieldColumn = new(fieldColumnId: i, squaresNumber: ref squaresNumber);
                list.Add(new BoardFieldColumn(GameID, fieldColumn.FieldColumnID));

            }
            return list;
        }

        private List<BoardPosition> AddBoardPositions()
        {
            List<BoardPosition> list = new();
            int count = 1;
            for (int i = 0; i < files.Length; i++)
            {
                for (int j = 0; j < ranks.Length; j++)
                {
                    Position position = new(positionId: count, fileId: files[i], rankId: ranks[j]);
                    list.Add(new BoardPosition(GameID, position.PositionID));
                    count++;
                }
            }
            return list;
        }

        private List<BoardRank> AddBoardRanks()
        {
            List<BoardRank> list = new();
            foreach (var rank in ranks)
            {
                list.Add(new BoardRank(GameID, rank));
            }
            return list;
        }

        private List<BoardFile> AddBoardFiles()
        {
            List<BoardFile> list = new();
            foreach (var file in files)
            {
                list.Add(new BoardFile(GameID, file));
            }
            return list;
        }
    }
}
