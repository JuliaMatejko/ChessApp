using ChessApp.Data;
using ChessApp.Models.Chess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ChessApp.Controllers
{
    public class GameStateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GameStateController(ApplicationDbContext context)
        {
            _context = context;
        }

        //POST *Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("GameID, " +
            "WhiteKingID, " +
            "BlackKingID, " +
            "WhitePawnThatCanBeTakenByEnPassantMoveID, " +
            "BlackPawnThatCanBeTakenByEnPassantMoveID, " +
            "CurrentPlayer, " +
            "WhiteKingIsInCheck, " +
            "BlackKingIsInCheck, " +
            "PlayersAgreedToADraw, " +
            "PlayerResigned, " +
            "PlayerOfferedADraw, " +
            "IsACheckmate, " +
            "IsAStalemate, " +
            "TimeFlag")] GameState gameState)
        {
            if (id != gameState.GameID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameState);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameStateExists(gameState.GameID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(GameController.Play), nameof(Game), new { id = gameState.GameID });
            }
            return RedirectToAction(nameof(GameController.Play), nameof(Game), new { id = gameState.GameID });
        }

        private bool GameStateExists(int id)
        {
            return _context.GameStates.Any(e => e.GameID == id);
        }
    }
}
