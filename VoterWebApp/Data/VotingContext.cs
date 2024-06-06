using Microsoft.EntityFrameworkCore;
using VoterWebApp.Models;

namespace VoterWebApp.Data
{
    public class VotingContext :DbContext
    {
        public VotingContext(DbContextOptions<VotingContext> options) : base(options) { }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Voter> Voters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial data
            modelBuilder.Entity<Candidate>().HasData(
                new Candidate { Id = 1, Name = "Candidate 1", Votes = 0 },
                new Candidate { Id = 2, Name = "Candidate 2", Votes = 0 }
            );

            modelBuilder.Entity<Voter>().HasData(
                new Voter { Id = 1, Name = "Voter 1", HasVoted = false },
                new Voter { Id = 2, Name = "Voter 2", HasVoted = false }
            );
        }
    }

}
