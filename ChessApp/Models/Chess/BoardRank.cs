using ChessApp.Models.Chess.BoardProperties;

namespace ChessApp.Models.Chess
{
    public class BoardRank
    {
        public int BoardID { get; set; }
        public string RankID { get; set; }

        public Board Board { get; set; }
        public Rank Rank { get; set; }

        public BoardRank(int boardId, string rankId)
        {
            BoardID = boardId;
            RankID = rankId;
        }

        public BoardRank()
        {

        }
    }
}
