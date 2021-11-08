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

        public FieldColumn(int fieldColumnId, ref int squaresNumber)
        {
            FieldColumnID = fieldColumnId;
            Fields = AddFields(ref squaresNumber);
        }

        private List<Field> AddFields(ref int squaresNumber)
        {
            int initialSquaresNumber = squaresNumber;
            List<Field> list = new();
            for (int i = squaresNumber + 1; i <= Board.boardSize + initialSquaresNumber;)
            {
                list.Add(new Field(fieldId: i, FieldColumnID, positionId: i, contentId: null));
                squaresNumber++;
            }
            return list;
        }

        public FieldColumn()
        {

        }
    }
}
