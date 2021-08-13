using ChessApp.Models.Chess.Pieces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChessApp.Models.Chess.BoardProperties
{
    public class Position
    {
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

        public Position(string fileid, string rankid)
        {
            FileID = fileid;
            RankID = rankid;
        }

        public Position()
        {

        }
    }
}
