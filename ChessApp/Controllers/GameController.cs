using ChessApp.Data;
using ChessApp.Models.Chess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Games.ToListAsync());
        }

        // GET: Game/Play/5        *Details
        public async Task<IActionResult> Play(int? gameId)
        {
            if (gameId == null)
            {
                return NotFound();
            }

            var game = await _context.Games.Where(s => s.GameID == gameId)
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
                                .ThenInclude(e => e.Content)
                                    .ThenInclude(e => e.ControlledSquares)*/
                .Include(s => s.Chessboard)
                    .ThenInclude(e => e.BoardsFieldColumns)
                        .ThenInclude(e => e.FieldColumn)
                            .ThenInclude(e => e.Fields)
                                .ThenInclude(e => e.Content)
                                    .ThenInclude(e => e.NextAvailablePositions)
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
                    .Include(s => s.Chessboard)
                        .ThenInclude(e => e.BoardsFieldColumns)
                            .ThenInclude(e => e.FieldColumn)
                                .ThenInclude(e => e.Fields)
                                    .ThenInclude(e => e.Content)
                                        .ThenInclude(e => e.Position)

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
                      .ThenInclude(e => e.PromotionTo)
                  .AsSplitQuery()
                  .AsNoTracking()
                  .FirstOrDefaultAsync(m => m.GameID == gameId);

            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Game/StartNewGame       *Create       
        public IActionResult StartNewGame()
        {
            return View();
        }

        // POST: Game/StartNewGame
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartNewGame(
            [Bind("GameID")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Play));
            }
            return View(game);
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.GameID == id);
        }
        /*
        // GET: Game/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        // POST: Game/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameID")] Game game)
        {
            if (id != game.GameID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            return View(game);
        }

        // GET: Game/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Game/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/
    }
}
