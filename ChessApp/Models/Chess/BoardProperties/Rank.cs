using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess.BoardProperties
{
    public class Rank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(1)]
        [Column("Rank")]
        public string RankID { get; set; }

        public ICollection<Position> Positions { get; set; }
        public List<BoardRank> BoardsRanks { get; set; }

        public Rank(string rankName)
        {
            RankID = rankName;
        }

        public Rank()
        {

        }
    }
}
