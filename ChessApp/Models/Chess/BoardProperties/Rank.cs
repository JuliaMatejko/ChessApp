using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess.BoardProperties
{
    public class Rank
    {
        [Key]
        public int RankId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(1)")]
        public string Name { get; set; }

        public Rank(string name)
        {
            Name = name;
        }
    }
}
