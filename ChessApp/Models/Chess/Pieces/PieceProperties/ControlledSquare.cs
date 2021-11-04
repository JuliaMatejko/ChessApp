using ChessApp.Models.Chess.BoardProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessApp.Models.Chess.Pieces.PieceProperties
{
    public class ControlledSquare
    {
        public int PieceID { get; set; }
        public int PositionID { get; set; }

        public Piece Piece { get; set; }
        public Position Position { get; set; }

        public ControlledSquare(int pieceId, int positionId)
        {
            PieceID = pieceId;
            PositionID = positionId;
        }

        public ControlledSquare()
        {

        }
    }
}
