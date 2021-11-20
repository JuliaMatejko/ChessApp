using ChessApp.Models.Chess.BoardProperties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess
{
    public class FieldColumn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FieldColumnID { get; set; }

        public List<Field> Fields { get; set; }
        public List<BoardFieldColumn> BoardsFieldColumns { get; set; }

        public FieldColumn(int fieldColumnId)
        {
            FieldColumnID = fieldColumnId;
        }

        public FieldColumn()
        {

        }
    }
}
