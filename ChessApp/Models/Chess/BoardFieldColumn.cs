using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess
{
    public class BoardFieldColumn
    {
        [ForeignKey("Board")]
        public int GameID { get; set; }
        public int FieldColumnID { get; set; }

        public Board Board { get; set; }
        public FieldColumn FieldColumn { get; set; }

        public BoardFieldColumn(int gameId, FieldColumn fieldColumn)
        {
            GameID = gameId;
            FieldColumn = fieldColumn;
            FieldColumnID = fieldColumn.FieldColumnID;
        }

        public BoardFieldColumn()
        {

        }
    }
}
