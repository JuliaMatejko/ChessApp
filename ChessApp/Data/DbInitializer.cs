﻿using System.Linq;
using ChessApp.Models.Chess;
using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces;
using ChessApp.Models.Chess.Pieces.PieceProperties;

namespace ChessApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Files.Any())
            {
                return;
            }

            //Add Files
            var files = new File[Board.boardSize];
            for (int i = 0; i < Board.files.Length; i++)
            {
                files[i] = new File(Board.files[i]);
            }
            foreach (File f in files)
            {
                context.Files.Add(f);
            }
            context.SaveChanges();

            //Add Ranks
            var ranks = new Rank[Board.boardSize];
            for (int i = 0; i < Board.ranks.Length; i++)
            {
                ranks[i] = new Rank(Board.ranks[i]);
            }
            foreach (Rank r in ranks)
            {
                context.Ranks.Add(r);
            }
            context.SaveChanges();
            
            //Add Positions
            var positions = new Position[Board.boardSize * Board.boardSize];
            int count = 0;
            for (int i = 0; i < Board.files.Length; i++)
            {
                for (int j = 0; j < Board.ranks.Length; j++)
                {
                    positions[count] = new Position(count + 1, Board.files[i], Board.ranks[j]);
                    count++;
                }
            }
            foreach (Position p in positions)
            {
                context.Positions.Add(p);
            }
            context.SaveChanges();

            //Add PieceNames
            var pieceNames = new PieceName[Piece.pieceNames.Length];
            for (int i = 0; i < Piece.pieceNames.Length; i++)
            {
                pieceNames[i] = new PieceName(Piece.pieceNames[i]);
            }
            foreach (PieceName pn in pieceNames)
            {
                context.PieceNames.Add(pn);
            }
            context.SaveChanges();

            //Add FieldColumns
            var fieldColumns = new FieldColumn[Board.boardSize];
            for (int i = 0; i < Board.boardSize; i++)
            {
                fieldColumns[i] = new FieldColumn(i + 1);
            }
            foreach (FieldColumn fc in fieldColumns)
            {
                context.FieldColumns.Add(fc);
            }
            context.SaveChanges();

            //Add Pawns
            var pawns = new Pawn[Board.boardSize * 2];
            for (var i = 0; i < Board.boardSize; i++)
            {
                pawns[i] = new Pawn(i + 1, true, positions[(i * 8) + 1]);  // set white pawns
            }
            var k = 0;
            for (var i = Board.boardSize; i < Board.boardSize * 2; i++)
            {
                pawns[i] = new Pawn(i + 1, false, positions[(k * 8) + 6]);  // set black pawns 
                k++;
            }
            foreach (Pawn p in pawns)
            {
                context.Pieces.Add(p);
            }
            context.SaveChanges();

            //Add Rooks
            //Add Bishops
            //Add Knights
            //Add Queens
            //Add Kings

            //Add Fields
            var fields = new Field[positions.Length];
            count = 0;
            for (int i = 0; i < Board.boardSize; i++)
            {
                for (int j = 0; j < Board.boardSize; j++)
                {
                    fields[count] = new Field(count + 1, fieldColumns[i].FieldColumnID, positions[count].PositionID, contentId: null);
                    count++;
                }
            }
            foreach (Field f in fields)
            {
                context.Fields.Add(f);
            }
            context.SaveChanges();

            //Add Board


            


        }
    }
}
