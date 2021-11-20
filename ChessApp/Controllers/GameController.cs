﻿using ChessApp.Data;
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
        public IActionResult Index()
        {
            return View();
        }

        // GET: Game/Play/5        *Edit
        public async Task<IActionResult> Play(int? gameId)
        {
            if (gameId == null)
            {
                return NotFound();
            }

            var game = await _context.Games.Where(s => s.GameID == gameId)
                .Include(s => s.Chessboard)
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
                                    .ThenInclude(e => e.Position)
                  .Include(s => s.Chessboard)
                    .ThenInclude(e => e.BoardsFieldColumns)
                        .ThenInclude(e => e.FieldColumn)
                            .ThenInclude(e => e.Fields)
                                .ThenInclude(e => e.Position)
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
                                    .ThenInclude(e => e.ControlledSquares)
                .Include(s => s.Chessboard)
                    .ThenInclude(e => e.BoardsFieldColumns)
                        .ThenInclude(e => e.FieldColumn)
                            .ThenInclude(e => e.Fields)
                                .ThenInclude(e => e.Content)
                                    .ThenInclude(e => e.NextAvailablePositions)
                

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
            /*
            while (!game.GameState.IsAWin && !game.GameState.IsADraw)
            {
                game.MakeAMove();
                game.RefreshAttackedSquares();
                if (game.GameState.PlayersAgreedToADraw)
                {
                    Console.WriteLine(" Players agreed to a draw.");//d
                    Console.WriteLine(" It's a draw!");//d
                }
                else if (GameState.PlayerResigned)
                {
                    Console.Write($" {GameState.CurrentPlayer} resigned.");//d
                    GameState.ChangeTurns();
                    Console.WriteLine($" {GameState.CurrentPlayer} won the game!");//d
                }
                else if (GameState.IsACheckmate)
                {
                    BoardView.PrintBoard(Chessboard);//d

                    Console.WriteLine(" Checkmate.");//d
                    Console.WriteLine($" {GameState.CurrentPlayer} won the game!");//d
                }
                else if (GameState.IsAStalemate)
                {
                    BoardView.PrintBoard(Chessboard);//d

                    Console.WriteLine(" Stalemate.");//d
                    Console.WriteLine(" It's a draw!");//d
                }
                else
                {
                    BoardView.PrintBoard(Chessboard);//d
                    GameState.ResetEnPassantFlag();
                    GameState.ResetCurrentPiecesAttackingTheKing();
                    GameState.ChangeTurns();
                }
            }*/
            
            return View(game);
        }

        // POST: Game/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("GameStateID, FirstPlayerID, SecondPlayerID")] Game game)
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
            return View(nameof(Play), game);
        }

        public IActionResult Draw()
        {
            return View();
        }

        public IActionResult Checkmate()
        {
            return View();
        }

        public IActionResult AcceptOrDenyADraw()
        {
            return View();
        }

        public IActionResult Resign()
        {
            return View();
        }

        public IActionResult OfferADraw()
        {
            return View();
        }

        // GET: Game/CreateNewGame       *Create       
        public IActionResult CreateNewGame()
        {
            return View();
        }

        // POST: Game/CreateNewGame
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewGame(
             [Bind("GameStateID, FirstPlayerID, SecondPlayerID")] Game game)
        {
            Game newGame = new(_context.Files.ToArray(),
                               _context.Ranks.ToArray(),
                               _context.Positions.ToArray(),
                               _context.Fields.ToArray(),
                               _context.FieldColumns.ToArray());
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
                return RedirectToAction(nameof(Play), new { newGame.GameID });
            }
            return RedirectToAction(nameof(Index));
        }

        /*public void StartGame()
        {
           // SetStartingBoard(); //done

           // RefreshAttackedSquares(); //done

           // BoardView.PrintBoard(Chessboard);//d  done in controller
           /*
            while (!GameState.IsAWin && !GameState.IsADraw)
            {
                MakeAMove();
                RefreshAttackedSquares();
                if (GameState.PlayersAgreedToADraw)
                {
                    Console.WriteLine(" Players agreed to a draw.");//d
                    Console.WriteLine(" It's a draw!");//d
                }
                else if (GameState.PlayerResigned)
                {
                    Console.Write($" {GameState.CurrentPlayer} resigned.");//d
                    GameState.ChangeTurns();
                    Console.WriteLine($" {GameState.CurrentPlayer} won the game!");//d
                }
                else if (GameState.IsACheckmate)
                {
                    BoardView.PrintBoard(Chessboard);//d

                    Console.WriteLine(" Checkmate.");//d
                    Console.WriteLine($" {GameState.CurrentPlayer} won the game!");//d
                }
                else if (GameState.IsAStalemate)
                {
                    BoardView.PrintBoard(Chessboard);//d

                    Console.WriteLine(" Stalemate.");//d
                    Console.WriteLine(" It's a draw!");//d
                }
                else
                {
                    BoardView.PrintBoard(Chessboard);//d
                    GameState.ResetEnPassantFlag();
                    GameState.ResetCurrentPiecesAttackingTheKing();
                    GameState.ChangeTurns();
                }
            }
        }*/

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.GameID == id);
        }
    }
}
