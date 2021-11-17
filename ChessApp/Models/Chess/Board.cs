using ChessApp.Models.Chess.BoardProperties;
using System;
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
            BoardsFiles = AddBoardFiles();
            BoardsRanks = AddBoardRanks();
            BoardsPositions = AddBoardPositions();
            BoardsFieldColumns = AddBoardFieldColumns();
        }

        private List<BoardFieldColumn> AddBoardFieldColumns()
        {
            List<BoardFieldColumn> boardFieldColumns = new();
            for (int i = 0; i < files.Length; i++)
            {
                boardFieldColumns.Add(new BoardFieldColumn(GameID, new FieldColumn(GameID)));
            }
            return boardFieldColumns;
        }

        private List<BoardPosition> AddBoardPositions()
        {
            List<BoardPosition> boardsPositions = new();
            for (int i = 0; i < files.Length; i++)
            {
                for (int j = 0; j < ranks.Length; j++)
                {
                    int positionId = (i * 8) + j + 1;
                    boardsPositions.Add(new BoardPosition(GameID, positionId));
                }
            }
            return boardsPositions;
        }

        private List<BoardRank> AddBoardRanks()
        {
            List<BoardRank> boardsRanks = new();
            foreach (var rank in ranks)
            {
                boardsRanks.Add(new BoardRank(GameID, rank));
            }
            return boardsRanks;
        }

        private List<BoardFile> AddBoardFiles()
        {
            List<BoardFile> boardsFiles = new();
            foreach (var file in files)
            {
                boardsFiles.Add(new BoardFile(GameID, file));
            }
            return boardsFiles;
        }

        public Board()
        {

        }
    }
}
