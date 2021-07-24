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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PositionID { get; set; }
        //public string Name { get; set; }

        public int FileID { get; set; }
        public int RankID { get; set; }
        public File File { get; set; }
        public Rank Rank { get; set; }
        /*
        public Position(File file, Rank rank)
        {
            File = file;
            Rank = rank;
            Name = File.Name + Rank.Name;
        }
        */
        public Position(int positionid, int fileid, int rankid)
        {
            PositionID = positionid;
            FileID = fileid;
            RankID = rankid;
        }

        public Position()
        {

        }
    }
}
