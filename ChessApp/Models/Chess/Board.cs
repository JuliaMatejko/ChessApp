using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces.PieceProperties;
using System;
using System.Collections.Generic;

namespace ChessApp.Models.Chess
{
    public class Board
    {
        public const int boardSize = 8;
        public static readonly string[] files = new string[boardSize] { "a", "b", "c", "d", "e", "f", "g", "h" };
        public static readonly string[] ranks = new string[boardSize] { "1", "2", "3", "4", "5", "6", "7", "8" };

        public int GameID { get; set; }
        public Game Game { get; set; }

        public List<BoardFile> BoardsFiles { get; set; }
        public List<BoardRank> BoardsRanks { get; set; }
        public List<BoardPosition> BoardsPositions { get; set; }
        public List<BoardFieldColumn> BoardsFieldColumns { get; set; }
        public List<Field> Fields { get; set; }
        public HashSet<NextAvailablePosition> NextAvailablePositions { get; set; }
        public HashSet<ControlledSquare> ControlledSquares { get; set; }

        public Board(int gameId, List<File> files, List<Rank> ranks, List<Position> positions, List<FieldColumn> fieldColumns)
        {
            GameID = gameId;
            BoardsFiles = AddBoardFiles(files);
            BoardsRanks = AddBoardRanks(ranks);
            BoardsPositions = AddBoardPositions(positions);
            BoardsFieldColumns = AddBoardFieldColumns(fieldColumns);
            Fields = AddFields(positions, fieldColumns);
        }

        private List<BoardFieldColumn> AddBoardFieldColumns(List<FieldColumn> fieldColumns)
        {
            List<BoardFieldColumn> boardFieldColumns = new();
            foreach (var fieldColumn in fieldColumns)
            {
                boardFieldColumns.Add(new BoardFieldColumn(GameID, fieldColumn));
            }
            return boardFieldColumns;
        }

        private List<Field> AddFields(List<Position> positions, List<FieldColumn> fieldColumns)
        {
            List<Field> fields = new();
            int count = 0;
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    fields.Add(new Field(GameID, positions[count], fieldColumns[i], contentId: null, pieceGameId: null));
                    count++;
                }
            }
            return fields;
        }

        private List<BoardPosition> AddBoardPositions(List<Position> positions)
        {
            List<BoardPosition> boardsPositions = new();
            foreach (var position in positions)
            {
                boardsPositions.Add(new BoardPosition(GameID, position));
            }
            return boardsPositions;
        }

        private List<BoardRank> AddBoardRanks(List<Rank> ranks)
        {
            List<BoardRank> boardsRanks = new();
            foreach (var rank in ranks)
            {
                boardsRanks.Add(new BoardRank(GameID, rank));
            }
            return boardsRanks;
        }

        private List<BoardFile> AddBoardFiles(List<File> files)
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
