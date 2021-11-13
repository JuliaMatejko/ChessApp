using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessApp.Models.Chess
{
    public class BoardView
    {
        public static void PrintBoard(Board board)
        {
            Console.WriteLine("");
            Console.WriteLine("   +---------------------------------------+");
            for (var j = 7; j >= 0; j--)
            {
                Console.Write($" {j + 1} |");
                for (var i = 0; i <= 7; i++)
                {
                    if (i != 7)
                    {
                        if (!(board.BoardsFieldColumns[i].FieldColumn.Fields[j].Content == null))
                        {
                            Console.Write($" {board.BoardsFieldColumns[i].FieldColumn.Fields[j].Content.PieceNameID} |");
                        }
                        else
                        {
                            Console.Write("    |");
                        }
                    }
                    else
                    {
                        if (!(board.BoardsFieldColumns[i].FieldColumn.Fields[j].Content == null))
                        {
                            Console.WriteLine($" {board.BoardsFieldColumns[i].FieldColumn.Fields[j].Content.PieceNameID} |");
                        }
                        else
                        {
                            Console.WriteLine("    |");
                        }
                    }
                }
                if (j != 0)
                {
                    Console.WriteLine("   |----+----+----+----+----+----+----+----|");
                }
                else
                {
                    Console.WriteLine("   +---------------------------------------+");
                    Console.WriteLine("     a    b    c    d    e    f    g    h   ");
                }
            }
            Console.WriteLine("");
        }
    }
}
