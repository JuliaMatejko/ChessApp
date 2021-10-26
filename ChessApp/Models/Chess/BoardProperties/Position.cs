using ChessApp.Models.Chess.Pieces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess.BoardProperties
{
    public class Position
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PositionID { get; set; }
        [Required]
        [Column("File")]
        public string FileID { get; set; }
        [Required]
        [Column("Rank")]
        public string RankID { get; set; }
        public string Name
        { 
            get { return FileID + RankID; } 
        }

        public File File { get; set; }
        public Rank Rank { get; set; }
        public Piece Piece { get; set; }
        public Field Field { get; set; }
        public HashSet<Piece> NextAvailablePositions { get; set; }
        public HashSet<Piece> ControlledSquares { get; set; }
        public List<BoardPosition> BoardsPositions { get; set; }

        public Position(int positionId, string fileId, string rankId)
        {
            PositionID = positionId;
            FileID = fileId;
            RankID = rankId;
        }

        public Position()
        {

        }
    }
}
