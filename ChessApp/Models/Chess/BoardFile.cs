using ChessApp.Models.Chess.BoardProperties;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess
{
    public class BoardFile
    {
        [ForeignKey("Board")]
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
