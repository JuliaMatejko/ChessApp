using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess.BoardProperties
{
    public class File
    {
        [Key]
        public int FileId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(1)")]
        public string Name { get; set; }

        public File(string name)
        {
            Name = name;
        }
    }
}
