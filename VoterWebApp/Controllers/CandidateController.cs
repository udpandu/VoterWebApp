using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VoterWebApp.Models;
using VoterWebApp.Data;

namespace VoterWebApp.Controllers
{
    public class CandidateController : Controller
    {
        private readonly VotingContext _context;

        public CandidateController(VotingContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Candidates.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(candidate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(candidate);
        }
    }
}