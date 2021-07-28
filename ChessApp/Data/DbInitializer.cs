using System.Linq;
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
                    positions[count] = new Position(Board.files[i], Board.ranks[j]);
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
        }
    }
}
