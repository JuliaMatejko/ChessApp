using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess.BoardProperties
{
    public class BoardRank
    {
        [ForeignKey("Board")]
        public int GameID { get; set; }
        public string RankID { get; set; }

        public Board Board { get; set; }
        public Rank Rank { get; set; }

        public BoardRank(int gameId, Rank rank)
        {
            GameID = gameId;
            Rank = rank;
            RankID = rank.RankID;
        }

        public BoardRank()
        {

        }
    }
}
