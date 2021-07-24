using System.Linq;
using ChessApp.Models.Chess;
using ChessApp.Models.Chess.BoardProperties;

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
                files[i] = new File(i + 1, Board.files[i]);
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
                ranks[i] = new Rank(i + 1, Board.ranks[i]);
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
                    positions[count] = new Position(count + 1, i + 1, j + 1);
                    count++;
                }
            }
            foreach (Position p in positions)
            {
                context.Positions.Add(p);
            }
            context.SaveChanges();
        }
    }
}
