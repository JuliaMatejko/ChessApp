using ChessApp.Data;
using ChessApp.Models.Chess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ChessApp.Controllers
{
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GameController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Game
        public IActionResult Index()
        {
            return View();
        }

        // POST: Game/CreateNewGame
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewGame(
             [Bind("GameID, FirstPlayerID, SecondPlayerID")] Game game)
        {
            Game newGame = new(_context.Files.ToList(),
                               _context.Ranks.ToList(),
                               _context.Positions.ToList(),
                               _context.FieldColumns.ToList());
            newGame.FirstPlayerID = game.FirstPlayerID;
            newGame.SecondPlayerID = game.SecondPlayerID;
            if (ModelState.IsValid)
            {
                _context.Add(newGame);
                await _context.SaveChangesAsync();
                newGame.SetStartingBoard();
                await _context.SaveChangesAsync();
                newGame.RefreshAttackedSquares();
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Play), new { id = newGame.GameID });
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Game/Play/5        *Edit
        public async Task<IActionResult> Play(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.Where(s => s.GameID == id)
                /*.Include(s => s.Chessboard)
                    .ThenInclude(e => e.BoardsFiles)
                        .ThenInclude(e => e.File)
                .Include(s => s.Chessboard)
                    .ThenInclude(e => e.BoardsRanks)
                        .ThenInclude(e => e.Rank)
                .Include(s => s.Chessboard)
                    .ThenInclude(e => e.BoardsPositions)
                        .ThenInclude(e => e.Position)
                  .Include(s => s.Chessboard)
                    .ThenInclude(e => e.BoardsFieldColumns)
                        .ThenInclude(e => e.FieldColumn)
                            .ThenInclude(e => e.Fields)
                                .ThenInclude(e => e.Position)
                                    .ThenInclude(e => e.File)
                  .Include(s => s.Chessboard)
                    .ThenInclude(e => e.BoardsFieldColumns)
                        .ThenInclude(e => e.FieldColumn)
                            .ThenInclude(e => e.Fields)
                                .ThenInclude(e => e.Position)
                                    .ThenInclude(e => e.Rank)
                  .Include(s => s.Chessboard)
                     .ThenInclude(e => e.BoardsFieldColumns)
                         .ThenInclude(e => e.FieldColumn)
                             .ThenInclude(e => e.Fields)
                                 .ThenInclude(e => e.Content)
                                     .ThenInclude(e => e.GameState)
                   .Include(s => s.Chessboard)
                        .ThenInclude(e => e.BoardsFieldColumns)
                            .ThenInclude(e => e.FieldColumn)
                                .ThenInclude(e => e.Fields)
                                    .ThenInclude(e => e.Content)
                                        .ThenInclude(e => e.Name)
                                            .OrderBy(e => e.GameID)
                   .Include(s => s.Chessboard)
                        .ThenInclude(e => e.BoardsFieldColumns)
                            .ThenInclude(e => e.FieldColumn)
                                .ThenInclude(e => e.Fields)
                                    .ThenInclude(e => e.Position)
                                        .OrderBy(e => e.GameID)
                   .Include(s => s.Chessboard)
                       .ThenInclude(e => e.BoardsFieldColumns)
                           .ThenInclude(e => e.FieldColumn)
                               .ThenInclude(e => e.Fields)
                                   .ThenInclude(e => e.Content)
                                       .ThenInclude(e => e.ControlledSquares)
                                            .OrderBy(e => e.GameID)
                   .Include(s => s.Chessboard)
                       .ThenInclude(e => e.BoardsFieldColumns)
                           .ThenInclude(e => e.FieldColumn)
                               .ThenInclude(e => e.Fields)
                                   .ThenInclude(e => e.Content)
                                       .ThenInclude(e => e.NextAvailablePositions)
                                            .OrderBy(e => e.GameID)*/
                    .Include(s => s.Chessboard)
                        .ThenInclude(e => e.Fields)
                            .ThenInclude(e => e.Position)
                                    .OrderBy(e => e.GameID)
                    .Include(s => s.Chessboard)
                        .ThenInclude(e => e.Fields)
                            .ThenInclude(e => e.Content)
                                    .OrderBy(e => e.GameID)
                    .Include(s => s.GameState)
                  /*
                                       .Include(s => s.GameState)
                                           .ThenInclude(e => e.WhiteKing)
                                       .Include(s => s.GameState)
                                           .ThenInclude(e => e.BlackKing)
                                       .Include(s => s.GameState)
                                           .ThenInclude(e => e.WhitePawnThatCanBeTakenByEnPassantMove)
                                       .Include(s => s.GameState)
                                           .ThenInclude(e => e.BlackPawnThatCanBeTakenByEnPassantMove)
                                       .Include(s => s.GameState)
                                           .ThenInclude(e => e.CurrentPlayerPiecesAttackingTheKing)

                                       .Include(s => s.Moves)
                                           .ThenInclude(e => e.PieceName)
                                       .Include(s => s.Moves)
                                           .ThenInclude(e => e.CurrentPosition)
                                       .Include(s => s.Moves)
                                           .ThenInclude(e => e.NewPosition)
                                       .Include(s => s.Moves)
                                           .ThenInclude(e => e.PromotionTo)*/
                  //.AsSplitQuery()
                  .AsNoTracking()
                  .FirstOrDefaultAsync();

            if (game == null)
            {
                return NotFound();
            }

            if (game.GameState.IsAWin || game.GameState.IsADraw)
            {
                return View(nameof(EndOfTheGame), game);
                //TO DO: else if - 3 move rule repetion, 5 move rule repetion, time vs not enough material, 50 move rule, 75 move rule
                //TO DO: else if (time flag)
            }
            else
            {
                return View(game);
            }
        }

        public IActionResult EndOfTheGame()
        {
            return View();
        }

        // POST: Game/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("GameID, FirstPlayerID, SecondPlayerID")] Game game)
        {
            if (id != game.GameID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    game.GameState.ResetEnPassantFlag();
                    game.GameState.ResetCurrentPiecesAttackingTheKing();
                    game.GameState.ChangeTurns();
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GameID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Play), new { id = game.GameID });
            }
            return View(nameof(Play), game);
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.GameID == id);
        }
    }
}
