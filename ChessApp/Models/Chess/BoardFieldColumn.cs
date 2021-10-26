namespace ChessApp.Models.Chess
{
    public class BoardFieldColumn
    {
        public int BoardID { get; set; }
        public int FieldColumnID { get; set; }

        public Board Board { get; set; }
        public FieldColumn FieldColumn { get; set; }

        public BoardFieldColumn(int boardId, int fieldColumnId)
        {
            BoardID = boardId;
            FieldColumnID = fieldColumnId;
        }

        public BoardFieldColumn()
        {

        }
    }
}
