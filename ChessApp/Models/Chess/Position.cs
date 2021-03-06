using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces;
using ChessApp.Models.Chess.Pieces.PieceProperties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess
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
        public ICollection<Field> Fields { get; set; }
        public HashSet<NextAvailablePosition> NextAvailablePositions { get; set; }
        public HashSet<ControlledSquare> ControlledSquares { get; set; }
        public List<BoardPosition> BoardsPositions { get; set; }

        public Position(int positionId, File file, Rank rank)
        {
            PositionID = positionId;
            File = file;
            FileID = file.FileID;
            Rank = rank;
            RankID = rank.RankID;
        }

        public Position()
        {

        }
    }
}
