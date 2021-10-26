﻿using ChessApp.Models.Chess.BoardProperties;
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

        public int BoardID { get; set; }
        [Required]
        public List<BoardFile> BoardsFiles { get; set; }
        [Required]
        public List<BoardRank> BoardsRanks { get; set; }
        [Required]
        public List<BoardPosition> BoardsPositions { get; set; }//
        [Required]
        public List<BoardFieldColumn> BoardsFieldColumns { get; set; } //= AddFieldColumns();


        private static List<FieldColumn> AddFieldColumns() //zamiast starego CreateABoard() works?
        {
            List<FieldColumn> columns = new();/*
            for (int i = 0; i < boardSize; i++)
            {
                columns.Add(new FieldColumn(Files[i], new List<Field>()));

                for (int j = 0; j < boardSize; j++)
                {
                    columns[i].Fields.Add(new Field(new Position(i, j), null));
                }
            }*/
            return columns;
        }
        /*
         public static Board CreateABoard()
         {
             Board board = new Board();

             for (var i = 0; i < boardSize; i++)
             {
                 board.Add(new List<Field>());

                 for (var j = 0; j < boardSize; j++)
                 {
                     board[i].Add(new Field(Files[i], Ranks[j], null));
                 }
             }
             return board;
         }
        */


        public Board()
        {

        }
    }
}
