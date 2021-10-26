using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess.BoardProperties
{
    public class File
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(1)]
        [Column("File")]
        public string FileID { get; set; }

        public ICollection<Position> Positions { get; set; }
        public List<BoardFile> BoardsFiles { get; set; }

        public File(string fileName)
        {
            FileID = fileName;
        }

        public File()
        {

        }
    }
}
