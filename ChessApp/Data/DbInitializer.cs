using System.Linq;
using ChessApp.Models.Chess;
using ChessApp.Models.Chess.BoardProperties;
using ChessApp.Models.Chess.Pieces;
using ChessApp.Models.Chess.Pieces.PieceProperties;

namespace ChessApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Files.Any())
            {
                return;
            }

            var game = new Game();
            context.Games.Add(game);
            context.SaveChanges();

            var gameState = new GameState(game.GameID);
            context.GameStates.Add(gameState);
            context.SaveChanges();

            //Add Files
            var files = new File[Board.boardSize];
            for (int i = 0; i < Board.files.Length; i++)
            {
                files[i] = new File(Board.files[i]);
            }
            foreach (File f in files)
            {
                context.Files.Add(f);
            }
            context.SaveChanges();

            //Add Ranks
            var ranks = new Rank[Board.boardSize];
            for (int i = 0; i < Board.ranks.Length; i++)
            {
                ranks[i] = new Rank(Board.ranks[i]);
            }
            foreach (Rank r in ranks)
            {
                context.Ranks.Add(r);
            }
            context.SaveChanges();
            
            //Add Positions
            var positions = new Position[Board.boardSize * Board.boardSize];
            int count = 0;
            for (int i = 0; i < Board.files.Length; i++)
            {
                for (int j = 0; j < Board.ranks.Length; j++)
                {
                    positions[count] = new Position(count + 1, Board.files[i], Board.ranks[j]);
                    count++;
                }
            }
            foreach (Position p in positions)
            {
                context.Positions.Add(p);
            }
            context.SaveChanges();

            //Add FieldColumns
            var fieldColumns = new FieldColumn[Board.boardSize];
            for (int i = 0; i < Board.boardSize; i++)
            {
                fieldColumns[i] = new FieldColumn(i + 1);
            }
            foreach (FieldColumn fc in fieldColumns)
            {
                context.FieldColumns.Add(fc);
            }
            context.SaveChanges();

            //Add Board
            var board = new Board(game.GameID);
            context.Boards.Add(board);
            context.SaveChanges();

            //Add BoardsFiles
            var boardsFiles = new BoardFile[Board.boardSize];
            for (int i = 0; i < Board.files.Length; i++)
            {
                boardsFiles[i] = new BoardFile(board.GameID, files[i].FileID);
            }
            foreach (BoardFile bf in boardsFiles)
            {
                context.BoardsFiles.Add(bf);
            }
            context.SaveChanges();

            //Add BoardsRanks
            var boardsRanks = new BoardRank[Board.boardSize];
            for (int i = 0; i < Board.ranks.Length; i++)
            {
                boardsRanks[i] = new BoardRank(board.GameID, ranks[i].RankID);
            }
            foreach (BoardRank br in boardsRanks)
            {
                context.BoardsRanks.Add(br);
            }
            context.SaveChanges();

            //Add BoardsPositions
            var boardsPositions = new BoardPosition[Board.boardSize * Board.boardSize];
            for (int i = 0; i < Board.boardSize * Board.boardSize; i++)
            {
                boardsPositions[i] = new BoardPosition(board.GameID, positions[i].PositionID);
            }
            foreach (BoardPosition bp in boardsPositions)
            {
                context.BoardsPositions.Add(bp);
            }
            context.SaveChanges();

            //Add BoardsFieldColumns
            var boardsFieldColumns = new BoardFieldColumn[Board.boardSize];
            for (int i = 0; i < Board.boardSize; i++)
            {
                boardsFieldColumns[i] = new BoardFieldColumn(board.GameID, fieldColumns[i].FieldColumnID);
            }
            foreach (BoardFieldColumn bfc in boardsFieldColumns)
            {
                context.BoardsFieldColumns.Add(bfc);
            }
            context.SaveChanges();

            //Add PieceNames
            var pieceNames = new PieceName[Piece.pieceNames.Length];
            for (int i = 0; i < Piece.pieceNames.Length; i++)
            {
                pieceNames[i] = new PieceName(Piece.pieceNames[i]);
            }
            foreach (PieceName pn in pieceNames)
            {
                context.PieceNames.Add(pn);
            }
            context.SaveChanges();

            //Add Pawns
            var pawns = new Pawn[Board.boardSize * 2];
            for (var i = 0; i < Board.boardSize; i++)
            {
                pawns[i] = new Pawn(i + 1, true, positions[(i * 8) + 1]);  // set white pawns
            }
            var k = 0;
            for (var i = Board.boardSize; i < Board.boardSize * 2; i++)
            {
                pawns[i] = new Pawn(i + 1, false, positions[(k * 8) + 6]);  // set black pawns 
                k++;
            }
            foreach (Pawn p in pawns)
            {
                context.Pieces.Add(p);
            }
            context.SaveChanges();

            //Add Rooks
            var rooks = new Rook[]
            {
                new Rook(17, true, positions[0]),
                new Rook(18, true, positions[56]),
                new Rook(19, false, positions[7]),
                new Rook(20, false, positions[63])
            };
            foreach (Rook r in rooks)
            {
                context.Pieces.Add(r);
            }
            context.SaveChanges();

            //Add Knights
            var knights = new Knight[]
            {
                new Knight(21, true, positions[8]),
                new Knight(22, true, positions[48]),
                new Knight(23, false, positions[15]),
                new Knight(24, false, positions[55])
            };
            foreach (Knight kn in knights)
            {
                context.Pieces.Add(kn);
            }
            context.SaveChanges();

            //Add Bishops
            var bishops = new Bishop[]
            {
                new Bishop(25, true, positions[16]),
                new Bishop(26, true, positions[40]),
                new Bishop(27, false, positions[23]),
                new Bishop(28, false, positions[47])
            };
            foreach (Bishop b in bishops)
            {
                context.Pieces.Add(b);
            }
            context.SaveChanges();

            //Add Queens
            var queens = new Queen[]
            {
                new Queen(29, true, positions[24]),
                new Queen(30, false, positions[31])
            };
            foreach (Queen q in queens)
            {
                context.Pieces.Add(q);
            }
            context.SaveChanges();

            //Add Kings
            var kings = new King[]
            {
                new King(31, true, positions[32]),
                new King(32, false, positions[39])
            };
            foreach (King kg in kings)
            {
                context.Pieces.Add(kg);
            }
            context.SaveChanges();

            //Add Fields
            var fields = new Field[positions.Length];
            count = 0;
            for (int i = 0; i < Board.boardSize; i++)
            {
                for (int j = 0; j < Board.boardSize; j++)
                {
                    fields[count] = new Field(count + 1, fieldColumns[i].FieldColumnID, positions[count].PositionID, contentId: null);
                    count++;
                }
            }
            //Set field content
            ////Set pawns
            //////white
            for (int i = 0; i < Board.boardSize; i++)
            {
                fields[(i * 8) + 1].PieceID = pawns[i].PieceID;
            }
            //////black
            count = 0;
            for (int i = Board.boardSize; i < Board.boardSize * 2; i++)
            {
                fields[(count * 8) + 6].PieceID = pawns[i].PieceID;
                count++;
            }
            ////Set rooks
            //////white
            fields[0].PieceID = rooks[0].PieceID;
            fields[56].PieceID = rooks[1].PieceID;
            //////black
            fields[7].PieceID = rooks[2].PieceID;
            fields[63].PieceID = rooks[3].PieceID;
            ////Set knights
            //////white
            fields[8].PieceID = knights[0].PieceID;
            fields[48].PieceID = knights[1].PieceID;
            //////black
            fields[15].PieceID = knights[2].PieceID;
            fields[55].PieceID = knights[3].PieceID;
            ////Set bishops
            //////white
            fields[16].PieceID = bishops[0].PieceID;
            fields[40].PieceID = bishops[1].PieceID;
            //////black
            fields[23].PieceID = bishops[2].PieceID;
            fields[47].PieceID = bishops[3].PieceID;
            ////Set queens
            //////white
            fields[24].PieceID = queens[0].PieceID;
            //////black
            fields[31].PieceID = queens[1].PieceID;
            ////Set kings
            //////white
            fields[32].PieceID = kings[0].PieceID;
            //////black
            fields[39].PieceID = kings[1].PieceID;
            foreach (Field f in fields)
            {
                context.Fields.Add(f);
            }
            context.SaveChanges();

           




        }
    }
}
