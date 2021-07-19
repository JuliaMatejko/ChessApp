using ChessApp.Models.Chess.BoardProperties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChessApp.Models.Chess
{
    public class FieldColumn// : List<Field>
    {
        [Key]
        public int FieldColumnId { get; set; }
        [Required]
        public File File { get; set; }
        [Required]
        public List<Field> Fields { get; set; }

        public FieldColumn(File file, List<Field> fields)
        {
            File = file;
            Fields = fields;
        }

        public FieldColumn()
        {

        }
    }
}
