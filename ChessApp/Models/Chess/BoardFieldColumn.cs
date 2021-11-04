namespace ChessApp.Models.Chess
{
    public class BoardFieldColumn
    {
        public int GameID { get; set; }
        public int FieldColumnID { get; set; }

        public Board Board { get; set; }
        public FieldColumn FieldColumn { get; set; }

        public BoardFieldColumn(int gameId, int fieldColumnId)
        {
            GameID = gameId;
            FieldColumnID = fieldColumnId;
        }

        public BoardFieldColumn()
        {

        }
    }
}
