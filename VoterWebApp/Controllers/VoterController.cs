using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VoterWebApp.Data;
using VoterWebApp.Models;

namespace VotingWebApp.Controllers
{
    public class VoterController : Controller
    {
        private readonly VotingContext _context;

        public VoterController(VotingContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Voters.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Voter voter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(voter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(voter);
        }

        public async Task<IActionResult> Vote()
        {
            ViewBag.Candidates = await _context.Candidates.ToListAsync();
            ViewBag.Voters = await _context.Voters.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Vote(int voterId, int candidateId)
        {
            var voter = await _context.Voters.FindAsync(voterId);
            if (voter == null || voter.HasVoted)
            {
                ModelState.AddModelError("", "Voter not found or has already voted.");
                ViewBag.Candidates = await _context.Candidates.ToListAsync();
                ViewBag.Voters = await _context.Voters.ToListAsync();
                return View();
            }

            var candidate = await _context.Candidates.FindAsync(candidateId);
            if (candidate == null)
            {
                ModelState.AddModelError("", "Candidate not found.");
                ViewBag.Candidates = await _context.Candidates.ToListAsync();
                ViewBag.Voters = await _context.Voters.ToListAsync();
                return View();
            }

            voter.HasVoted = true;
            candidate.Votes += 1;

            _context.Entry(voter).State = EntityState.Modified;
            _context.Entry(candidate).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}