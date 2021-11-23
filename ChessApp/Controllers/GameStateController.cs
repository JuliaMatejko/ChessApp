using ChessApp.Data;
using ChessApp.Models.Chess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> PlayerResigned(
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
            "PlayerOfferedADraw")] GameState gameState)
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
                    await _context.SaveChangesAsync();//data-toggle="modal" data-target="#playerResignedModal"
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
                return RedirectToAction(nameof(GameController.Play), nameof(GameController), new { id = gameState.GameID });
            }
            return RedirectToAction(nameof(GameController.Play), nameof(GameController), new { id = gameState.GameID });
        }

        private bool GameStateExists(int id)
        {
            return _context.GameStates.Any(e => e.GameID == id);
        }
    }
}
