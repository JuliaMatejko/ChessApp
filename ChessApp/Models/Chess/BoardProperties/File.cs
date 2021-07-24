using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess.BoardProperties
{
    public class File
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FileID { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(1)")]
        public string Name { get; set; }

        public ICollection<Position> Positions { get; set; }

        public File(int fileid, string name)
        {
            FileID = fileid;
            Name = name;
        }
        public File()
        {

        }
    }
}
