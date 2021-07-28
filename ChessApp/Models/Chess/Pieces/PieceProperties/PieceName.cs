using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess.Pieces.PieceProperties
{
    public class PieceName
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(2)]
        [Column("PieceName")]
        [Display(Name = "Piece Name")]
        public string PieceNameID { get; set; }

        public PieceName(string pieceName)
        {
            PieceNameID = pieceName;
        }

        public PieceName()
        {

        }
    }
}
