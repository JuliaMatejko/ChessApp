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

        public Board(int gameId, File[] files, Rank[]ranks, Position[] positions, Field[] fields, FieldColumn[] fieldColumns)
        {
            GameID = gameId;
            BoardsFiles = AddBoardFiles(files);
            BoardsRanks = AddBoardRanks(ranks);
            BoardsPositions = AddBoardPositions(positions);
            BoardsFieldColumns = AddBoardFieldColumns(fieldColumns, fields);
        }

        private List<BoardFieldColumn> AddBoardFieldColumns(FieldColumn[] fieldColumns, Field[] fields)
        {
            List<BoardFieldColumn> boardFieldColumns = new();
            foreach (var fieldColumn in fieldColumns)
            {
                boardFieldColumns.Add(new BoardFieldColumn(GameID, fieldColumn));
            }
            return boardFieldColumns;
        }

        private List<BoardPosition> AddBoardPositions(Position[] positions)
        {
            List<BoardPosition> boardsPositions = new();
            foreach (var position in positions)
            {
                boardsPositions.Add(new BoardPosition(GameID, position));
            }
            return boardsPositions;
        }

        private List<BoardRank> AddBoardRanks(Rank[] ranks)
        {
            List<BoardRank> boardsRanks = new();
            foreach (var rank in ranks)
            {
                boardsRanks.Add(new BoardRank(GameID, rank));
            }
            return boardsRanks;
        }

        private List<BoardFile> AddBoardFiles(File[] files)
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
