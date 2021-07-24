using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess.BoardProperties
{
    public class Rank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RankID { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(1)")]
        public string Name { get; set; }

        public ICollection<Position> Positions { get; set; }

        public Rank(int rankid, string name)
        {
            RankID = rankid;
            Name = name;
        }

        public Rank()
        {

        }
    }
}
