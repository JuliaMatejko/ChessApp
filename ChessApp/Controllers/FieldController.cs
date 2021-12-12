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
    public class FieldController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FieldController(ApplicationDbContext context)
        {
            _context = context;
        }

        //POST *Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("GameID, " +
            "PositionID, " +
            "PieceGameID, " +
            "PieceID, " +
            "FieldColumnID")] Field field)
        {
            if (id != field.GameID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(field);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FieldExists(field.GameID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(GameController.Play), nameof(Game), new { id = field.GameID });
            }
            return RedirectToAction(nameof(GameController.Play), nameof(Game), new { id = field.GameID });
        }

        private bool FieldExists(int id)
        {
            return _context.Fields.Any(e => e.GameID == id);
        }
    }
}
