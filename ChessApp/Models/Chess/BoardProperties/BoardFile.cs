using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess.BoardProperties
{
    public class BoardFile
    {
        [ForeignKey("Board")]
        public int GameID { get; set; }
        public string FileID { get; set; }

        public Board Board { get; set; }
        public File File { get; set; }

        public BoardFile(int gameId, File file)
        {
            GameID = gameId;
            File = file;
            FileID = file.FileID;
        }

        public BoardFile()
        {

        }
    }
}
