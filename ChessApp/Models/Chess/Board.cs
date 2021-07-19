using ChessApp.Models.Chess.BoardProperties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChessApp.Models.Chess
{
    public class Board //: List<FieldColumn>
    {
        public const int BoardSize = 8;

        [Key]
        public int BoardId { get; set; }
        [Required]
        public static List<File> Files { get; } = AddFiles(new string[BoardSize] { "a", "b", "c", "d", "e", "f", "g", "h" });
        [Required]
        public static List<Rank> Ranks { get; } = AddRanks(new string[BoardSize] { "1", "2", "3", "4", "5", "6", "7", "8" });
        [Required]
        public static List<Position> Positions { get; } = AddPositions();
        [Required]
        public List<FieldColumn> FieldColumns { get; } = AddFieldColumns();

        private static List<File> AddFiles(string[] names)
        {
            List<File> list = new List<File>();
            foreach (string name in names)
            {
                list.Add(new File(name));
            }
            return list;
        }

        private static List<Rank> AddRanks(string[] names)
        {
            List<Rank> list = new List<Rank>();
            foreach (string name in names)
            {
                list.Add(new Rank(name));
            }
            return list;
        }

        private static List<Position> AddPositions()
        {
            List<Position> positions = new List<Position>();
            for (var i = 0; i < BoardSize; i++)
            {
                for (var j = 0; j < BoardSize; j++)
                {
                    positions.Add(new Position(Files[i], Ranks[j]));
                }
            }
            return positions;
        }

        private static List<FieldColumn> AddFieldColumns() //zamiast starego CreateABoard() works?
        {
            List<FieldColumn> columns = new List<FieldColumn>();
            for (int i = 0; i < BoardSize; i++)
            {
                columns.Add(new FieldColumn(Files[i], new List<Field>()));

                for (int j = 0; j < BoardSize; j++)
                {
                    columns[i].Fields.Add(new Field(new Position(Files[i], Ranks[j]), null));
                }
            }
            return columns;
        }
        /*
         public static Board CreateABoard()
         {
             Board board = new Board();

             for (var i = 0; i < BoardSize; i++)
             {
                 board.Add(new List<Field>());

                 for (var j = 0; j < BoardSize; j++)
                 {
                     board[i].Add(new Field(Files[i], Ranks[j], null));
                 }
             }
             return board;
         }
        */
    }
}
