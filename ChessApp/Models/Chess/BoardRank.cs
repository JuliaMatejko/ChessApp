using ChessApp.Models.Chess.BoardProperties;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess
{
    public class BoardRank
    {
        [ForeignKey("Board")]
        public int GameID { get; set; }
        public string RankID { get; set; }

        public Board Board { get; set; }
        public Rank Rank { get; set; }

        public BoardRank(int gameId, string rankId)
        {
            GameID = gameId;
            RankID = rankId;
        }

        public BoardRank()
        {

        }
    }
}
