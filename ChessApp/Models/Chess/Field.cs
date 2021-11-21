using ChessApp.Models.Chess.Pieces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess
{
    public class Field
    {
        [ForeignKey("Board")]
        public int GameID { get; set; }
        public Board Board { get; set; }
        [ForeignKey("Position")]
        public int PositionID { get; set; }
        public Position Position { get; set; }
        public int? PieceGameID { get; set; }
        [Column("ContentID")]
        public int? PieceID { get; set; }
        [ForeignKey("PieceGameID, PieceID")]
        public Piece Content { get; set; }

        public int FieldColumnID { get; set; }
        public FieldColumn FieldColumn { get; set; }

        public Field(int gameId, Position position, FieldColumn fieldColumn, int? contentId, int? pieceGameId)
        {
            GameID = gameId;
            Position = position;
            PositionID = position.PositionID;
            FieldColumn = fieldColumn;
            FieldColumnID = fieldColumn.FieldColumnID;
            PieceID = contentId;
            PieceGameID = pieceGameId;
        }

        public Field()
        {

        }
    }
}