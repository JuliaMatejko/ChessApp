using ChessApp.Models.Chess.BoardProperties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessApp.Models.Chess
{
    public class FieldColumn// : List<Field>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string FieldColumnId { get; set; }

        //public File FieldColumnName { get; set; }
        public List<Field> Fields { get; set; }

        public FieldColumn(List<Field> fields)//, Field fieldColumnName)
        {
            //FieldColumnName = fieldColumnName;
            Fields = fields;
        }

        public FieldColumn()
        {

        }
    }
}
