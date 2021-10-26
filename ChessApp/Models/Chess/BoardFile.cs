using ChessApp.Models.Chess.BoardProperties;

namespace ChessApp.Models.Chess
{
    public class BoardFile
    {
        public int BoardID { get; set; }
        public string FileID { get; set; }

        public Board Board { get; set; }
        public File File { get; set; }

        public BoardFile(int boardId, string fileId)
        {
            BoardID = boardId;
            FileID = fileId;
        }

        public BoardFile()
        {

        }
    }
}
