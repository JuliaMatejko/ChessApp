using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChessApp.Models.Chess.BoardProperties
{
    public class Position
    {
        [Key]
        public int PositionId { get; set; }
        [Required]
        public File File { get; set; }
        [Required]
        public Rank Rank { get; set; }
        [Required]
        public string Name { get; set; }

        public Position(File file, Rank rank)
        {
            File = file;
            Rank = rank;
            Name = File.Name + Rank.Name;
        }

        public Position()
        {

        }
    }
}
