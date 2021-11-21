using System.Collections.Generic;
using System.Linq;
using ChessApp.Models.Chess;
using ChessApp.Models.Chess.Pieces;
using ChessApp.Models.Chess.Pieces.PieceProperties;

namespace ChessApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Games.Any())
            {
                return;
            }

            //Add Files
            var files = new List<File>();
            for (int i = 0; i < Board.files.Length; i++)
            {
                files.Add(new File(Board.files[i]));
            }
            foreach (File f in files)
            {
                context.Files.Add(f);
            }
            context.SaveChanges();

            //Add Ranks
            var ranks = new List<Rank>();
            for (int i = 0; i < Board.ranks.Length; i++)
            {
                ranks.Add(new Rank(Board.ranks[i]));
            }
            foreach (Rank r in ranks)
            {
                context.Ranks.Add(r);
            }
            context.SaveChanges();

            //Add Positions
            var positions = new List<Position>();
            int count = 0;
            for (int i = 0; i < Board.files.Length; i++)
            {
                for (int j = 0; j < Board.ranks.Length; j++)
                {
                    positions.Add(new Position(count + 1, files[i], ranks[j]));
                    count++;
                }
            }
            foreach (Position p in positions)
            {
                context.Positions.Add(p);
            }
            context.SaveChanges();

            //Add FieldColumns
            var fieldColumns = new List<FieldColumn>();
            for (int i = 0; i < Board.files.Length; i++)
            {
                fieldColumns.Add(new FieldColumn(i + 1));
            }
            foreach (FieldColumn fc in fieldColumns)
            {
                context.FieldColumns.Add(fc);
            }
            context.SaveChanges();

            //Add PieceNames
            var pieceNames = new List<PieceName>();
            for (int i = 0; i < Piece.pieceNames.Length; i++)
            {
                pieceNames.Add(new PieceName(Piece.pieceNames[i]));
            }
            foreach (PieceName pn in pieceNames)
            {
                context.PieceNames.Add(pn);
            }
            context.SaveChanges();

            //Add Game
            var game = new Game(files, ranks, positions, fieldColumns);
            context.Games.Add(game);
            context.SaveChanges();
  
            game.StartGame();
        }
    }
}
