using ChessApp.Models.Chess.BoardProperties;

namespace ChessApp.Models.Chess
{
    public class BoardFile
    {
        public int GameID { get; set; }
        public string FileID { get; set; }

        public Board Board { get; set; }
        public File File { get; set; }

        public BoardFile(int gameId, string fileId)
        {
            GameID = gameId;
            FileID = fileId;
        }

        public BoardFile()
        {

        }
    }
}
